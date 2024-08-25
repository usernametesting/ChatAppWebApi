
namespace Domain.Entities.IBaseEntity;

public class BaseEntity<TKey>
{
    public TKey Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public BaseEntity()
    {
        CreatedDate = DateTime.Now;
    }
}
