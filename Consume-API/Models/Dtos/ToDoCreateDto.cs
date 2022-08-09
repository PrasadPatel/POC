namespace Consume_API.Models.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class ToDoCreateDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool completed { get; set; }
    }
}
