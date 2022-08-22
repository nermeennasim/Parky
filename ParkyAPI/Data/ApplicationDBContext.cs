using Microsoft.EntityFrameworkCore;
using ParkyAPI.Models;

namespace ParkyAPI.Data
{
    public class ApplicationDBContext: DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {
                
        }

       public DbSet<NationalPark>  NationalParks { get; set; }
        public DbSet<Trail> Trails { get; set; }

    }
}
