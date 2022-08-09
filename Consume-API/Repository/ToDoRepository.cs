namespace Consume_API.Repository
{
    /// <summary>
    /// ToDo Repository inheriting generic repository and implementing IToDoRepository
    /// </summary>
    public class ToDoRepository:Repository<ToDo>, IToDoRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoRepository"/> class.
        /// </summary>
        /// <param name="clientFactory"></param>
        public ToDoRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
