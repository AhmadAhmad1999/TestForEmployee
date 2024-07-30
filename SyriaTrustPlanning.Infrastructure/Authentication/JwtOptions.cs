using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyriaTrustPlanning.Infrastructure.Authentication
{
    public class JwtOptions
    {
        public string Issueer { get; init; }
        public string Audience { get; init; }
        public string SecurityKey { get; init; }
    }
}
