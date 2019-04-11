using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework;
using ThiHuong.Framework.Constants;
using ThiHuong.Framework.Models;
using ThiHuong.Framework.ViewModels.EntityViewModel;
using ThiHuong.Logic.BaseRepository;
using ThiHuong.Logic.BaseService;
using ThiHuong.Logic.Validations;

namespace ThiHuong.Logic.QuestionService
{
    public interface IQuestionService : IBaseService<Question>
    {
        Task<QuestionViewModel> CreateQuestionAsync(QuestionViewModel question, IFormFile file, string pathInServer);
        Task<List<QuestionViewModel>> GetQuestionByExamId(int examId);
    }

    public class QuestionService : BaseService<Question>, IQuestionService
    {
        private IHttpContextAccessor httpContextAccessor;
        private IHostingEnvironment env;
        private QuestionValidation questionValidation;

        public QuestionService(UnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env) 
            : base(unitOfWork.QuestionRepository, unitOfWork)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.env = env;
            this.questionValidation = new QuestionValidation(this.unitOfWork);
        }

        public async Task<QuestionViewModel> CreateQuestionAsync(QuestionViewModel questionViewModel, IFormFile file, string pathInServerWithoutHost)
        {
            var question = questionViewModel.ToEntity<Question>();

            question.Type = QuestionType.TEXT;

            //prepare the path in server
            string pathInServer = env.WebRootPath;

            if (file != null)
            {
                question.Type = QuestionType.UNTEXT;
                pathInServer = Path.Combine(pathInServer, pathInServerWithoutHost);
            }

            if (!question.Type.Equals(QuestionType.TEXT))
            {
                if (!Directory.Exists(pathInServer))
                {
                    Directory.CreateDirectory(pathInServer);
                }
                pathInServer = Path.Combine(pathInServer, file.FileName);
                using (FileStream stream = File.Create(pathInServer))
                {
                    await file.OpenReadStream().CopyToAsync(stream);

                    //prepare link to access from browser
                    var hostName = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
                    var uriBuilder = new System.UriBuilder(hostName);
                    uriBuilder.Path = Path.Combine(uriBuilder.Path, pathInServerWithoutHost, file.FileName);
                    question.Path = uriBuilder.Uri.AbsoluteUri;
                }
            }
            question.IsActive = true;

            if (!questionValidation.IsValidQuestionToCreate(question)) throw new ThiHuongException(ErrorMessage.QUESTION_NOT_VALID_TO_CREATE);

            await this.repository.AddAsync(question);
            await this.unitOfWork.SaveChangesAsync();

            return question.ToViewModel<QuestionViewModel>();
        }

        public async Task<List<QuestionViewModel>> GetQuestionByExamId(int examId)
        {
            //valid exam
            var examValidation = new ExamValidation(this.unitOfWork);
            if (!examValidation.IsExist(examId)) throw new ThiHuongException(ErrorMessage.EXAM_NOT_FOUND);

            var questionIdsContainInExamId = unitOfWork.ExamDetailRepository.Get(ed => ed.ExamId == examId)
                                                       .Select(ed => ed.QuestionId);
            var entityResult = await this.repository.Get(q => questionIdsContainInExamId.Contains(q.Id)).ToListAsync();
            return entityResult.ToListViewModel<Question, QuestionViewModel>();
        }
    }
}
