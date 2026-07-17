namespace MyFirstBlog.Controllers;

using Microsoft.AspNetCore.Mvc;
using MyFirstBlog.Dtos;
using MyFirstBlog.Services;
using System.ComponentModel.DataAnnotations;

public class CreatePostRequest
{
    [Required]
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
}

[ApiController]
[Route("posts")]

public class PostsController : ControllerBase {
    private IPostService _postService;

    public PostsController(IPostService postService) {
        _postService = postService;
    }

    // Get /posts
    [HttpGet]
    public IEnumerable<PostDto> GetPosts() {
        return _postService.GetPosts();
    }

    // Get /posts/:slug
    [HttpGet("{slug}")]
    public ActionResult<PostDto> GetPost(string slug) {
        var post = _postService.GetPost(slug);

        if (post is null) {
            return NotFound();
        }

        return post;
    }
    [HttpPost]
    public ActionResult CreatePost(CreatePostRequest enter)
    {
        if(!ModelState.IsValid)
        {
            if(enter.Title == null)
            {
                ModelState.AddModelError("Title", "Title cannot be blank");
            }
            return BadRequest(ModelState);
        }

        var newPost = new PostDto
        {
            Id = Guid.NewGuid(),
            Title = enter.Title,
            Body = enter.Description,
            Slug = enter.Title.ToLower().Replace(" ", "-"),
        };

        _postService.createpost(newPost);

        return Ok(enter);
        
    }
}
