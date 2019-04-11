using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework.Constants;
using ThiHuong.Framework.Models;

namespace ThiHuong.Logic.Validations
{
    public class ExamValidation : BaseValidation
    {
        public ExamValidation(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool IsvalidExamEntityToCreate(Exam exam)
        {
            var stageValidation = new StageValidation(this.unitOfWork);

            return IsValidDuration(exam)
                && IsValidOpenTimeToCreate(exam)
                && IsValidTotalQuestion(exam)
                && IsValidYearInExam(exam)
                && ! stageValidation.IsPublicStage(exam.StageId);
        }

        public bool IsValidExamToEnroll(Exam exam)
        {
            return IsPublicExam(exam);
        }

        public bool IsValidToGenerateCode(Exam exam)
        {
            return !IsPublicExam(exam) // chưa public
                && !IsCodeExist(exam); // chưa gen code
        }

        public bool IsCodeExist(Exam exam)
        {
            return exam != null && exam.Code != null;
        }

        public bool IsValidYearInExam(Exam exam)
        {
            return exam.Year != null && exam.Year.Value >= DateTime.Now.Year;
        }

        public bool IsValidOpenTimeToCreate(Exam exam)
        {
            return exam.OpenTime != null && DateTime.Compare(DateTime.UtcNow, exam.OpenTime.Value) >= 0; //opentime after current time
        }

        public bool IsValidDuration(Exam exam)
        {
            return exam != null && exam.Duration != null && exam.Duration.Value > 0;
        }

        public bool IsValidTotalQuestion(Exam exam)
        {
            return exam != null && exam.TotalQuestion != null && exam.TotalQuestion.Value > 0;
        }

        public bool IsPublicExam(Exam exam)
        {
            return exam.Status.Equals(StatusConstant.PUBLIC_EXAM, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsPublicExam(object Id)
        {
            var exam = this.unitOfWork.ExamRepository.FindAsync(Id).Result;
            return exam != null && IsPublicExam(exam);
        }

        public override bool IsActive(object Id)
        {
            throw new NotImplementedException();
        }

        public override bool IsExist(object Id)
        {
            return this.unitOfWork.ExamRepository.FindAsync(Id).Result != null;
        }
    }
}
