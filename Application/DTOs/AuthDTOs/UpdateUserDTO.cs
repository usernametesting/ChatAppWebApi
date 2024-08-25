using ETicaretAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AuthDTOs;

public class UpdateUserDTO
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }

    public virtual List<string>? RoleNames { get; set; } = new List<string>();
    public virtual ICollection<AppRole>? AllRoles { get; set; } = new List<AppRole>();
}
