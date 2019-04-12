using System;
using System.Collections.Generic;

namespace ThiHuong.Framework.Models
{
    public partial class ExamDetail
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual Question Question { get; set; }
    }
}
