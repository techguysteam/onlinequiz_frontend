using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiHuong.Framework
{
    public class ThiHuongException : Exception
    {
        public ThiHuongException()
        {
        }

        public ThiHuongException(string message) : base(message)
        {
        }

        public ThiHuongException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
