using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
        public DbSet<Genre> Genres { get; set; }
		public DbSet<Movie> Movies { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
