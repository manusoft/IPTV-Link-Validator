using System.Diagnostics;
using IPTVAPI.Core.Constants;
using IPTVAPI.Core.Models;
using Newtonsoft.Json;

namespace IPTVAPI.Core.Services;

public class FetchService
{
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
