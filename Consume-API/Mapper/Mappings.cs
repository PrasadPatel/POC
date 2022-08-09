namespace Consume_API.Mapper
{
    /// <summary>
    /// Used for mapping DTO's to models
    /// </summary>
    public class Mappings : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mappings"/> class
        /// </summary>
        public Mappings()
        {
            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<ToDo, ToDoCreateDto>().ReverseMap();
            CreateMap<ToDo, ToDoUpdateDto>().ReverseMap();
        }
    }
}
