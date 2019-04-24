using System;
using System.Collections.Generic;

namespace ThiHuong.Framework.Models
{
    public partial class Exam
    {
        public Exam()
        {
            AccountInStage = new HashSet<AccountInStage>();
            ExamDetail = new HashSet<ExamDetail>();
        }

        public int? Duration { get; set; }
        public int? TotalQuestion { get; set; }
        public string Status { get; set; }
        public DateTime? OpenTime { get; set; }
        public int? StageId { get; set; }
        public int? Year { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual Stage Stage { get; set; }
        public virtual ICollection<AccountInStage> AccountInStage { get; set; }
        public virtual ICollection<ExamDetail> ExamDetail { get; set; }
    }
}
