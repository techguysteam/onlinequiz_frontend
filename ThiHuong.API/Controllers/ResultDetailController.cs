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
        public dynamic SubmitAnswerPartial(List<SubmitAnswerViewModel> submitAnswerViewModel)
        {
            return ExecuteInMonitoring(() =>
            {
                this.service.SubmitAnswerPartial(submitAnswerViewModel, this.CurrentUserId);
                return null;
            });
        }

        [HttpPost("submit")]
        public dynamic SubmitAnswerFinally(List<SubmitAnswerViewModel> submitAnswerViewModel)
        {
            return ExecuteInMonitoring(() =>
            {
                this.service.SubmitAnswerPartial(submitAnswerViewModel, this.CurrentUserId);
                //calculate point
                
                return null;
            });
        }
    }
}
