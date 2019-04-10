using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework;
using ThiHuong.Framework.Constants;
using ThiHuong.Framework.Helpers;
using ThiHuong.Framework.Models;
using ThiHuong.Framework.ViewModels;
using ThiHuong.Framework.ViewModels.EntityViewModel;
using ThiHuong.Logic.AccountInStageService;
using ThiHuong.Logic.BaseRepository;
using ThiHuong.Logic.BaseService;

namespace ThiHuong.Logic.ExamService
{
    public interface IExamService : IBaseService<Exam>
    {
        void CreateExam(ExamViewModel exam);
        Task<string> GenerateCode(int examId);
        Task<List<QuestionForExamination>> Enroll(int accountId, ExamEnrollmentViewModel enrollment);
    }

    public class ExamService : BaseService<Exam>, IExamService
    {
        public ExamService(UnitOfWork unitOfWork)
            : base(unitOfWork.ExamRepository, unitOfWork)
        {
        }

        public void CreateExam(ExamViewModel exam)
        {

            this.repository.AddAsync(exam.ToEntity<Exam>());
        }

        public async Task<List<QuestionForExamination>> Enroll(int accountId, ExamEnrollmentViewModel enrollment)
        {
            var exam = await this.repository.FindAsync(enrollment.ExamId);

            //check stage and exam
            if (await IsValidStage(exam.StageId.Value) == false
                | await IsvalidExamToEnroll(exam.Id) == false) throw new ThiHuongException("Exam is not valid");

            IAccountInStageService accountInStageService =
                new AccountInStageService.AccountInStageService(this.unitOfWork.AccountInStageRepository, this.unitOfWork); ;

            bool validCode = string.Compare(exam.Code, enrollment.Code) == 0;

            if (!validCode) throw new ThiHuongException("The code is not valid");

            accountInStageService.CreateAccountEnrollment(accountId, enrollment.ExamId, exam.StageId.Value);

            var questions = exam.ExamDetail.Select(ed => ed.Question).ToList()
                                           .ToListViewModel<Question, QuestionForExamination>();
            return questions;
        }

        public async Task<string> GenerateCode(int examId)
        {
            var exam = await this.repository.FindAsync(examId);

            if (exam == null) throw new ThiHuongException("Exam not found");

            string code = null;
            if (await IsValidStage(exam.StageId.Value) && DateTime.Compare(DateTime.UtcNow, exam.OpenTime.Value) <= 0)
            {
                if (string.IsNullOrEmpty(exam.Code))
                {
                    code = CodeGenerator.GetUniqueCode(Constant.TotalCodeLength);
                    exam.Code = code;
                    this.repository.Update(exam);
                    this.unitOfWork.SaveChangesAsync();
                }
            }

            return exam.Code;
        }

        private async Task<bool> IsValidStage(int stageId)
        {
            var stage = await this.unitOfWork.StageRepository.FindAsync(stageId);
            if (stage != null)
            {
                if (DateTime.UtcNow.IsValid(stage.StartDate.Value, stage.EndDate.Value))
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> IsvalidExamToEnroll(int examId)
        {
            var exam = await this.repository.FindAsync(examId);

            if (exam == null) throw new ThiHuongException("Exam not found");

            if (DateTime.Compare(DateTime.UtcNow, exam.OpenTime.Value) >= 0)
            {
                return true;
            }
            return false;
        }
    }
}
