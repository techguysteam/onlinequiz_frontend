using System;
using System.Collections.Generic;

namespace ThiHuong.Framework.Models
{
    public partial class AccountInStage
    {
        public AccountInStage()
        {
            ResultDetail = new HashSet<ResultDetail>();
        }

        public int Id { get; set; }
        public int AccountId { get; set; }
        public int? ExamId { get; set; }
        public int? Rank { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public float? Point { get; set; }

        public Account Account { get; set; }
        public Exam Exam { get; set; }
        public ICollection<ResultDetail> ResultDetail { get; set; }
    }
}
