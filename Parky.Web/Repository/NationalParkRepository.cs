using Parky.Web.Models;
using Parky.Web.Repository.IRepository;
using System.Net.Http;

namespace Parky.Web.Repository
{
#pragma warning disable IDE0052 
    public class NationalParkRepository: Repository<NationalPark>,INationalParkRepository
    {
        //pass dependency injection
        private readonly IHttpClientFactory _clientFactory;

        public NationalParkRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
#pragma warning disable IDE0052
}
