namespace Application.Helpers.Users;

public interface IUserHelper
{
    int GetCurrentlyUserId();
    int GetUserIdByToken();

}
