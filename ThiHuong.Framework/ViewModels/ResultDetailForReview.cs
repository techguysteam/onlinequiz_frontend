using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiHuong.Framework.ViewModels
{
    public class ResultDetailForReview : BaseViewModel
    {
        public string Type { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string Answer { get; set; }
        public float? Point { get; set; }

        public string Choice { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
