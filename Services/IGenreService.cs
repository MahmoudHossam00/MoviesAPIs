using WebApplication2.Models;

namespace WebApplication2.Services
{
	public interface IGenreService
	{
		Task<IEnumerable<Genre>> GetAll();
		Task<Genre> GetById(byte id);
		Task<Genre> Add(Genre Genre);
		Genre Update(Genre genre);
		Genre Delete(Genre genre);
		Task<bool> IsValidGenre(byte id);
	}
}
