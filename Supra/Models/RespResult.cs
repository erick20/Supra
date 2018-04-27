using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supra.Models
{
    public class RespResult
    {
        public int opCode { get; set; }

        public string opDesc { get; set; }

        public dynamic RESULT { get; set; }
    }

}
