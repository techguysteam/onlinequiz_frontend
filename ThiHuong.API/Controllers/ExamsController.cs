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
        public dynamic CreateExam(ExamViewModel exam)
        {
            return ExecuteInMonitoring(() =>
            {
                service.CreateExam(exam);
                return exam;
            });
        }

        [HttpPost("code")]
        public dynamic GenerateCode(int examId)
        {
            return ExecuteInMonitoring(() =>
            {
                return new { code = this.service.GenerateCode(examId) };
            });
        }

        [HttpPost("enroll")]
        public dynamic Enroll(ExamEnrollmentViewModel enrollment)
        {
            return ExecuteInMonitoring(() =>
            {
                return this.service.Enroll(this.CurrentUserId, enrollment);
            });
        }
        
    }
}