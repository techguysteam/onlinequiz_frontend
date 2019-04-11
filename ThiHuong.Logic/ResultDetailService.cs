using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework;
using ThiHuong.Framework.Constants;
using ThiHuong.Framework.Models;
using ThiHuong.Framework.ViewModels;
using ThiHuong.Logic.BaseRepository;
using ThiHuong.Logic.BaseService;
using ThiHuong.Logic.Validations;

namespace ThiHuong.Logic
{
    public interface IResultDetailService : IBaseService<ResultDetail>
    {
        Task SubmitAnswerPartialAsync(List<SubmitAnswerViewModel> answers, int accountId);
        Task CalculatePoint(int accountId, int examId);
    }

    public class ResultDetailService : BaseService<ResultDetail>, IResultDetailService
    {
        public ResultDetailService(IBaseRepository<ResultDetail> repository, UnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public async Task CalculatePoint(int accountId, int examId)
        {
            var resultDetails = this.repository.Get(rd => rd.AccountId == accountId && rd.ExamId == examId).ToList();
            var questionIds = resultDetails.Select(rd => rd.QuestionId);
            var questionsInExam = this.unitOfWork.QuestionRepository.Get(q => questionIds.Contains(q.Id)).ToList();
            float point = 0;
            resultDetails.ForEach(rd =>
            {
                var question = questionsInExam.FirstOrDefault(q => q.Id == rd.QuestionId);
                rd.Answer = question.Answer;
                rd.IsCorrect = false;
                rd.Point = question.Point;
                if (question.Answer.Equals(rd.Answer, StringComparison.OrdinalIgnoreCase))
                {
                    point += question.Point ?? 0;
                    rd.IsCorrect = true;
                }
            });
            this.repository.UpdateRange(resultDetails);

            //Update AccountInStage
            var currentAccount = this.unitOfWork.AccountInStageRepository
                                                        .Get(a => a.AccountId == accountId && a.FinishTime == null,
                                                             a => a.OrderByDescending(acc => acc.Id))
                                                        .FirstOrDefault();
            currentAccount.FinishTime = DateTime.UtcNow;
            currentAccount.Point = point;
            this.unitOfWork.AccountInStageRepository.Update(currentAccount);

            //Save change
            await this.unitOfWork.SaveChangesAsync();

        }

        public async Task SubmitAnswerPartialAsync(List<SubmitAnswerViewModel> answers, int accountId)
        {
            //check answer list has elements
            if (answers == null || answers.Count <= 0) throw new ThiHuongException("No answer");

            answers.ForEach(a => a.AccountId = accountId);
            var resultDetails = answers.ToListEntity<SubmitAnswerViewModel, ResultDetail>();

            //check valid exam
            var examId = resultDetails.First().ExamId;
            var examValidation = new ExamValidation(this.unitOfWork);
            if (!examValidation.IsPublicExam(examId)) throw new ThiHuongException(ErrorMessage.EXAM_NOT_PUBLIC);

            var questionIds = resultDetails.Select(rd => rd.QuestionId);

            //Find and update the answers that have already submitted
            var resultDetailsAlreadyAdded = this.repository.Get(rd => rd.ExamId == examId
                                                                      && rd.AccountId == accountId
                                                                      && questionIds.Contains(rd.QuestionId))
                                                           .ToList();

            if (resultDetailsAlreadyAdded != null && resultDetailsAlreadyAdded.Count > 0)
            {
                resultDetailsAlreadyAdded.ForEach(answer =>
                {
                    var updatedAnswer = resultDetails.Where(rd => rd.QuestionId == answer.QuestionId).FirstOrDefault();
                    answer.Choice = updatedAnswer.Choice;
                });
                questionIds = resultDetailsAlreadyAdded.Select(rd => rd.QuestionId);

                this.repository.UpdateRange(resultDetailsAlreadyAdded);
                //After updating the answer, remove it from the result detail list
                resultDetails.RemoveAll(rd => questionIds.Contains(rd.QuestionId));
            }

            await this.repository.AddRangeAsync(resultDetails);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
