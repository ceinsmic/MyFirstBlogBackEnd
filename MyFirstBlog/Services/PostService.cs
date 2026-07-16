namespace MyFirstBlog.Services;

using MyFirstBlog.Helpers;
using MyFirstBlog.Entities;
using System.Text.RegularExpressions;
using MyFirstBlog.Dtos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;

public interface IPostService
{
    IEnumerable<PostDto> GetPosts();
    PostDto GetPost(String slug);
    Task<PostDto> createpost(PostDto inpost);
}

public class PostService : IPostService
{

    
    private DataContext _context;

    public PostService(DataContext context)
    {
        _context = context;
    }

    

    public IEnumerable<PostDto> GetPosts()
    {
        return _context.Posts.Select(post => post.AsDto());
    }

    public PostDto GetPost(string slug)
    {
        return getPost(slug).AsDto();
    }

    private Post getPost(string slug)
    {
        return _context.Posts.Where(a=>a.Slug==slug.ToString()).SingleOrDefault();
    }

    public async Task<PostDto> createpost(PostDto inpost)
    {
        var post = new Post
        {
            Id = Guid.NewGuid(),
            Title = inpost.Title,
            Slug = inpost.Slug,
            Body = inpost.Body,
            CreatedDate = DateTime.UtcNow
        };
        await _context.AddAsync(post);
        await _context.SaveChangesAsync();
        Console.WriteLine("TEST");
        return inpost;
    }
}
