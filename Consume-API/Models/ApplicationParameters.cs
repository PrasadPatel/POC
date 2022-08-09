namespace Consume_API.Models
{
    /// <summary>
    /// Can be used for application related configuration
    /// </summary>
    public class ApplicationParameters
    {
        /// <summary>
        /// Gets/Sets Users Api Endpoint property
        /// </summary>
        public string UsersApiEndpoint { get; set; }
        /// <summary>
        /// Gets/Sets ToDo Api Endpoint property
        /// </summary>
        public string TodosApiEndpoint { get; set; }
    }
}