using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiHuong.Framework.ViewModels.EntityViewModel
{
    public partial class ExamViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public DateTime? Duration { get; set; }
        public int? TotalQuestion { get; set; }
        public string Status { get; set; }
        public DateTime? OpenTime { get; set; }
        public int? StageId { get; set; }
        public int? Year { get; set; }
        public string Name { get; set; }
    }

    public partial class QuestionViewModel : BaseViewModel
    {
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
    }

    public partial class ResultDetailViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Choice { get; set; }
        public bool? IsCorrect { get; set; }
        public float? Point { get; set; }
        public string Answer { get; set; }

    }

    public partial class Stage : BaseViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }

    }

}
