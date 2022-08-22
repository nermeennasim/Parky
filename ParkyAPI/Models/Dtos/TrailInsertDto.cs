using System.ComponentModel.DataAnnotations;
using static ParkyAPI.Models.Trail;

namespace ParkyAPI.Models.Dtos
{
    public class TrailInsertDto
    {

        
      
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }

       

        public DifficultyType Difficulty { get; set; }
        [Required]
        public int NationalParkId { get; set; }
      
     
        
    }
}
