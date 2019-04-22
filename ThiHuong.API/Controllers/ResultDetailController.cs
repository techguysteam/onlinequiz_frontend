using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThiHuong.Framework.Helpers;
using ThiHuong.Framework.ViewModels;
using ThiHuong.Logic;

namespace ThiHuong.API.Controllers
{
    [ApiController]
    [Route("api/answer")]
    [Authorize(Policy = "USER")]
    public class ResultDetailController : ThiHuongController
    {
        private IResultDetailService service;

        public ResultDetailController(IResultDetailService service, ExtensionSettings extensionSettings)
            : base(extensionSettings)
        {
            this.service = service;
        }

        [HttpPost("{examId}")]
        public async Task<dynamic> SubmitAnswerPartialAsync([FromBody] List<SubmitAnswerViewModel> submitAnswerViewModel, int examId)
        {
            return await ExecuteInMonitoring(async () =>
            {
                await this.service.SubmitAnswerPartialAsync(submitAnswerViewModel, this.CurrentUserId, examId);
                return null;
            });
        }

        [HttpPost("submit/{examId}")]
        public async Task<dynamic> SubmitAnswerFinally(int examId, [FromBody] List<SubmitAnswerViewModel> submitAnswerViewModel = null)
        {
            return await ExecuteInMonitoring(async () =>
            {
                //Submit answer
                if (submitAnswerViewModel != null && submitAnswerViewModel.Count > 0)
                    await this.service.SubmitAnswerPartialAsync(submitAnswerViewModel, this.CurrentUserId, examId);

                //Calculate point
                await this.service.CalculatePoint(this.CurrentUserId, examId);
                return null;
            });
        }

        
    }
}
