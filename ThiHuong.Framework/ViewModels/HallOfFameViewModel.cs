using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiHuong.Framework.ViewModels
{
    public class HallOfFameViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Address { get; set; }
        public string ExamName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public float? Point { get; set; }
    }
}
