using Domain.Entities.BaseEntities;
using Domain.Entities.IBaseEntity;
using ETicaretAPI.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.ConcretEntities;

public class Contact : BaseEntity<int>, IBaseEntity<int>
{
    public int UserId { get; set; }
    public virtual AppUser User { get; set; }
    public int ContactUserId { get; set; }
    public virtual AppUser ContactUser { get; set; }
}
