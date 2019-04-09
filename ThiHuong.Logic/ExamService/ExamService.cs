using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework.Models;
using ThiHuong.Framework.ViewModels.EntityViewModel;
using ThiHuong.Logic.BaseRepository;
using ThiHuong.Logic.BaseService;

namespace ThiHuong.Logic.ExamService
{
    public interface IExamService : IBaseService<Exam>
    {
        void CreateExam(ExamViewModel exam);
    }

    public class ExamService : BaseService<Exam>, IExamService
    {
        public ExamService(UnitOfWork unitOfWork) 
            : base(unitOfWork.ExamRepository, unitOfWork)
        {
        }

        public void CreateExam(ExamViewModel exam)
        {

            this.repository.AddAsync(exam.ToEntity<Exam>());
        }
    }
}
