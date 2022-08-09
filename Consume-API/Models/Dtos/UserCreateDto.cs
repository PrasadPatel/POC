namespace Consume_API.Models.Dtos
{
    /// <summary>
    /// DTO for user create
    /// </summary>
    public class UserCreateDto
    {
        /// <summary>
        /// Gets/Sets name property
        /// </summary>
        public string? name { get; set; }
        /// <summary>
        /// Gets/Sets username property
        /// </summary>
        public string? username { get; set; }
        /// <summary>
        /// Gets/Sets email property
        /// </summary>
        public string? email { get; set; }
        /// <summary>
        /// Gets/Sets address property
        /// </summary>
        public Address? address { get; set; }
        /// <summary>
        /// Gets/Sets phone property
        /// </summary>
        public string? phone { get; set; }
        /// <summary>
        /// Gets/Sets website property
        /// </summary>
        public string? website { get; set; }
        /// <summary>
        /// Gets/Sets company property
        /// </summary>
        public Company? company { get; set; }
    }
}
