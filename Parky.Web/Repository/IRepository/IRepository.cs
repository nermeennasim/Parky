using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parky.Web.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
       public Task<T> GetAsync(string url, int Id); //id for individual object of T.
       public Task<IEnumerable<T>> GetAllAsync(string url); //get all objects return INUmberable of type T
        Task<bool> CreateAsync(string url, T objToCreate);// url and object to create
      public  Task<bool> UpdateAsync(string url, T objToUpdate);// url and object to b updated
       public Task<bool> DeleteAsync(string url, int Id); //delete url and ID
    }
}
