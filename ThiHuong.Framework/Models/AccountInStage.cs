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
        public int StageId { get; set; }
        public int AccountId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public float? Point { get; set; }
        public bool? IsTalent { get; set; }
        public int? ExamId { get; set; }

        public Account Account { get; set; }
        public Stage Stage { get; set; }
        public ICollection<ResultDetail> ResultDetail { get; set; }
    }
}
