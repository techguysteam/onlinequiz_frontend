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
    //[Authorize(Roles ="ADMIN")]
    public class ExamsController : ThiHuongController
    {
        private IExamService service;

        public ExamsController(IExamService service, ExtensionSettings extensionSettings) : base(extensionSettings)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<dynamic> CreateExam(ExamViewModel exam)
        {
            return await ExecuteInMonitoring(async () =>
            {
                await service.CreateExamAsync(exam);
                return exam;
            });
        }

        [HttpPost("code")]
        public async Task<dynamic> GenerateCode(int examId)
        {
            return await ExecuteInMonitoring(async () =>
            {
                return new { code = await this.service.GenerateCodeAsync(examId) };
            });
        }

        [HttpPost("enroll")]
        public async Task<dynamic> Enroll(ExamEnrollmentViewModel enrollment)
        {
            return await ExecuteInMonitoring(async () =>
            {
                return await this.service.Enroll(this.CurrentUserId, enrollment);
            });
        }
        
    }
}