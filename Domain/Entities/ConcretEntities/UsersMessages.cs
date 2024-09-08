using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;
using Domain.Entities.IBaseEntity;
using ETicaretAPI.Domain.Entities.Identity;


namespace Domain.Entities.ConcretEntities;

public class UsersMessages : BaseEntity<int>, IBaseEntity<int>, ISoftDelete
{
    public bool IsDeleted { get; set; }

    public int FromUserId { get; set; }
    public virtual AppUser FromUser { get; set; }

    public int ToUserId { get; set; }
    public int MessageId { get; set; }
    public virtual Message Message { get; set; }

}
