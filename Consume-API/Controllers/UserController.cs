namespace Consume_API.Controllers
{
    /// <summary>
    /// UserController
    /// </summary>
    /// <scope>
    /// Configure session like HttpContext.Session.GetString("JWToken")) to hold token
    /// </scope>
    [Route("api/v{version:apiVersion}/Users")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<UserController> _logger;
        private readonly IOptions<ApplicationParameters> _applicationParameters;
        private readonly IMapper _mapper;

        /// <summary>
        /// UserController parameterized constructor
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="applicationParameters">IOptions</param>
        /// <param name="userRepo">IUserRepository</param>
        /// <param name="mapper">IMapper</param>
        public UserController(ILogger<UserController> logger, IOptions<ApplicationParameters> applicationParameters, IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _logger = logger;
            _applicationParameters = applicationParameters;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of Users
        /// </summary>
        /// <returns>List of Users</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsers()
        {
            IEnumerable<User>? lstUsers = await _userRepo.GetAllAsync(_applicationParameters.Value.UsersApiEndpoint, "");
            return lstUsers == null ? NotFound() : Ok(lstUsers);
        }

        /// <summary>
        /// Get individual User
        /// </summary>
        /// <param name="id">Id of User</param>
        /// <returns>User</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            User? user = await _userRepo.GetAsync(_applicationParameters.Value.UsersApiEndpoint, id, "");
            return user == null ? NotFound() : Ok(user);
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="obj">User DTO for creation</param>
        /// <returns>Object result</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(UserCreateDto obj)
        {
            if (ModelState.IsValid)
            {
                var userObj = _mapper.Map<User>(obj);
                await _userRepo.CreateAsync(_applicationParameters.Value.UsersApiEndpoint, userObj, "");
                return Created(_applicationParameters.Value.TodosApiEndpoint, new { success = true, message = "Created" });
            }
            else
            {
                return NotFound(new { success = false, message = "Not created" });
            }
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="obj">User DTO for Updation</param>
        /// <returns>Object result</returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(UserUpdateDto obj)
        {
            if (ModelState.IsValid)
            {
                var userObj = _mapper.Map<User>(obj);
                await _userRepo.UpdateAsync(_applicationParameters.Value.UsersApiEndpoint + userObj.id, userObj, "");
                return Ok(new { success = true, message = "Updated" });
            }
            else
            {
                return NotFound(new { success = false, message = "Not updated" });
            }
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="id">Id of User</param>
        /// <returns>Object result</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _userRepo.DeleteAsync(_applicationParameters.Value.UsersApiEndpoint, id, "");
            return status ? Ok(new { success = true, message = "Deleted" }) : NotFound(new { success = false, message = "Not Deleted" });
        }
    }
}

