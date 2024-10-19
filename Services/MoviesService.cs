
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Services
{
	public class MoviesService(ApplicationDbContext _context) : IMoviesService
	{
		public async Task<Movie> Add(Movie movie)
		{
			await _context.AddAsync(movie);
			_context.SaveChanges();
			return movie;
		}

		public Movie Delete(Movie movie)
		{
			_context.Remove(movie);
			_context.SaveChanges();
			return movie;
		}

		public async Task<IEnumerable<Movie>> GetAll(byte GenreId = 0)
		{

		return await _context
				.Movies
				.Where(m=>m.GenreId== GenreId|| GenreId==0)
				.OrderByDescending(m => m.Rate)
				.Include(M => M.Genre).ToListAsync();
		}

		public async Task<Movie> GetById(int id)
		{
			return await _context.Movies.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == id);
		}

		public Movie Update(Movie movie)
		{
			_context.Update(movie);
			_context.SaveChanges();
			return movie;
		}
	}
}
