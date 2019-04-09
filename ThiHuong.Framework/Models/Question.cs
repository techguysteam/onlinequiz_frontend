using System;
using System.Collections.Generic;

namespace ThiHuong.Framework.Models
{
    public partial class Question
    {
        public Question()
        {
            ExamDetail = new HashSet<ExamDetail>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string Answer { get; set; }
        public float? Point { get; set; }
        public bool? IsActive { get; set; }

        public ICollection<ExamDetail> ExamDetail { get; set; }
    }
}
