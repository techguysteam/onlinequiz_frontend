using Microsoft.AspNetCore.Mvc;
using ThiHuong.Framework.Helpers;
using ThiHuong.Framework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework;

namespace ThiHuong.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThiHuongController : ControllerBase
    {
        public ExtensionSettings extensionSettings { get; }

        public int CurrentUserId
        {
            get
            {
                string value = HttpContext.User?.FindFirst(ClaimTypes.UserId)?.Value;
                int result = 0;
                int.TryParse(value, out result);
                return result;
            }
        }

        public string CurrentUsername
        {
            get
            {
                return HttpContext.User?.FindFirst(ClaimTypes.Username)?.Value;
            }
        }


        public ThiHuongController(ExtensionSettings extensionSettings)
        {
            this.extensionSettings = extensionSettings;
        }

        protected dynamic ExecuteInMonitoring(Func<dynamic> function)
        {
            dynamic result;
            try
            {
                result = function();
            }
            catch (ThiHuongException ex)
            {
                return BaseResponse.GetErrorResponse(ex.Message);
            }catch(Exception ex)
            {
                return BaseResponse.GetErrorResponse(ex.Message);
            }
            return BaseResponse.GetSuccessResponse(result);
        }

        protected async Task<dynamic> ExecuteInMonitoring(Func<Task<dynamic>> function)
        {
            dynamic result;
            try
            {
                result = await function();
            }
            catch (ThiHuongException ex)
            {
                return BaseResponse.GetErrorResponse(ex.Message);
            }
            catch (Exception ex)
            {
                return BaseResponse.GetErrorResponse(ex.Message);
            }
            return BaseResponse.GetSuccessResponse(result);
        }
    }
}
