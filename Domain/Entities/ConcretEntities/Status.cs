using Domain.Entities.BaseEntities;
using Domain.Entities.IBaseEntity;
using Domain.Enums.MessageEnums;
using ETicaretAPI.Domain.Entities.Identity;

namespace Domain.Entities.ConcretEntities;

public class Status : BaseEntity<int>, IBaseEntity<int>
{
    public StatusType StatusType { get; set; }

    public string? MediaUrl { get; set; }

    public string? Decription { get; set; }

    public int UserId { get; set; }
    public AppUser User { get; set; }
}
