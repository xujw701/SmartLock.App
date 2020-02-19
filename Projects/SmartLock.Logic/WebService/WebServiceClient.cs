using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartLock.Model.Services;

namespace SmartLock.Logic.Services.WebUtilities
{
    public class WebServiceClient
    {
        private readonly IUserSession _userSession;

        public WebServiceClient(IUserSession userSession = null)
        {
            _userSession = userSession;
        }

        public async Task<TResponse> PostAsync<TResponse>(Uri uri, object data)
        {
            using (var client = CreateWebClient())
            {
                // Serialize
                var jsonString = JsonConvert.SerializeObject(data);
                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // POST data and deserialize response
                using (var response = await client.PostAsync(uri, jsonContent))
                {
                    EnsureSuccessStatusCode(response);

                    var responseString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<TResponse>(responseString);
                }
            }
        }

        public async Task PostAsync(Uri uri, object data)
        {
            using (var client = CreateWebClient())
            {
                // Serialize
                var jsonString = JsonConvert.SerializeObject(data);
                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // POST data
                using (var response = await client.PostAsync(uri, jsonContent))
                {
                    EnsureSuccessStatusCode(response);

                    await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task<TResponse> PutAsync<TResponse>(Uri uri, object data)
        {
            using (var client = CreateWebClient())
            {
                // Serialize
                var jsonString = JsonConvert.SerializeObject(data);
                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // POST data and deserialize response
                using (var response = await client.PutAsync(uri, jsonContent))
                {
                    EnsureSuccessStatusCode(response);

                    var responseString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<TResponse>(responseString);
                }
            }
        }

        public async Task PutAsync(Uri uri, object data)
        {
            using (var client = CreateWebClient())
            {
                // Serialize
                var jsonString = JsonConvert.SerializeObject(data);
                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // POST data and deserialize response
                using (var response = await client.PutAsync(uri, jsonContent))
                {
                    EnsureSuccessStatusCode(response);

                    await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task<TResponse> PostRawAsync<TResponse>(Uri uri, byte[] data)
        {
            using (var client = CreateWebClient())
            {
                // POST data
                using (var response = await client.PostAsync(uri, new ByteArrayContent(data)))
                {
                    EnsureSuccessStatusCode(response);

                    var responseString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<TResponse>(responseString);
                }
            }
        }

        public async Task PostRawAsync(Uri uri, byte[] data)
        {
            using (var client = CreateWebClient())
            {
                // POST data
                using (var response = await client.PostAsync(uri, new ByteArrayContent(data)))
                {
                    EnsureSuccessStatusCode(response);

                    var responseString = await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task<TResponse> GetAsync<TResponse>(Uri uri)
        {
            using (var client = CreateWebClient())
            {
                // POST data and deserialize response
                using (var response = await client.GetAsync(uri))
                {
                    EnsureSuccessStatusCode(response);

                    var responseString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<TResponse>(responseString);
                }
            }
        }

        public async Task DeleteAsync(Uri uri)
        {
            using (var client = CreateWebClient())
            {
                // DELETE
                using (var response = await client.DeleteAsync(uri))
                {
                    EnsureSuccessStatusCode(response);
                }
            }
        }

        public async Task<TResponse> DeleteAsync<TResponse>(Uri uri)
        {
            using (var client = CreateWebClient())
            {
                // DELETE
                using (var response = await client.DeleteAsync(uri))
                {
                    EnsureSuccessStatusCode(response);

                    var responseString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<TResponse>(responseString);
                }
            }
        }

        private void EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                //throw new WebServiceClientException(ex.Message, ex, response);
            }
        }

        private HttpClient CreateWebClient()
        {
            var client = new HttpClient();

            if (_userSession != null)
            {
                client.DefaultRequestHeaders.Add("UserId", _userSession.UserId.ToString());
                client.DefaultRequestHeaders.Add("Bearer", _userSession.Token);
            }

            return client;
        }
    }
}
