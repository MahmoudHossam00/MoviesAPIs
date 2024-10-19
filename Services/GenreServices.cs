using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Services
{
	public class GenresServices(ApplicationDbContext _context) : IGenreService
	{
		
		public async Task<Genre> Add(Genre Genre)
		{
			
			await _context.AddAsync(Genre);
			_context.SaveChanges();
			return Genre;
		}

		public Genre Delete(Genre genre)
		{
			_context.Remove(genre);
			_context.SaveChanges();
			return genre;
		}

		public async Task<IEnumerable<Genre>> GetAll()
		{

			return await  _context.Genres.OrderBy(g => g.Name).ToListAsync();
		}

		public async Task<Genre> GetById(byte id)
		{
			return await _context.Genres.SingleOrDefaultAsync(x => x.Id == id);
		}

		public Genre Update(Genre genre)
		{
			_context.Update(genre);
			_context.SaveChanges();
			return genre;

		}

		public  Task<bool> IsValidGenre(byte id)
		{
			return  _context.Genres.AnyAsync(g => g.Id == id);
		}
	}
}
