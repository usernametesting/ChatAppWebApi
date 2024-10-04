using Domain.Entities.ConcretEntities;

namespace Application.DTOs.UsersDTOs;

public class CurrentlyUser
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public bool IsOnline { get; set; }
    public string? LastActivityDate { get; set; }
    public string? ProfImageUrl{ get; set; }
    public string? Biografy { get; set; }

    public List<Contact> ?Contacts { get; set; }
}
