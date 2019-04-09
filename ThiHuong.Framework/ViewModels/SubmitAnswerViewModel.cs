using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiHuong.Framework.ViewModels
{
    public class SubmitAnswerViewModel : BaseViewModel
    {
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public int AccountId { get; set; }
        public string Choice { get; set; }
    }
}
