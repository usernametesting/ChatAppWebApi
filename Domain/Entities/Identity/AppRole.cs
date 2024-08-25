using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Domain.Entities.Identity
{
    public class AppRole  : IdentityRole<int>, IBaseEntity<int>, ISoftDelete 
    {
        public virtual ICollection<AppUserRole>? UserRoles { get; set; } 
        public bool IsDeleted { get ; set ; }
        public DateTime CreatedDate { get ; set ; }
        public DateTime? UpdatedDate { get ; set ; }

    }
}
