
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Repository
{
    class NationalParkRepository : INationalParkRepository

        
    {
        private readonly ApplicationDBContext _db;

        //constructor
        public NationalParkRepository(ApplicationDBContext db)
        {
            _db = db;
             
        }
        //here we need to give implementation of all these methods we have declared
        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int NationalParkId)
        {
            //using Linq here to process collection
           return _db.NationalParks.FirstOrDefault(a=>a.Id == NationalParkId);
        }

        public ICollection<NationalPark> GetAllNationalParks()
        {
           return _db.NationalParks.OrderBy(a=>a.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            bool value = _db.NationalParks.Any(a=>a.Name.ToLower().Trim() == name.ToLower().Trim());

            return value;
        }

        public bool NationalParkExists(int id)
        {
            return _db.NationalParks.Any(a=>a.Id == id);
        }

        public bool Save()
        {
            return  _db.SaveChanges() >=0 ? true : false;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
          _db.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
