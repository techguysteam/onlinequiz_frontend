using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiHuong.Framework.Helpers
{
    public class AppSettings
    {
        public string SecretKey { get; set; }

        public int TokenExpireTime { get; set; }

        public string SaveDirectory { get; set; }
    }
}
