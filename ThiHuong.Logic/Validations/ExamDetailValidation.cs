using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiHuong.Logic.Validations
{
    public class ExamDetailValidation : BaseValidation
    {
        public ExamDetailValidation(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool IsQuestionAlreadyInExam(int questionId, int examId)
        {
            var examDetail = this.unitOfWork.ExamDetailRepository.Get(ed => ed.ExamId == examId && ed.QuestionId == questionId).FirstOrDefault();
            return examDetail != null;
        }

        public override bool IsActive(object Id)
        {
            throw new NotImplementedException();
        }

        public override bool IsExist(object Id)
        {
            throw new NotImplementedException();
        }
    }
}
