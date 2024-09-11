using Application.DTOs.SignalRDTOs;
using Application.DTOs.UsersDTOs;
using Application.Repositories.Users;
using Domain.Entities.ConcretEntities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;
using Persistence.Repositories.Generics;

namespace Persistence.Repositories.Concrets.Users;

public class UserReadRepository : GenericReadRepository<AppUser, int>, IUserReadrepository<AppUser, int>
{
    private readonly DbSet<AppUser> _entity;
    private readonly ProductDbContext _context;
    public UserReadRepository(ProductDbContext context) : base(context)
    {
        _context = context;
        _entity = _context.Set<AppUser>();

    }

    public async Task<List<UserWithMessages>> GetUsersWithMessages(int senderId)
    {
        var userWithMessages = await _entity
       .Where(user=>user.Id!=senderId)
      .Select(u => new
      {
          User = new UserWithMessages
          {
              Id = u.Id,
              UserName = u.UserName,
              IsOnline  = u.IsOnline,
              LastActivityDate = u.LastActivityDate,
              Messages = _context.UsersMessages
                  .Where(um => (um.FromUserId == u.Id && um.ToUserId==senderId)||( um.ToUserId == u.Id && um.FromUserId==senderId))
                  .Select(um => new MessageDTO
                  {
                      Message = um.Message.Content,
                      MessageType = um.Message.MessageType,
                      CreatedDate = um.CreatedDate,
                      IsSender = (senderId==um.FromUserId)
                  })
                  .OrderBy(m => m.CreatedDate)  
                  .ToList()
          },
          LastMessageDate = _context.UsersMessages
              .Where(um => um.FromUserId == u.Id || um.ToUserId == u.Id)
              .Max(um => (DateTime?)um.CreatedDate)  
      })
      .OrderByDescending(u => u.LastMessageDate)  
      .Select(u => u.User)  
      .ToListAsync();

        return userWithMessages;

    }
}
