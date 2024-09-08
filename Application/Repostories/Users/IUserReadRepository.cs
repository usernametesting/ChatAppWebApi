using Application.DTOs.SignalRDTOs;
using Application.DTOs.UsersDTOs;
using Application.Repositories.Commons;
using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;

namespace Application.Repositories.Users;

public interface IUserReadrepository<TEntity, TKey> 
    : IGenericReadRepository<TEntity,TKey> where TEntity : IBaseEntity<TKey>
{
    Task<List<UserWithMessages>> GetUsersWithMessages(int senderId);

}
