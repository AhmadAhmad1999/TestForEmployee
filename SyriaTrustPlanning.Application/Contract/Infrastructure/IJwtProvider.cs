using SyriaTrustPlanning.Domain.Entities.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyriaTrustPlanning.Application.Contract.Infrastructure
{
    public interface IJwtProvider
    {
        public string Generate(User user);
        public int GetUserIdFromToken(string token);
    }
}
