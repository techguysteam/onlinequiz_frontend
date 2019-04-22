using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        Task AddQuestionIntoExam(int examId, int questionId);
        Task RemoveQuestionFromExam(int examId, int questionId);

        Task<BasePagination> GetExamPagination(int size = 10, int page = 0);
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

        public async Task AddQuestionIntoExam(int examId, int questionId)
        {
            //exam must be PENDING
            if (!examValidation.IsExist(examId)) throw new ThiHuongException(ErrorMessage.EXAM_NOT_FOUND);
            if (!examValidation.IsPendingExam(examId)) throw new ThiHuongException(ErrorMessage.EXAM_NOT_IN_PENDING);

            //check question is valid
            var questionValidation = new QuestionValidation(this.unitOfWork);
            if (!questionValidation.IsActive(questionId)) throw new ThiHuongException(ErrorMessage.QUESTION_NOT_FOUND);

            //check question is already in the exam
            var examDetailValidation = new ExamDetailValidation(this.unitOfWork);
            if (examDetailValidation.IsQuestionAlreadyInExam(questionId, examId)) throw new ThiHuongException(ErrorMessage.QUESTION_ALREADY_IN_EXAM);

            //add question to exam
            var examDetail = new ExamDetail()
            {
                ExamId = examId,
                QuestionId = questionId
            };
            await this.unitOfWork.ExamDetailRepository.AddAsync(examDetail);
            await this.unitOfWork.SaveChangesAsync();
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
            else
            {
                throw new ThiHuongException(ErrorMessage.EXAM_INVALID_TO_CREATE);
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
            if (!accountInStageValidation.IsAccountInStage(accountId, enrollment.ExamId))
            {
                IAccountInStageService accountInStageService =
                new AccountInStageService.AccountInStageService(this.unitOfWork);
                await accountInStageService.CreateAccountEnrollmentAsync(accountId, enrollment.ExamId);
            }

            //check account valid to enroll

            var accountInStage = this.unitOfWork.AccountInStageRepository.Get(a => a.AccountId == accountId && a.ExamId == exam.Id)
                                                                         .First();
            if (!accountInStageValidation.IsValidAccountInStageToEnroll(accountInStage)) throw new ThiHuongException(ErrorMessage.EXAM_ALREADY_TAKEN);

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

        public async Task<BasePagination> GetExamPagination(int size = 10, int page = 0)
        {
            var exams = await this.repository.Get(null, exam => exam.OrderBy(e => e.Id)).ToListAsync();
            var totalExam = this.repository.Get().Count();

            return new BasePagination()
            {
                Content = exams.ToListViewModel<Exam, ExamViewModel>(),
                Page = page,
                Size = size,
                Total = totalExam
            };
        }

        public async Task RemoveQuestionFromExam(int examId, int questionId)
        {
            //check if exam is public
            if (!examValidation.IsPendingExam(examId)) throw new ThiHuongException(ErrorMessage.EXAM_NOT_IN_PENDING);

            //check question must be in system
            var examDetailValidation = new ExamDetailValidation(this.unitOfWork);
            if (!examDetailValidation.IsQuestionAlreadyInExam(questionId, examId)) throw new ThiHuongException(ErrorMessage.QUESTION_NOT_IN_EXAM);

            var examDetail = this.unitOfWork.ExamDetailRepository.Get(ed => ed.ExamId == examId && ed.QuestionId == questionId).First();
            this.unitOfWork.ExamDetailRepository.Delete(examDetail);
            await this.unitOfWork.SaveChangesAsync();
        }
    }
}
