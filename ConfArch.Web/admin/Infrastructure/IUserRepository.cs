namespace Kentico.Admin
{
    public interface IUserRepository
    {
        IUser GetByExternalId(string externalId);


        IUser GetByUsernameAndPassword(string username, string password);


        IUser GetByUserName(string userName);
    }
}