using System.Diagnostics;
using System.Reflection;
using IPTVAPI.Core.Constants;
using IPTVAPI.Core.Models;
using Newtonsoft.Json;

namespace IPTVAPI.Core.Services;

public class FetchService
{
    public async Task<List<Country>> GetCountriesLocalAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "IPTVAPI.Core.Resources.Raws.countries.json";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();
        var countryList = JsonConvert.DeserializeObject<List<Country>>(contents);

        if (countryList != null)
        {
            return countryList;
        }

        return null;
    }

    public async Task<List<OnlineChannel>> GetChannelsAsync()
    {
        try
        {
            using var client = new HttpClient();

            var response = await client.GetAsync(AppConstants.ChannelsAPI);
            {
                var content = await response.Content.ReadAsStringAsync();

                var deserialize = JsonConvert.DeserializeObject<List<OnlineChannel>>(content);

                return deserialize;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<List<OnlineStream>> GetStreamsAsync()
    {
        try
        {
            using var client = new HttpClient();

            client.Timeout = TimeSpan.FromSeconds(30);

            var response = await client.GetAsync(AppConstants.StreamsAPI);
            {
                var content = await response.Content.ReadAsStringAsync();

                var deserialize = JsonConvert.DeserializeObject<List<OnlineStream>>(content);

                return deserialize;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }
}
