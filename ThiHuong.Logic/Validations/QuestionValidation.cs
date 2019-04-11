using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework.Constants;
using ThiHuong.Framework.Models;

namespace ThiHuong.Logic.Validations
{
    public class QuestionValidation : BaseValidation
    {
        public QuestionValidation(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool IsUntextQuestion(Question question)
        {
            return question != null && question.Type.Equals(QuestionType.UNTEXT);
        }

        public bool IsValidQuestionToCreate(Question question)
        {
            var result = !IsExist(question.Id) //Id chưa tồn tại
                && IsEnoughFourChoicesAndAnswer(question)
                && IsValidAnswer(question)
                && question.IsActive != null;

            if (question.Type.Equals(QuestionType.UNTEXT))
            {
                result = result && IsValidUntextQuestion(question);
            }
            else
            {
                result = result && IsValidTextQuestion(question);
            }
            return result;
        }

        public bool IsEnoughFourChoicesAndAnswer(Question question)
        {
            return !string.IsNullOrEmpty(question.A)
                && !string.IsNullOrEmpty(question.B)
                && !string.IsNullOrEmpty(question.C)
                && !string.IsNullOrEmpty(question.D)
                && !string.IsNullOrEmpty(question.Answer);
        }

        public bool IsValidAnswer(Question question)
        {
            var choices = new List<string>() { "a", "b", "c", "d" };
            return choices.Contains(question.Answer.Trim().ToLower());
        }

        public bool IsValidUntextQuestion(Question question) {
            return question != null && question.Type.Equals(QuestionType.UNTEXT) && !string.IsNullOrEmpty(question.Path);
        }
        public bool IsValidTextQuestion(Question question)
        {
            return question != null && question.Type.Equals(QuestionType.TEXT) && string.IsNullOrEmpty(question.Path);
        }

        public override bool IsActive(object Id)
        {
            var question = this.unitOfWork.QuestionRepository.FindAsync(Id).Result;
            return IsActive(question);
        }

        public bool IsActive(Question question)
        {
            return question != null && !(question.IsActive ?? true);
        }

        public bool IsExist(Question question)
        {
            return question != null;
        }

        public override bool IsExist(object Id)
        {
            var question = this.unitOfWork.QuestionRepository.FindAsync(Id).Result;
            return IsExist(question);
        }
    }
}
