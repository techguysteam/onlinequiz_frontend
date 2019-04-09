using System;
using System.Collections.Generic;

namespace ThiHuong.Framework.Models
{
    public partial class Stage
    {
        public Stage()
        {
            AccountInStage = new HashSet<AccountInStage>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }

        public ICollection<AccountInStage> AccountInStage { get; set; }
    }
}
