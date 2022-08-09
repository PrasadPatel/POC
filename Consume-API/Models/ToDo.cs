namespace Consume_API.Models
{
    /// <summary>
    ///  Model for ToDo
    /// </summary>
    public class ToDo
    {
        /// <summary>
        /// Gets/Sets userId property
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// Gets/Sets id property
        /// </summary>
        [Key]
        public int id { get; set; }
        /// <summary>
        /// Gets/Sets title property
        /// </summary>
        public string? title { get; set; }
        /// <summary>
        /// Gets/Sets completed property
        /// </summary>
        public bool completed { get; set; }
    }
}
