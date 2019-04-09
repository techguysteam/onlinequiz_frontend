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

namespace ThiHuong.Logic.QuestionService
{
    public interface IQuestionService : IBaseService<Question>
    {
        Task<QuestionViewModel> CreateQuestion(Question question, IFormFile file, string pathInServer);
        Task<List<QuestionViewModel>> GetQuestionByExamId(int examId);
    }

    public class QuestionService : BaseService<Question>, IQuestionService
    {
        private IHttpContextAccessor httpContextAccessor;
        private IHostingEnvironment env;

        public QuestionService(UnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env) 
            : base(unitOfWork.QuestionRepository, unitOfWork)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.env = env;
        }

        public async Task<QuestionViewModel> CreateQuestion(Question question, IFormFile file, string pathInServerWithoutHost)
        {
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
            this.repository.AddAsync(question);
            this.unitOfWork.SaveChangesAsync();
            return question.ToViewModel<QuestionViewModel>();
        }

        public async Task<List<QuestionViewModel>> GetQuestionByExamId(int examId)
        {
            var questionIdsContainInExamId = unitOfWork.ExamDetailRepository.Get(ed => ed.ExamId == examId)
                                                       .Select(ed => ed.QuestionId);
            var entityResult = await this.repository.Get(q => questionIdsContainInExamId.Contains(q.Id)).ToListAsync();
            return entityResult.ToListViewModel<Question, QuestionViewModel>();
        }
    }
}
