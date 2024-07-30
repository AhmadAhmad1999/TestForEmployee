using SyriaTrustPlanning.Domain.Common;
using SyriaTrustPlanning.Domain.Constants;

namespace SyriaTrustPlanning.Domain.Entities.IdentityModels
{
    public class User : AuditableEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; } = null!;
    }
}
