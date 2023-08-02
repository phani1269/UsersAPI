using OfficeOpenXml;
using Redis.OM.Searching;
using Redis.OM;
using System;

namespace UsersAPI.Services
{
    
    public class UserService : IUserService
    {
        private readonly RedisCollection<UserModel> _user;
        private readonly RedisConnectionProvider _provider;

        public UserService(RedisConnectionProvider provider)
        {
            _provider = provider;
            _user = (RedisCollection<UserModel>)provider.RedisCollection<UserModel>();
        }

        public async Task<bool> BulkUserUpload(IFormFile usersFile)
        {
            if (usersFile?.Length > 0)
            {
                var stream = usersFile.OpenReadStream();
                List<UserModel> usersData = new List<UserModel>();
                try
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (var row = 2; row <= rowCount; row++)
                        {
                            try
                            {
                                var id = worksheet.Cells[row, 1].Value?.ToString();
                                var first_name = worksheet.Cells[row, 2].Value?.ToString();
                                var lastName = worksheet.Cells[row, 3].Value?.ToString();
                                var email = worksheet.Cells[row, 4].Value?.ToString();
                                var gender = worksheet.Cells[row, 5].Value?.ToString();
                                var ip_address = worksheet.Cells[row, 6].Value?.ToString();


                                UserModel userData = new UserModel()
                                {
                                    Id = Convert.ToInt32(id),
                                    FirstName = first_name,
                                    LastName = lastName,
                                    Email = email,
                                    Gender = gender,
                                    IpAddress = ip_address,
                                };
                                usersData.Add(userData);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                    foreach (var user in usersData)
                    {
                        // Add data into redis
                        await _user.InsertAsync(user);
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    
        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            var users = await _user.OrderBy(x => x.Id).ToListAsync();
            return users;
        }
    }
}
