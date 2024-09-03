using Domain.Entities.BaseEntities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Identity;
public class AppUserToken : IdentityUserToken<int>, IBaseEntity<int>
{
    public int Id { get; set; }

    public DateTime? Expires { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public override int UserId { get => base.UserId; set => base.UserId = value; }
    public virtual AppUser User { get; set; }
}
