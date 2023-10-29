using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class Context : DbContext
{
    public virtual DbSet<Post> Posts { get; set; } = default!;
    
    public Context(DbContextOptions options) : base(options)
    {
        
    }
}