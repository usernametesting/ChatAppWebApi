using Application.DTOs.SignalRDTOs;
using Application.Repositories.Commons;
using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;
using Domain.Entities.ConcretEntities;
using ETicaretAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Products;

public interface IMessageReadRepository :IGenericReadRepository<AppUser,int>
{
    Task<List<MessageDTO>?> GetUsersWithMessages(int Id);

}
