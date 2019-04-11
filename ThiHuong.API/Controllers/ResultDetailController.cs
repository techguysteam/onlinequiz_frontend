using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThiHuong.Framework.Helpers;
using ThiHuong.Framework.ViewModels;
using ThiHuong.Logic;

namespace ThiHuong.API.Controllers
{
    [ApiController]
    [Route("api/answer")]
    public class ResultDetailController : ThiHuongController
    {
        private IResultDetailService service;

        public ResultDetailController(IResultDetailService service, ExtensionSettings extensionSettings)
            : base(extensionSettings)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<dynamic> SubmitAnswerPartialAsync(List<SubmitAnswerViewModel> submitAnswerViewModel)
        {
            return await ExecuteInMonitoring(async () =>
            {
                await this.service.SubmitAnswerPartialAsync(submitAnswerViewModel, this.CurrentUserId);
                return null;
            });
        }

        [HttpPost("submit")]
        public async Task<dynamic> SubmitAnswerFinally(List<SubmitAnswerViewModel> submitAnswerViewModel = null)
        {
            return await ExecuteInMonitoring(async () =>
            {
                //Submit answer
                if (submitAnswerViewModel != null && submitAnswerViewModel.Count > 0)
                    await this.service.SubmitAnswerPartialAsync(submitAnswerViewModel, this.CurrentUserId);

                //Calculate point
                int examId = submitAnswerViewModel.First().ExamId;
                await this.service.CalculatePoint(this.CurrentUserId, examId);
                return null;
            });
        }
    }
}
