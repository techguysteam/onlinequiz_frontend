using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThiHuong.Framework.Helpers;
using ThiHuong.Framework.Models;
using ThiHuong.Framework.ViewModels;
using ThiHuong.Framework.ViewModels.EntityViewModel;
using ThiHuong.Logic.ExamService;

namespace ThiHuong.API.Controllers
{
    [ApiController]
    
    public class ExamController : ThiHuongController
    {
        private IExamService service;

        public ExamController(IExamService service, ExtensionSettings extensionSettings) : base(extensionSettings)
        {
            this.service = service;
        }

        [HttpPost]
        [Authorize(Policy = "ADMIN")]
        public async Task<dynamic> CreateExam(ExamViewModel exam)
        {
            return await ExecuteInMonitoring(async () =>
            {
                await service.CreateExamAsync(exam);
                return exam;
            });
        }

        [HttpPost("code/{examId}")]
        [Authorize(Policy = "ADMIN")]
        public async Task<dynamic> GenerateCode(int examId)
        {
            return await ExecuteInMonitoring(async () =>
            {
                return new { code = await this.service.GenerateCodeAsync(examId) };
            });
        }

        [HttpPost("enroll")]
        [Authorize(Policy = "USER")]
        public async Task<dynamic> Enroll(ExamEnrollmentViewModel enrollment)
        {
            return await ExecuteInMonitoring(async () =>
            {
                return await this.service.Enroll(this.CurrentUserId, enrollment);
            });
        }

        [HttpPost("{examId}/{questionId}")]
        [Authorize(Policy = "ADMIN")]
        public async Task<dynamic> AddQuestionToExam(int examId, int questionId)
        {
            return await ExecuteInMonitoring(async () =>
            {
                await this.service.AddQuestionIntoExam(examId, questionId);
                return new { success = "success" };
            });
        }

        [HttpDelete("{examId}/{questionId}")]
        [Authorize(Policy = "ADMIN")]
        public async Task<dynamic> RemoveQuestionFromExam(int examId, int questionId)
        {
            return await ExecuteInMonitoring(async () =>
            {
                await this.service.RemoveQuestionFromExam(examId, questionId);
                return new { success = "success" };
            });
        }

    }
}