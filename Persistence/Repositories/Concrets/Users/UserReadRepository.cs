using Application.DTOs.SignalRDTOs;
using Application.DTOs.UsersDTOs;
using Application.Repositories.Users;
using Domain.Entities.ConcretEntities;
using Domain.Enums.MessageEnums;
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

    public async Task<UserDTO> GetUserByIdAsync(int senderId, int userId)
    {
        var userWithMessages = await _entity
       .Where(user => user.Id == userId)
      .Select(u => new
      {
          User = new UserDTO
          {
              Id = u.Id,
              UserName = u.UserName,
              IsOnline = u.IsOnline,
              LastActivityDate = u.LastActivityDate ?? "never signed",
              ProfImageUrl = u.ProfImageUrl,
              Messages = _context.UsersMessages
                  .Where(um => (um.FromUserId == u.Id && um.ToUserId == senderId) || (um.ToUserId == u.Id && um.FromUserId == senderId))
                  .Select(um => new MessageDTO
                  {
                      Message = um.Message.Content,
                      MessageType = um.Message.MessageType,
                      CreatedDate = um.CreatedDate,
                      IsSender = (senderId == um.FromUserId),
                      State = um.Message.State

                  })
                  .OrderBy(m => m.CreatedDate)
                  .ToList(),
              UnreadMessageCount = _context.UsersMessages
                .Where(um => um.FromUserId == u.Id && um.ToUserId == senderId && um.Message.State != MessageState.SEEN)
                .Count()
          },
      })
      .Select(u => u.User)
      .FirstOrDefaultAsync();

        return userWithMessages;
    }

    public async Task<List<UserDTO>> GetUsersWithMessages(int senderId)
    {
        var userWithMessages = await _entity
       .Where(user => user.Id != senderId)
      .Select(u => new
      {
          User = new UserDTO
          {
              Id = u.Id,
              IsMutualFriendship = (u.Contacts!.Any(c => c.ContactUserId == senderId)),
              UserName = u.UserName,
              IsOnline = u.IsOnline,
              LastActivityDate = u.LastActivityDate ?? "never signed",
              ProfImageUrl = u.ProfImageUrl,
              Statuses = u.Contacts!.Any(c => c.ContactUserId == senderId) ? u.Statuses!.ToList() : new List<Status>(),
              Biografy = u.Biografy,
              Messages = _context.UsersMessages
                  .Where(um => (um.FromUserId == u.Id && um.ToUserId == senderId) || (um.ToUserId == u.Id && um.FromUserId == senderId))
                  .Select(um => new MessageDTO
                  {
                      Message = um.Message.Content,
                      MessageType = um.Message.MessageType,
                      CreatedDate = um.CreatedDate,
                      IsSender = (senderId == um.FromUserId),
                      State = um.Message.State

                  })
                  .OrderBy(m => m.CreatedDate)
                  .ToList(),
              UnreadMessageCount = _context.UsersMessages
                .Where(um => um.FromUserId == u.Id && um.ToUserId == senderId && um.Message.State != MessageState.SEEN)
                .Count()
          },
          LastMessageDate = _context.UsersMessages
              .Where(um => um.FromUserId == u.Id || um.ToUserId == u.Id)
              .Max(um => (DateTime?)um.CreatedDate)
      })
      .OrderByDescending(u => u.LastMessageDate)
      .Select(u => u.User)
      .Where(u => u.Messages!.Count != 0)
      .ToListAsync();

        return userWithMessages;

    }
}
