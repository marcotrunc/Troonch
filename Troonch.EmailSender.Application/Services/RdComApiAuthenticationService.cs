using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using Troonch.Application.Base.Utilities;
using Troonch.EmailSender.Rdcom.Domain.Configuration;
using Troonch.EmailSender.Rdcom.Domain.DTOs;
using System.Reflection;

namespace Troonch.EmailSender.Rdcom.Services;

public class RdComApiAuthenticationService
{
    private readonly RdcomConfiguration _rdcomConfiguration;
    private readonly RdcomEndPointConfiguration _endpointConfiguration;
    private readonly ILogger<RdComApiAuthenticationService> _logger;
    private readonly string resourceName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\RdcomEndpoints.json";

    public RdComApiAuthenticationService(
                        ILogger<RdComApiAuthenticationService> logger,
                        IOptions<RdcomConfiguration> rdcomConfiguration
    )
    {
        _logger = logger;
        _endpointConfiguration = JsonLoaderUtility<RdcomEndPointConfiguration>.LoadFromEmbeddedResource(resourceName, _logger);
        _rdcomConfiguration = rdcomConfiguration.Value;
    }
    #region public methods

    public async Task<string> GetAccessTokenAsync()
    {
        _logger.LogInformation("RdcomApiAuthenticator::GetAccessToken - Init get access token process.");

        var rdcomAuthenticationResult = await GetRdcomAuthenticationResult();

        if (rdcomAuthenticationResult == null)
        {
            _logger.LogError("RdcomApiAuthenticator::GetAccessToken - authentication failed");
            return null;
        }

        return rdcomAuthenticationResult.AccessToken;
    }

    #endregion

    #region private methods
    private async Task<RdcomAuthenticationResponse> GetRdcomAuthenticationResult()
    {
        var rdcomAuthenticationResult = await AcquireTokenSilent();

        if (rdcomAuthenticationResult == null)
        {
            rdcomAuthenticationResult = await AcquireTokenInteractive();
            await DeleteTokensExpired();
        }

        return rdcomAuthenticationResult;
    }

    private async Task DeleteTokensExpired()
    {
        _logger.LogInformation("RdcomApiAuthenticator::DeleteTokensExpired - Retrive Token From Rdcom List");

        string apiUrl = $"{_endpointConfiguration.BaseUri}{_endpointConfiguration.GetToken}";
        var tokensExpired = new List<RdcomAuthenticationResponse>();

        try
        {
            var httpClient = new HttpClientUtility(_rdcomConfiguration.Username, _rdcomConfiguration.Password);
            var response = await httpClient.DoRequest(apiUrl, null, HttpClientUtility.WebMethods.Get);

            if (response == null)
            {
                _logger.LogError("RdcomApiAuthenticator::DeleteTokensExpired - response is null");
                throw new ArgumentNullException(nameof(response));
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("RdcomApiAuthenticator::DeleteTokensExpired - Token not retrived correctly");
            }

            string jsonContent = await response.Content.ReadAsStringAsync();

            var tokens = JsonLoaderUtility<RdcomAuthenticationResponse[]>.GetParameterValue(jsonContent, "results");

            tokensExpired = tokens.AsQueryable()
                                        .Where(token =>
                                                token.ExpireDate != null &&
                                                token.ExpireDate.Value.ToUniversalTime() <= DateTime.UtcNow.ToLocalTime())
                                        .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError($"RdcomApiAuthenticator::DeleteTokensExpired Get Token with username and password failure, {ex.Message}");
        }


        if (tokensExpired == null || !tokensExpired.Any())
        {
            _logger.LogInformation($"RdcomApiAuthenticator::DeleteTokensExpired No token expired to delete");
            return;
        }

        foreach (var token in tokensExpired)
        {
            try
            {
                var isTokenExpiredDeleted = await ExeceuteDeleteAsync(token);

                if (!isTokenExpiredDeleted)
                {
                    throw new Exception(nameof(token));
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"RdcomApiAuthenticator::DeleteTokensExpired Token expired not deleted ex: {e.Message}");
            }
        }
    }

