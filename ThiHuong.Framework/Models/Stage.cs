using System;
using System.Collections.Generic;

namespace ThiHuong.Framework.Models
{
    public partial class Stage
    {
        public Stage()
        {
            Exam = new HashSet<Exam>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Exam> Exam { get; set; }
    }
}
