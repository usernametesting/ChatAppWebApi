using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;
using Domain.Entities.IBaseEntity;
using Domain.Enums.MessageEnums;

namespace Domain.Entities.ConcretEntities;

public class Message : BaseEntity<int>,IBaseEntity<int>
{
    public string ?Content{ get; set; }

    public int UserMessagesId{ get; set; }
    public virtual UsersMessages UserMessages { get; set; }
    public bool isSender { get; set; }
    public MessageType MessageType { get; set; }
    public MessageState State { get; set; }


}
