
using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Repository
{
    class TrailRepository : ITrailRepository

        
    {
        private readonly ApplicationDBContext _db;

        //constructor
        public TrailRepository(ApplicationDBContext db)
        {
            _db = db;
             
        }
        //here we need to give implementation of all these methods we have declared
        public bool CreateTrail(Trail nationalPark)
        {
            _db.Trails.Add(nationalPark);
            return Save();
        }

        public bool DeleteTrail(Trail nationalPark)
        {
            _db.Trails.Remove(nationalPark);
            return Save();
        }

        public Trail GetTrail(int TrailId)
        {
            //using Linq here to process collection
           return _db.Trails.Include(c => c.NationalPark).FirstOrDefault(c=>c.Id == TrailId);
        }

        public ICollection<Trail> GetTrails()
        {
           return _db.Trails.OrderBy(c=>c.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            bool value = _db.Trails.Any(a=>a.Name.ToLower().Trim() == name.ToLower().Trim());

            return value;
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(a=>a.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail nationalPark)
        {
          _db.Trails.Update(nationalPark);
            return Save();
        }

        ICollection<Trail> ITrailRepository.GetTrailsInNationalPark(int npId)
        {
            return _db.Trails.Include(c => c.NationalPark).Where(c=>c.NationalParkId == npId).ToList();
        }
    }
}
