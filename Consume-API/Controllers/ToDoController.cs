
namespace Consume_API.Controllers
{
    /// <summary>
    /// ToDo controller
    /// </summary>
    [Route("api/v{version:apiVersion}/ToDos")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoRepository _toRepo;
        private readonly ILogger<ToDoController> _logger;
        private readonly IOptions<ApplicationParameters> _applicationParameters;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="applicationParameters"></param>
        /// <param name="toRepo"></param>
        /// <param name="mapper"></param>
        public ToDoController(ILogger<ToDoController> logger, IOptions<ApplicationParameters> applicationParameters, 
            IToDoRepository toRepo, IMapper mapper)
        {
            _toRepo = toRepo;
            _logger = logger;
            _applicationParameters = applicationParameters;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of ToDo
        /// </summary>
        /// <returns>List of ToDo</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllToDo()
        {
            IEnumerable<ToDo>? lstToDos = await _toRepo.GetAllAsync(_applicationParameters.Value.TodosApiEndpoint, "");
            return lstToDos == null ? NotFound() : Ok(lstToDos);
        }

        /// <summary>
        /// Get individual ToDo
        /// </summary>
        /// <param name="id">Id of ToDo</param>
        /// <returns>User</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            ToDo? todo = await _toRepo.GetAsync(_applicationParameters.Value.TodosApiEndpoint, id, "");
            return todo == null ? NotFound() : Ok(todo);
        }

        /// <summary>
        /// Get todo list for User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("/GetTodosForUser/{userId:int}")]
        public async Task<IActionResult> GetTodosForUser(int userId)
        {
            IEnumerable<ToDo>? lstToDos = await _toRepo.GetAllAsync(_applicationParameters.Value.UsersApiEndpoint + userId + "/todos", "");
            return lstToDos == null ? NotFound() : Ok(lstToDos);
        }

        /// <summary>
        /// Get todo for given todo completed status and user id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("/GetTodoForUserByStat/user/{userId:int}/{completed:bool}")]
        public async Task<IActionResult> GetTodoForUserByStatus(int userId, bool completed)
        {
            IEnumerable<ToDo>? lstToDos = await _toRepo.GetAllAsync(
                _applicationParameters.Value.TodosApiEndpoint.ToString().Remove(_applicationParameters.Value.TodosApiEndpoint.ToString().Length - 1)
                + "?userId=" + userId + "&completed=" + completed.ToString().ToLowerInvariant(), "");
            return lstToDos == null ? NotFound() : Ok(lstToDos);
        }

        /// <summary>
        /// Get todo for given todo id and user id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="todoId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("/GetTodoForUser/user/{userId:int}/todo/{todoId:int}")]
        public async Task<IActionResult> GetTodoForUserById(int userId, int todoId)
        {
            IEnumerable<ToDo>? lstToDos = await _toRepo.GetAllAsync(
                _applicationParameters.Value.TodosApiEndpoint.ToString().Remove(_applicationParameters.Value.TodosApiEndpoint.ToString().Length - 1)
                + "?userId="+userId+"&id="+ todoId, "");
            return lstToDos == null ? NotFound() : Ok(lstToDos);
        }

        /// <summary>
        /// Create ToDo
        /// </summary>
        /// <param name="obj">ToDo DTO for creation</param>
        /// <returns>Object result</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(ToDoCreateDto obj)
        {
            if (ModelState.IsValid)
            {
                var todoObj = _mapper.Map<ToDo>(obj);
                await _toRepo.CreateAsync(_applicationParameters.Value.TodosApiEndpoint, todoObj, "");
                return Created(_applicationParameters.Value.TodosApiEndpoint, new { success = true, message = "Created" });
            }
            else
            {
                return NotFound(new { success = false, message = "Not created" });
            }
        }

        /// <summary>
        /// Update ToDo
        /// </summary>
        /// <param name="obj">ToDo DTO for Updation</param>
        /// <returns>Object result</returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(ToDoUpdateDto obj)
        {
            if (ModelState.IsValid)
            {
                var todoObj = _mapper.Map<ToDo>(obj);
                await _toRepo.UpdateAsync(_applicationParameters.Value.TodosApiEndpoint + todoObj.id, todoObj, "");
                return Ok(new { success = true, message = "Updated" });
            }
            else
            {
                return NotFound(new { success = false, message = "Not updated" });
            }
        }

        /// <summary>
        /// Delete ToDo
        /// </summary>
        /// <param name="id">Id of ToDo</param>
        /// <returns>Object result</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _toRepo.DeleteAsync(_applicationParameters.Value.TodosApiEndpoint, id, "");
            return status ? Ok(new { success = true, message = "Deleted" }) : NotFound(new { success = false, message = "Not Deleted" });
        }
    }
}
