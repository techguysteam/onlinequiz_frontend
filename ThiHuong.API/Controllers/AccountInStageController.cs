using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThiHuong.Framework.Helpers;
using ThiHuong.Framework.ViewModels;
using ThiHuong.Logic.AccountInStageService;

namespace ThiHuong.API.Controllers
{
    [Route("api/account-in-stage")]
    public class AccountInStageController : ThiHuongController
    {
        private IAccountInStageService service;

        public AccountInStageController(IAccountInStageService service, ExtensionSettings extensionSettings) : base(extensionSettings)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("top")]
        public dynamic GetTheHallOfFame(int stageId, int size = 10, int page = 0)
        {
            return ExecuteInMonitoring(() =>
            {
                return service.GetHallOfFamebyStageId(stageId, size, page);
            });

        }


        [HttpGet("result-detail/{examId}")]
        [Authorize(Policy = "USER")]
        public dynamic GetResultDetailForReview(int examId)
        {
            return ExecuteInMonitoring(() =>
              {
                  return this.service.GetResultDetailForReviews(examId, this.CurrentUserId);
              });
        }

    }
}