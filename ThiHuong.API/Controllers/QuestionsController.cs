using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class QuestionController : ThiHuongController
    {
        private IQuestionService service;

        public QuestionController(IQuestionService service, ExtensionSettings extensionSettings) : base(extensionSettings)
        {
            this.service = service;
        }

        [HttpPost]
        [Authorize(Policy = "ADMIN")]
        public async Task<dynamic> CreateQuestionAsync([FromForm]QuestionViewModel question)
        {
            IFormFile file = null;
            if (HttpContext.Request.Form != null)
            {
                file = HttpContext.Request.Form.Files.FirstOrDefault();
            }
            return await ExecuteInMonitoring(async () =>
            {
                return await this.service.CreateQuestionAsync(question, file, this.extensionSettings.appSettings.SaveDirectory);
            });
        }

        [HttpGet]
        [Authorize(Policy = "ADMIN")]
        public async Task<dynamic> GetQuestion()
        {
            return await ExecuteInMonitoring(async () =>
            {
                return await this.service.Get<QuestionViewModel>();
            });
        }

        [HttpGet("{examId}")]
        [Authorize(Policy = "ADMIN")]
        public async Task<dynamic> GetQuestionByExamId(int examId)
        {
            return await ExecuteInMonitoring(async () =>
            {
                return await this.service.GetQuestionByExamId(examId);
            });
        }





    }
}