using Microsoft.AspNetCore.Http;
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
using ThiHuong.Logic.Validations;

namespace ThiHuong.Logic.ExamService
{
    public interface IExamService : IBaseService<Exam>
    {
        Task CreateExamAsync(ExamViewModel examViewModel);
        Task<string> GenerateCodeAsync(int examId);
        Task<List<QuestionForExamination>> Enroll(int accountId, ExamEnrollmentViewModel enrollment);
    }

    public class ExamService : BaseService<Exam>, IExamService
    {
        private IHttpContextAccessor httpContextAccessor;
        private ExamValidation examValidation;

        public ExamService(IHttpContextAccessor httpContextAccessor, UnitOfWork unitOfWork)
            : base(unitOfWork.ExamRepository, unitOfWork)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.examValidation = new ExamValidation(this.unitOfWork);
        }

        public async Task CreateExamAsync(ExamViewModel examViewModel)
        {
            var exam = examViewModel.ToEntity<Exam>();
            exam.Status = StatusConstant.PENDING_EXAM;

            //check valid exam
            if (examValidation.IsvalidExamEntityToCreate(exam))
            {
                await this.repository.AddAsync(exam);
                await this.unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<List<QuestionForExamination>> Enroll(int accountId, ExamEnrollmentViewModel enrollment)
        {
            //check exam found or not
            if (!examValidation.IsExist(enrollment.ExamId))
                throw new ThiHuongException(ErrorMessage.EXAM_NOT_FOUND);

            var exam = await this.repository.FindAsync(enrollment.ExamId);
            
            if (!examValidation.IsValidExamToEnroll(exam)) throw new ThiHuongException(ErrorMessage.EXAM_NOT_PUBLIC);

            //check code valid
            bool validCode = string.Compare(exam.Code, enrollment.Code) == 0;

            if (!validCode) throw new ThiHuongException(ErrorMessage.CODE_NOT_VALID);

            //create account in stage if not in the system
            var accountInStageValidation = new AccountInStageValidation(this.unitOfWork);
            if(!accountInStageValidation.IsAccountInStage(accountId, enrollment.ExamId))
            {
                IAccountInStageService accountInStageService =
                new AccountInStageService.AccountInStageService(this.unitOfWork.AccountInStageRepository, this.unitOfWork);
                await accountInStageService.CreateAccountEnrollmentAsync(accountId, enrollment.ExamId);
            }

            //check account valid to enroll

            var accountInStage = this.unitOfWork.AccountInStageRepository.Get(a => a.AccountId == accountId && a.ExamId == exam.Id)
                                                                         .First();
            if ( ! accountInStageValidation.IsValidAccountInStageToEnroll(accountInStage) ) throw new ThiHuongException(ErrorMessage.EXAM_ALREADY_TAKEN);

            //Get question by exam
            var questions = exam.ExamDetail.Select(ed => ed.Question).ToList()
                                           .ToListViewModel<Question, QuestionForExamination>();
            return questions;
        }

        public async Task<string> GenerateCodeAsync(int examId)
        {
            var exam = await this.repository.FindAsync(examId);

            if (exam == null) throw new ThiHuongException("Exam not found");

            string code = null;
            if (examValidation.IsValidToGenerateCode(exam))
            {
                if (string.IsNullOrEmpty(exam.Code))
                {
                    code = CodeGenerator.GetUniqueCode(Constant.TotalCodeLength);
                    exam.Code = code;
                    this.repository.Update(exam);
                    await this.unitOfWork.SaveChangesAsync();
                }
            }
            return exam.Code;
        }

    }
}
