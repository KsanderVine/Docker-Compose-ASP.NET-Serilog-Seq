using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserBlogApp.Data;
using UserBlogApp.Dtos;
using UserBlogApp.Extenstions;
using UserBlogApp.Models;

namespace UserBlogApp.Controllers
{
    [Route("api/users/{authorId:guid}/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IUsersRepository _usersRepository;
        private readonly IPostsRepository _postsRepository;
        private readonly IMapper _mapper;

        public PostsController(
            ILogger<PostsController> logger,
            IUsersRepository usersRepository,
            IPostsRepository postsRepository,
            IMapper mapper)
        {
            _logger = logger;
            _usersRepository = usersRepository;
            _postsRepository = postsRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetAllPosts))]
        public ActionResult<IEnumerable<PostReadDto>> GetAllPosts([FromRoute] Guid authorId)
        {
            _logger.LogInformation("Request for {controller}: \"{action}\" with author id {authorId}",
                nameof(PostsController),
                nameof(GetAllPosts),
                authorId);

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(_postsRepository.GetPostsByAuthorId(authorId)));
        }

        [HttpGet("{id}", Name = nameof(GetPostById))]
        public ActionResult<PostReadDto> GetPostById([FromRoute] Guid authorId, [FromRoute] Guid id)
        {
            _logger.LogInformation("Request for {controller}: \"{action}\" with author id {authorId} and post id {id}",
                nameof(PostsController),
                nameof(GetPostById),
                authorId,
                id);

            var post = _postsRepository.GetById(id);

            if (post is null)
            {
                _logger.LogWarning("Post with id {id} not found", id);
                return NotFound();
            }

            return Ok(_mapper.Map<PostReadDto>(post));
        }

        [HttpPost(Name = nameof(CreatePost))]
        public async Task<ActionResult<PostReadDto>> CreatePost([FromRoute] Guid authorId, [FromBody] PostCreateDto postCreateDto)
        {
            _logger.LogInformation("Request for {controller}: \"{action}\" with author id {authorId} and model {model}",
                nameof(PostsController),
                nameof(CreatePost),
                authorId,
                postCreateDto.ToJson());

            var user = _usersRepository.GetById(authorId);

            if (user is null)
            {
                _logger.LogWarning("User with id {authorId} not found", authorId);
                return NotFound();
            }

            var post = _mapper.Map<Post>(postCreateDto);
            post.AuthorId = authorId;

            await _postsRepository.CreateAsync(post);
            if(!await _postsRepository.SaveAsync())
            {
                _logger.LogWarning("Internal server error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            _logger.LogInformation("New post added for author id {authorId} with post id {id}", authorId, post.Id);

            var postReadDto = _mapper.Map<PostReadDto>(post);
            return CreatedAtRoute(nameof(GetPostById), new { authorId, post.Id }, postReadDto);
        }
    }
}
