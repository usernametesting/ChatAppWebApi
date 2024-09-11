using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;
using Domain.Entities.ConcretEntities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace ETicaretAPI.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<int>, IBaseEntity<int>, ISoftDelete
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? ConnectionId { get; set; }

        public bool IsOnline { get; set; }
        public string? LastActivityDate { get; set; }

        public virtual ICollection<AppUserRole>? UserRoles { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<UsersMessages>? Messages { get; set; }
        public virtual ICollection<AppUserToken>? UserTokens { get; set; }

        public AppUser()
        {
            UserRoles = new HashSet<AppUserRole>();
            //SecurityStamp = Guid.NewGuid().ToString();
        }
    }
}
