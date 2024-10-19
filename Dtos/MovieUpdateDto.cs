namespace WebApplication2.Dtos
{
	public class MovieUpdateDto : MovieDto
	{
		public IFormFile? Poster { get; set; }
	}
}
