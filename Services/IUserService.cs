namespace UsersAPI.Services
{
    public interface IUserService
    {
        Task<bool> BulkUserUpload(IFormFile usersFile);
        Task<IEnumerable<UserModel>> GetAllUsers();
    }
}
