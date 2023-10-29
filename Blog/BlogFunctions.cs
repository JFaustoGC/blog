using System;
using System.IO;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Data;

public partial class BlogFunctions
{
    private readonly Context _context;

    public BlogFunctions(Context context) => _context = context;

    [FunctionName("CreatePost")]
    public async Task<IActionResult> CreatePost(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "createpost")] HttpRequest req, ILogger log)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var post = JsonConvert.DeserializeObject<Post>(requestBody);

        if(post == null)
             return new BadRequestObjectResult("Please pass a correct data in the request body");

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        return new OkObjectResult(post);
    }

    [FunctionName("GetAllPosts")]
    public async Task<IActionResult> GetAllPosts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getallposts")] HttpRequest req, ILogger log)
    {
        var posts = await _context.Posts.ToListAsync();

        return new OkObjectResult(posts);
    }
    
    [FunctionName("GetPost")]
    public async Task<IActionResult> GetPost(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getpost/{id}")] HttpRequest req, ILogger log, Guid id)
    {
        var post = await _context.Posts.FindAsync(id);

        if(post == null)
            return new NotFoundObjectResult("Post not found");

        return new OkObjectResult(post);
    }
    
    [FunctionName("UpdatePost")]
    public async Task<IActionResult> UpdatePost(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "updatepost/{id}")] HttpRequest req, ILogger log, Guid id)
    {
        var post = await _context.Posts.FindAsync(id);
        
        if(post == null)
            return new NotFoundObjectResult("Post not found");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var updatedPost = JsonConvert.DeserializeObject<Post>(requestBody);
        
        if(updatedPost == null)
             return new BadRequestObjectResult("Please pass a correct data in the request body");

        post.Username = updatedPost.Username;
        post.Type = updatedPost.Type;

        _context.Posts.Update(post);
        await _context.SaveChangesAsync();

        return new OkObjectResult(post);
    }
    
    [FunctionName("DeletePost")]
    public async Task<IActionResult> DeletePost(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "deletepost/{id}")] HttpRequest req, ILogger log, Guid id)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
        {
            return new NotFoundObjectResult("Post not found");
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return new OkResult();
    }
}