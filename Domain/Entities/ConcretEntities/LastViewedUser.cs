using Domain.Entities.BaseEntities;
using Domain.Entities.IBaseEntity;
using ETicaretAPI.Domain.Entities.Identity;

namespace Domain.Entities.ConcretEntities;

public class LastViewedUser:BaseEntity<int>,IBaseEntity<int>
{

    public DateTime date { get; set; }
    public int UserId { get; set; }
    public virtual AppUser User{ get; set; }
    public int ViewedUserId  { get; set; }
}
