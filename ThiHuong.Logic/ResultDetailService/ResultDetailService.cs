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
        Task SubmitAnswerPartialAsync(List<SubmitAnswerViewModel> answers, int accountId, int examId);
        Task CalculatePoint(int accountId, int examId);

    }

    public class ResultDetailService : BaseService<ResultDetail>, IResultDetailService
    {
        private ResultDetailValidation resultDetailValidation;
        public ResultDetailService(UnitOfWork unitOfWork)
            : base(unitOfWork.ResultDetailRepository, unitOfWork)
        {
            this.resultDetailValidation = new ResultDetailValidation(this.unitOfWork);
        }

        public async Task CalculatePoint(int accountId, int examId)
        {
            //get account in stage Id
            var accountInStage = this.unitOfWork.AccountInStageRepository.Get(a => a.AccountId == accountId && a.ExamId == examId).First();

            //get all answers
            var resultDetails = this.repository.Get(rd => rd.AccountInStageId == accountInStage.Id).ToList();
            var questionIds = resultDetails.Select(rd => rd.QuestionId);
            var questionsInExam = this.unitOfWork.QuestionRepository.Get(q => questionIds.Contains(q.Id)).ToList();

            //calculate point
            float point = 0;
            resultDetails.ForEach(rd =>
            {
                var question = questionsInExam.FirstOrDefault(q => q.Id == rd.QuestionId);
                rd.Answer = question.Answer;
                rd.IsCorrect = false;
                rd.Point = question.Point;
                if (question.Answer.Equals(rd.Choice, StringComparison.OrdinalIgnoreCase))
                {
                    point += question.Point ?? 1;
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

        

        public async Task SubmitAnswerPartialAsync(List<SubmitAnswerViewModel> answers, int accountId, int examId)
        {
            //if allow to submit answer
            var accountInStageValidation = new AccountInStageValidation(this.unitOfWork);
            var accountInStage = this.unitOfWork.AccountInStageRepository.Get(a => a.AccountId == accountId && a.ExamId == examId).First();
            if (!accountInStageValidation.IsValidToSubmitAnswer(accountInStage)) throw new ThiHuongException(ErrorMessage.EXAM_ALREADY_TAKEN);
            
            //check answer list has elements
            if (answers == null || answers.Count <= 0) throw new ThiHuongException("No answer");

            answers.ForEach(a => a.AccountId = accountId);
            var resultDetails = answers.ToListEntity<ResultDetail>();

           

            //Find and update the answers that have already submitted
            var questionIds = resultDetails.Select(rd => rd.QuestionId).ToList();

            //check answer contains valid question in ExamDetail
            var examDetails = this.unitOfWork.ExamDetailRepository.Get(rd => rd.ExamId == examId).ToList();
            questionIds.ForEach(qId =>
            {
                if (examDetails.FirstOrDefault(ed => ed.QuestionId == qId) == null)
                {
                    var rdt = resultDetails.First(rd => rd.QuestionId == qId);
                    resultDetails.Remove(rdt);
                }
            });


            var resultDetailsAlreadyAdded = this.repository.Get(rd => rd.AccountInStageId == accountInStage.Id && questionIds.Contains(rd.QuestionId))
                                                           .ToList();

            if (resultDetailsAlreadyAdded != null && resultDetailsAlreadyAdded.Count > 0)
            {
                resultDetailsAlreadyAdded.ForEach(answer =>
                {
                    var updatedAnswer = resultDetails.Where(rd => rd.QuestionId == answer.QuestionId).FirstOrDefault();
                    answer.Choice = updatedAnswer.Choice;
                });
                questionIds = resultDetailsAlreadyAdded.Select(rd => rd.QuestionId).ToList();

                this.repository.UpdateRange(resultDetailsAlreadyAdded);
                //After updating the answer, remove it from the result detail list
                resultDetails.RemoveAll(rd => questionIds.Contains(rd.QuestionId));
            }
            resultDetails.ForEach(rd => rd.AccountInStageId = accountInStage.Id);

            await this.repository.AddRangeAsync(resultDetails);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
