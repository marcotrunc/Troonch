using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Troonch.Application.Base.Utilities;

public class HttpClientUtility
{
    private readonly HttpClient _httpClient;
    private readonly string _bearerToken;
    private readonly string _base64Credentials;
    public HttpClientUtility(string bearerToken)
    {
        _httpClient = new HttpClient();
        _bearerToken = bearerToken;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    }
    public HttpClientUtility(string username, string password)
    {
        _httpClient = new HttpClient();
        _base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    }

    public async Task<HttpResponseMessage> DoRequest(string apiUrl, string jsonBody, WebMethods httpMethod)
    {
        if (!String.IsNullOrEmpty(_bearerToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
        }

        if (!String.IsNullOrEmpty(_base64Credentials))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _base64Credentials);
        }

        StringContent content = null;

        if (!String.IsNullOrEmpty(jsonBody))
        {
            content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        }

        HttpResponseMessage response = null;

        try
        {
            switch (httpMethod)
            {
                case WebMethods.Get:
                    response = await _httpClient.GetAsync(apiUrl);
                    break;

                case WebMethods.Post:
                    response = await _httpClient.PostAsync(apiUrl, content);
                    break;
                case WebMethods.Put:
                    response = await _httpClient.PutAsync(apiUrl, content);
                    break;
                case WebMethods.Delete:
                    response = await _httpClient.DeleteAsync(apiUrl);
                    break;
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"POST request failed. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"HttpClientUtility::DoRequest Exception : {ex.Message}");
        }

        _httpClient.Dispose();

        return response;
    }
    public enum WebMethods
    {
        Get,
        Post,
        Put,
        Delete
    }
}
