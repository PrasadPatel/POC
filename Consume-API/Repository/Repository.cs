
namespace Consume_API.Repository
{
    /// <summary>
    /// Repository implementing IRepository of type T
    /// </summary>
    /// <typeparam name="T">Type T/Entity</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;
        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="clientFactory"></param>
        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        /// <summary>
        /// Get list of type T / entity
        /// </summary>
        /// <param name="url">Api endpoint</param>
        /// <param name="token">Token</param>
        /// <returns>List of type T / entity</returns>
        public async Task<IEnumerable<T>?> GetAllAsync(string url, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue
                    ("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            return null;
        }
        /// <summary>
        /// Get individual type T / entity
        /// </summary>
        /// <param name="url">Api endpoint</param>
        /// <param name="Id">Id of type T / entity</param>
        /// <param name="token">Token</param>
        /// <returns>Type T / entity</returns>
        public async Task<T?> GetAsync(string url, int Id, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + Id);
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue
                    ("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            return null;
        }
        /// <summary>
        /// Create type T / entity
        /// </summary>
        /// <param name="url">Api endpoint</param>
        /// <param name="objToCreate">Type T / entity</param>
        /// <param name="token">Token</param>
        /// <returns>True if created; False otherwise</returns>
        public async Task<bool> CreateAsync(string url, T objToCreate, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }

            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue
                    ("Bearer", token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            return response.StatusCode == HttpStatusCode.Created;
        }
        /// <summary>
        /// Update type T / entity
        /// </summary>
        /// <param name="url">Api endpoint</param>
        /// <param name="objToUpdate">Type T / entity</param>
        /// <param name="token">Token</param>
        /// <returns>True if updated; False otherwise</returns>
        public async Task<bool> UpdateAsync(string url, T objToUpdate, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            if (objToUpdate != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(objToUpdate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }

            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue
                    ("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
        }
        /// <summary>
        /// Delete type T / entity
        /// </summary>
        /// <param name="url">Api endpoint</param>
        /// <param name="Id">Id of type T / entity</param>
        /// <param name="token">Token</param>
        /// <returns>True if deleted; False otherwise</returns>
        public async Task<bool> DeleteAsync(string url, int Id, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url + Id);
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue
                    ("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}

