using Redis.OM.Modeling;

namespace UsersAPI
{
    [Document(StorageType = StorageType.Json, Prefixes = new[] { "user" })]
    public class UserModel
    {
        [RedisIdField]
        [Indexed(Sortable =true)]
        public int Id { get; set; }
        [Indexed] public string? FirstName { get; set; }
        [Indexed] public string? LastName { get; set; }
        [Indexed] public string? Email { get; set; }
        [Indexed] public string? Gender { get; set; }
        [Indexed] public string? IpAddress { get; set; }
    }
}
