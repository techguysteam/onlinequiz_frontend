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

        //HttpPost

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
                return await this.service.CreateQuestionAsync(question, file, this.extensionSettings.AppSettings.SaveDirectory);
            });
        }

        [HttpPost("deactive/{questionId}")]
        [Authorize(Policy = "ADMIN")]
        public async Task<dynamic> DeactivateQuestion(int questionId)
        {
            return await ExecuteInMonitoring(async () =>
            {
                await this.service.Deactivate(questionId);
                return new { success = "success" };
            });
        }

        [HttpPost("active/{questionId}")]
        [Authorize(Policy = "ADMIN")]
        public async Task<dynamic> ActivateQuestion(int questionId)
        {
            return await ExecuteInMonitoring(async () =>
            {
                await this.service.Activate(questionId);
                return new { success = "success" };
            });
        }

        //HttpGet

        [HttpGet]
        [Authorize(Roles = "User")]
        [Authorize(Roles = "Admin")]
        public async Task<dynamic> GetQuestion(int size = 10, int page = 0)
        {
            return await ExecuteInMonitoring(async () =>
            {
                return await this.service.GetQuestionPagination(size, page);
            });
        }

        [HttpGet("active")]
        [Authorize(Policy = "ADMIN")]
        public async Task<dynamic> GetActiveQuestions(int size = 10, int page = 0)
        {
            return await ExecuteInMonitoring(async () =>
            {
                return await this.service.GetActiveQuestions(size, page);
            });
        }

        [HttpGet("{examId}")]
        [Authorize(Policy = "ADMIN")]
        public async Task<dynamic> GetQuestionByExamId(int examId, int size = 10, int page = 0)
        {
            return await ExecuteInMonitoring(async () =>
            {
                return await this.service.GetQuestionByExamId(examId, size, page);
            });
        }

        //HttpPut
        [HttpPut]
        [Authorize(Policy = "ADMIN")]
        public async Task<dynamic> UpdateQuestion([FromForm]QuestionViewModel question)
        {
            IFormFile file = null;
            if (HttpContext.Request.Form != null)
            {
                file = HttpContext.Request.Form.Files.FirstOrDefault();
            }
            return await ExecuteInMonitoring(async () =>
            {
                var result = await this.service.UpdateQuestion(question, file, this.extensionSettings.AppSettings.SaveDirectory);
                return result;
            });
        }

        //HttpDelete
        [HttpDelete("{questionId}")]
        [Authorize(Policy = "ADMIN")]
        public async Task<dynamic> DeleteQuestionPermanently(int questionId)
        {
            return await ExecuteInMonitoring(async () =>
            {
                await this.service.DeletePermanently(questionId);
                return new { success = "success" };
            });
        }
    }
}