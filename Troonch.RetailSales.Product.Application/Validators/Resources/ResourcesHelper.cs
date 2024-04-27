using System.Globalization;
using System.Resources;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Troonch.RetailSales.Product.Application.Validators.Resources
{
    public class ResourcesHelper 
    {
        private readonly ILogger<ResourcesHelper> _logger;
        private readonly ResourceManager _resourceManager;
        private readonly string _userLanguage;
        public ResourcesHelper(ILogger<ResourcesHelper> logger)
        {
            _logger = logger;
            _resourceManager = new ResourceManager("Troonch.RetailSales.Product.Application.Validators.Resources.Validator", Assembly.GetExecutingAssembly());
            _userLanguage = Thread.CurrentThread.CurrentUICulture.Name;
        }
        public string GetString(string key, List<ResourceHelperParameter>? parameters = null)
        {
            string? emptyErrorMessage = _resourceManager.GetString(key, new CultureInfo(_userLanguage));

            if(emptyErrorMessage == null) 
            {
                emptyErrorMessage = "Generic Error";
            }

            string result = emptyErrorMessage;

            if (parameters == null || !parameters.Any())
            {
                return result;
            }


            for(int i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];  
                string? parameterValue = parameter.ParameterKey;

                if (parameter.IsInResource && parameter.ParameterKey is not null)
                {
                    parameterValue = _resourceManager.GetString(parameter.ParameterKey, new CultureInfo(_userLanguage));
                }

                if(String.IsNullOrEmpty(parameterValue))
                {
                    _logger.LogInformation("ResourcesHelper::GetString The param for error concatenation Message is null");
                    return emptyErrorMessage;
                }

                result = result.Replace("{"+i.ToString()+"}",parameterValue.ToString());
            }

            return result;
        }    
    }

    public class ResourceHelperParameter
    {
        public string? ParameterKey { get; set; }
        public bool IsInResource { get; set; } = true;
    }
}
