
namespace Consume_API.Repository
{
    /// <summary>
    /// User repository implementing IUserRepository and inheriting Repository of type User
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="clientFactory"></param>
        public UserRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}