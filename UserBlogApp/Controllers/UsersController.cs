using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserBlogApp.Data;
using UserBlogApp.Dtos;
using UserBlogApp.Extenstions;
using UserBlogApp.Models;

namespace UserBlogApp.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public UsersController(
            ILogger<UsersController> logger,
            IUsersRepository usersRepository,
            IMapper mapper)
        {
            _logger = logger;
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetAllUsers))]
        public ActionResult<IEnumerable<UserReadDto>> GetAllUsers()
        {
            _logger.LogInformation("Request for {controller}: \"{action}\"",
                nameof(UsersController),
                nameof(GetAllUsers));

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(_usersRepository.GetAll()));
        }

        [HttpGet("{id}", Name = nameof(GetUserById))]
        public ActionResult<UserReadDto> GetUserById([FromRoute] Guid id)
        {
            _logger.LogInformation("Request for {controller}: \"{action}\" with user id {id}",
                nameof(UsersController),
                nameof(GetUserById),
                id);

            var user = _usersRepository.GetById(id);

            if (user is null)
            {
                _logger.LogWarning("User with id {id} not found", id);
                return NotFound();
            }

            return Ok(_mapper.Map<UserReadDto>(user));
        }

        [HttpPost(Name = nameof(CreateUser))]
        public async Task<ActionResult<UserReadDto>> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            _logger.LogInformation("Request for {controller}: \"{action}\" with model {model}",
                nameof(UsersController),
                nameof(GetUserById),
                userCreateDto.ToJson());

            var user = _mapper.Map<User>(userCreateDto);

            await _usersRepository.CreateAsync(user);
            if (!await _usersRepository.SaveAsync())
            {
                _logger.LogWarning("Internal server error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            _logger.LogInformation("New user added with id {id}", user.Id);

            var userReadDto = _mapper.Map<UserReadDto>(user);
            return CreatedAtRoute(nameof(GetUserById), new { user.Id }, userReadDto);
        }
    }
}
