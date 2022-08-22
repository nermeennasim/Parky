using Parky.Web.Models;
using Parky.Web.Repository.IRepository;
using System.Net.Http;

namespace Parky.Web.Repository
{
#pragma warning disable IDE0052
    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }

#pragma warning disable IDE0052
}
