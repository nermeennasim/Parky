using Newtonsoft.Json;
using Parky.Web.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Parky.Web.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;
        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IHttpClientFactory ClientFactory => _clientFactory;

        public async Task<bool> CreateAsync(string url, T objToCreate)
        {
            var request= new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), System.Text.Encoding.UTF8,"application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            //check reponse code
            //created 201
            if(response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
               
        }

        public async Task<bool> DeleteAsync(string url, int Id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url+Id);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            //check for no Content code
            if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            // This is the extra code to validate the SSL
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            client = new HttpClient(httpClientHandler) { BaseAddress = new Uri(url) };
            // end of the extra code

            try // I added a extra Try-Catch
            {
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Response " + ex.InnerException.Message);
                }

            }
            return null;
        }

        public async Task<bool> UpdateAsync(string url, T objToUpdate)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            if (objToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToUpdate), System.Text.Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            //check reponse code
            //created 201
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       public async  Task<T> GetAsync(string url, int Id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + Id);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //convert response back to INUMBERABLE of type <T>
                //called deserialize object
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);

            }

            return null;
        }

       
    }
}
