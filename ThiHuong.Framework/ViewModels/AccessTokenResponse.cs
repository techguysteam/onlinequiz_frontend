using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiHuong.Framework.ViewModels
{
    public class AccessTokenResponse
    {
        public string Username { get; set; }

        public string Role { get; set; }

        public string AccessToken { get; set; }

    }
}
