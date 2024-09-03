using Domain.Entities.Commons;
using Domain.Entities.IBaseEntity;
using ETicaretAPI.Domain.Entities.Identity;


namespace Domain.Entities.ConcretEntities;

public class Message : BaseEntity<int>, ISoftDelete
{
    public bool IsDeleted { get; set; }

    public int FromUserId { get; set; }
    public virtual AppUser FromUser { get; set; }
    public int ToUserId { get; set; }
    public virtual AppUser ToUser { get; set; }

}
