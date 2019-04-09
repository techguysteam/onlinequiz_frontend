using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThiHuong.Framework.Constants;
using ThiHuong.Framework.Helpers;
using ThiHuong.Framework.Models;
using ThiHuong.Framework.ViewModels;
using ThiHuong.Framework.ViewModels.EntityViewModel;
using ThiHuong.Logic.QuestionService;

namespace ThiHuong.API.Controllers
{
    [ApiController]
    public class QuestionsController : ThiHuongController
    {
        private IQuestionService service;

        public QuestionsController(IQuestionService service, ExtensionSettings extensionSettings) : base(extensionSettings)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<dynamic> CreateQuestion([FromForm]Question question)
        {
            IFormFile file = null;
            if (HttpContext.Request.Form != null)
            {
                file = HttpContext.Request.Form.Files.FirstOrDefault();
            }
            return ExecuteInMonitoring(() =>
            {
                return this.service.CreateQuestion(question, file, this.extensionSettings.appSettings.SaveDirectory).Result;
            });
        }

        [HttpGet]
        public dynamic GetQuestion()
        {
            return ExecuteInMonitoring(() =>
            {
                return this.service.Get<QuestionViewModel>();
            });
        }

        [HttpGet("{examId}")]
        public dynamic GetQuestionByExamId(int examId)
        {
            return ExecuteInMonitoring(() =>
            {
                return this.service.GetQuestionByExamId(examId);
            });
        }





    }
}