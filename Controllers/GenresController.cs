using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Dtos;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GenresController(IGenreService _genresSerive, ApplicationDbContext _context) : ControllerBase
	{



		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAllAsync()
		{

			var Genres = await _genresSerive.GetAll();
			//var Genres = await _context.Genres.OrderBy(g=>g.Name).ToListAsync();
			return Ok(Genres);
		}
		[HttpPost]
		[Authorize(Roles = "MODERATOR")]
		public async Task<IActionResult> CreateAsync(GenreDto dto)
		{
			var genre = new Genre { Name = dto.Name };
			await _genresSerive.Add(genre);
			return Ok(genre);

		}

		[HttpPut("{id}")]
		[Authorize(Roles = "MODERATOR, ADMIN")]
		public async Task<IActionResult> UpdateAsync(byte id, [FromBody] GenreDto dto)
		{
			var genre =await _genresSerive.GetById(id);
			if (genre == null)
				return NotFound($"No Genre Was found with the id {id}");

			genre.Name = dto.Name;
			_genresSerive.Update(genre);
			return Ok(genre);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "MODERATOR, ADMIN")]
		public async Task<IActionResult> DeleteAsync(byte id)
		{
			var genre = await _genresSerive.GetById(id);
			if (genre == null)
				return NotFound($"No Genre Was found with the id {id}");

			_genresSerive.Delete(genre);
			
			return Ok(genre);
		}


	}
}