    private async Task<bool> ExeceuteDeleteAsync(RdcomAuthenticationResponse tokenToDelete)
    {
        var apiUrl = $"{_endpointConfiguration.BaseUri}{_endpointConfiguration.GetToken}";

        if (tokenToDelete == null)
        {
            return false;
        }

        var httpClient = new HttpClientUtility(_rdcomConfiguration.Username, _rdcomConfiguration.Password);
        var payload = new Dictionary<string, string>();

        payload.Add("token", tokenToDelete.AccessToken);


        var jsonBody = JsonConvert.SerializeObject(payload);


        var response = await httpClient.DoRequest(apiUrl, jsonBody, HttpClientUtility.WebMethods.Delete);

        if (response == null)
        {
            throw new ArgumentNullException(nameof(response));
        }

        var jsonContent = await response.Content.ReadAsStringAsync();

        var responseTokenDelete = JsonConvert.DeserializeObject<RdcomAuthenticationResponse>(jsonContent);

        if (responseTokenDelete == null)
        {
            throw new ArgumentNullException(nameof(responseTokenDelete));
        }


        _logger.LogInformation($"RdcomApiAuthenticator::DeleteTokensExpired - Token {tokenToDelete.AccessToken} Expired Deleted Correctly");
        return true;
    }
    private async Task<RdcomAuthenticationResponse> AcquireTokenSilent()
    {
        RdcomAuthenticationResponse rdcomAuthenticationResult = null;

        _logger.LogInformation("RdcomApiAuthenticator::AcquireTokenSilent - Retrive Token From Rdcom List");

        string apiUrl = $"{_endpointConfiguration.BaseUri}{_endpointConfiguration.GetToken}";

        try
        {
            var httpClient = new HttpClientUtility(_rdcomConfiguration.Username, _rdcomConfiguration.Password);
            var response = await httpClient.DoRequest(apiUrl, null, HttpClientUtility.WebMethods.Get);

            if (response == null)
            {
                _logger.LogError("RdcomApiAuthenticator::AcquireTokenSilent - response is null");
                throw new ArgumentNullException(nameof(response));
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("RdcomApiAuthenticator::AcquireTokenSilent -Token not retrived correctly");
            }

            string jsonContent = await response.Content.ReadAsStringAsync();

            var tokens = JsonLoaderUtility<RdcomAuthenticationResponse[]>.GetParameterValue(jsonContent, "results");

            rdcomAuthenticationResult = tokens.AsQueryable().FirstOrDefault(token => token.ExpireDate == null);

            if (rdcomAuthenticationResult != null)
            {
                return rdcomAuthenticationResult;
            }

            rdcomAuthenticationResult = tokens.AsQueryable().FirstOrDefault(token => token.ExpireDate.Value.ToUniversalTime() > DateTime.UtcNow.ToUniversalTime());
        }
        catch (Exception ex)
        {
            _logger.LogError($"RdcomApiAuthenticator::AcquireTokenInteractive Get Token with username and password failure, {ex.Message}");
        }


        return rdcomAuthenticationResult;
    }

    private async Task<RdcomAuthenticationResponse> AcquireTokenInteractive()
    {
        RdcomAuthenticationResponse rdcomAuthenticationResult = null;

        _logger.LogInformation("RdcomApiAuthenticator::AcquireTokenInteractive - Token expired, init retrive token process");

        string apiUrl = $"{_endpointConfiguration.BaseUri}{_endpointConfiguration.GetToken}";

        try
        {
            var httpClient = new HttpClientUtility(_rdcomConfiguration.Username, _rdcomConfiguration.Password);
            var response = await httpClient.DoRequest(apiUrl, null, HttpClientUtility.WebMethods.Post);

            if (response == null)
            {
                _logger.LogError("RdcomApiAuthenticator::AcquireTokenInteractive - response is null");
                throw new ArgumentNullException(nameof(response));
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Token not created");
            }

            string jsonContent = await response.Content.ReadAsStringAsync();

            rdcomAuthenticationResult = JsonConvert.DeserializeObject<RdcomAuthenticationResponse>(jsonContent);

        }
        catch (Exception ex)
        {
            _logger.LogError($"RdcomApiAuthenticator::AcquireTokenInteractive Get Token with username and password failure, {ex.Message}");
        }

        return rdcomAuthenticationResult;
    }
    #endregion
}


