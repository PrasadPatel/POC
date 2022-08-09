namespace Consume_API.Models.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class ToDoUpdateDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
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
