using System;
using System.Collections.Generic;

namespace ThiHuong.Framework.Models
{
    public partial class ResultDetail
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public int AccountId { get; set; }
        public string Choice { get; set; }
        public bool? IsCorrect { get; set; }
        public float? Point { get; set; }
        public string Answer { get; set; }

        public AccountInStage Account { get; set; }
    }
}
