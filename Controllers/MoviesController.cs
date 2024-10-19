using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
	
	[Route("api/[controller]")]
	[ApiController]
	public class MoviesController : ControllerBase
	{
		private IMoviesService _moviesService;
		private IGenreService _genresService;

		public MoviesController(IMoviesService moviesService, IGenreService genresService)
		{
			_moviesService = moviesService;
			_genresService = genresService;
		}

		private new List<String> _allowedExtensions = new List<String> { ".jpg", ".png" };
		private long _maxAllowedPosterSize = 1048576;

		
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAllAsync()
		{
			var movies = await _moviesService.GetAll();
			//map movies to DTO
			return Ok(movies);
		}

		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> GetByIdAsync(int id)
		{
			var Movie = await _moviesService.GetById(id);
			if (Movie == null)
			{
				return NotFound("Enter A correct Id");
			}
			return Ok(Movie);
		}

		[HttpGet("GetByGenreId")]
		[Authorize]
		public async Task<IActionResult> GetByGenreIdAsync(byte genreid)
		{
			var movies = await _moviesService.GetAll(genreid);

			return Ok(movies);

		}


		
		[HttpPost]
		[Authorize(Roles = "MODERATOR, ADMIN")]
		public async Task<IActionResult> CreateAsync([FromForm] MovieCreateDto dto)
		{
			if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
			{
				return BadRequest("only .png and .jpg images are allowed");
			}
			if (dto.Poster.Length > _maxAllowedPosterSize)
			{
				return BadRequest("Max Allowed Size For Poster is 1 MB!");
			}

			var IsValidGenre = await _genresService.IsValidGenre(dto.GenreId);
			if (!IsValidGenre)
			{
				return BadRequest("Enter A correct GenreId");
			}
			using var dataStream = new MemoryStream();
			await dto.Poster.CopyToAsync(dataStream);
			var Movie = new Movie
			{
				Title = dto.Title,
				Year = dto.Year,
				GenreId = dto.GenreId,
				Rate = dto.Rate,
				StoryLine = dto.StoryLine,
				Poster = dataStream.ToArray()
			};
			await _moviesService.Add(Movie);
			
			return Ok(Movie);
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "MODERATOR, ADMIN")]
		public async Task<IActionResult> UpdateAsync(int id,[FromForm]MovieUpdateDto dto)
		{
			var movie = await _moviesService.GetById(id);
			if (movie == null)
				return NotFound($"No Movie Was Found With ID {id}");

			var IsValidGenre = await _genresService.IsValidGenre(dto.GenreId);
			if (!IsValidGenre)
			{
				return BadRequest("Enter A correct GenreId");
			}

			if(dto.Poster!=null)
			{
				if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
				{
					return BadRequest("only .png and .jpg images are allowed");
				}
				if (dto.Poster.Length > _maxAllowedPosterSize)
				{
					return BadRequest("Max Allowed Size For Poster is 1 MB!");
				}
				using var dataStream = new MemoryStream();
				await dto.Poster.CopyToAsync(dataStream);
				movie.Poster=dataStream.ToArray();
			}



			movie.Title = dto.Title;
			movie.Year = dto.Year;
			movie.GenreId = dto.GenreId;
			movie.Rate = dto.Rate;
			movie.StoryLine = dto.StoryLine;

			_moviesService.Update(movie);
			return Ok(movie);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "MODERATOR, ADMIN")]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			var movie = await _moviesService.GetById(id);
			if (movie == null)
				return NotFound($"No Movie Was Found With ID {id}");

			_moviesService.Delete(movie);
			return Ok(movie);
		}

	}
}
