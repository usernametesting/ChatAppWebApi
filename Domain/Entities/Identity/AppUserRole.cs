using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
namespace Domain.Entities.Identity;

public class AppUserRole : IdentityUserRole<int>
{
    public override int UserId { get => base.UserId; set => base.UserId = value; }
    public virtual AppUser User { get; set; }
    public override int RoleId { get => base.RoleId; set => base.RoleId = value; }
    public virtual AppRole Role { get; set; }
}