using ParkyAPI.Models;
using System.Collections.Generic;

namespace ParkyAPI.Repository.IRepository
{
    public interface INationalParkRepository
    {

        ICollection<NationalPark> GetAllNationalParks();
        NationalPark GetNationalPark(int NationalParkId);
        bool NationalParkExists(string name);
        bool NationalParkExists(int id);
        bool CreateNationalPark(NationalPark nationalPark);
        bool UpdateNationalPark(NationalPark nationalPark);
        bool DeleteNationalPark(NationalPark nationalPark);
        bool Save();


    }
}
