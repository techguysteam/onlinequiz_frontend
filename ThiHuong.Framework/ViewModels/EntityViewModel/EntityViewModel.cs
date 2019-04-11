using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiHuong.Framework.ViewModels.EntityViewModel
{
    public partial class ExamViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int? Duration { get; set; }
        public int? TotalQuestion { get; set; }
        public string Status { get; set; }
        public DateTime? OpenTime { get; set; }
        public int? StageId { get; set; }
        public int? Year { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public partial class QuestionViewModel : BaseViewModel
    {
        public int Id { get; set; }
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value.Trim(); }
        }

        private string path;

        public string Path
        {
            get { return path; }
            set { path = value.Trim(); }
        }
        private string content;

        public string Content
        {
            get { return content; }
            set { content = value.Trim(); }
        }

        private string a;

        public string A
        {
            get { return a; }
            set { a = value.Trim(); }
        }

        private string b;

        public string B
        {
            get { return B; }
            set { B = value.Trim(); }
        }

        private string c;

        public string C
        {
            get { return C; }
            set { C = value.Trim(); }
        }

        private string d;

        public string D
        {
            get { return D; }
            set { D = value.Trim(); }
        }

        private string answer;

        public string Answer
        {
            get { return answer; }
            set { answer = value.Trim(); }
        }

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
