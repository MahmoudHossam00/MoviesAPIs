namespace WebApplication2.Dtos
{
	public class MovieCreateDto : MovieDto
	{
		public IFormFile Poster { get; set; }
	}
}
