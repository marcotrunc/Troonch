using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Troonch.Application.Base.Utilities;

public class JsonLoaderUtility<T>
{
    private static T _instance;
    private static object _lock = new object();
    public static T LoadFromEmbeddedResource<TLogger>(string path, ILogger<TLogger> _logger)
    {
        lock (_lock)
        {
            if (_instance == null)
            {

                try
                {
                    using (var reader = new StreamReader(path))
                    {
                        var configuaration = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());

                        if (configuaration is null)
                        {
                            throw new JsonException("File not deserialized");
                        }

                        _instance = configuaration;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    _logger.LogError($"LoadFromEmbeddedResource::LoadFromEmbeddedResource -> The file was not found: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"LoadFromEmbeddedResource::LoadFromEmbeddedResource -> Error parsing JSON: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"LoadFromEmbeddedResource::LoadFromEmbeddedResource -> An error occurred: {ex.Message}");
                }
            }
        }
        return _instance;
    }

    public static T GetParameterValue(string jsonString, string parameterName) 
    {
        if (string.IsNullOrEmpty(jsonString))
        {
            throw new InvalidOperationException("Json String is not valid");
        }

        if (String.IsNullOrEmpty(parameterName))
        {
            throw new InvalidOperationException("Parameter name is not valid");
        }

        Dictionary<string, object> jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

        if (jsonObject == null)
        {
            throw new InvalidOperationException($"JSON Object is null");
        }

        if (jsonObject.ContainsKey(parameterName) == false)
        {
            throw new InvalidOperationException($"Parameter '{parameterName}' not found in the JSON string.");
        }

        if (jsonObject.TryGetValue(parameterName, out object retrievedValue))
        {
            return JsonConvert.DeserializeObject<T>(retrievedValue.ToString());
        }
        else
        {
            throw new Exception("Deserialization not completed successfully");
        }

    }
}
