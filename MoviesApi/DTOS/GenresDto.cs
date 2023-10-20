using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOS
{
    public class GenresDto
    {
        [MaxLength(100)]
        public string Name { get; set; }=string.Empty;
    }
}
