using Application.DTOs.SignalRDTOs;
using Application.Repositories.Products;
using Domain.Entities.ConcretEntities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generics;

namespace Persistence.Repositories.Concrets.Messages;

public class MessageReadRepository : GenericReadRepository<AppUser, int>, IMessageReadRepository

{

    public MessageReadRepository(DbContext context) : base(context)
    {
    }

    public async Task<List<MessageDTO>?> GetUsersWithMessages(int Id)
    {
        ////var messages  =await _entity.Where(um=>um.FromUserId==Id || um.ToUserId==Id).Select(m=>m.Message).ToListAsync();

        ////var messages = await _entity
        ////                    .Where(um => um.FromUserId == Id || um.ToUserId == Id)
        ////                    .Select(m => new MessageDTO
        ////                    {
        ////                        Message = m.Message.content,
        ////                        IsSender = (m.FromUserId == Id),
        ////                        CreatedDate = m.Message.CreatedDate
        ////                    })
        ////                    .OrderBy(m => m.CreatedDate) 
        ////                    .ToListAsync();

        //var users = await _entity.


        //return messages;
        return default;
    }
}
