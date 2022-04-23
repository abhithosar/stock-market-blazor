using StockMarket.Shared.Contracts;
using StockMarket.Shared.Data.Context;
using StockMarket.Shared.Data.Models;
using System.Net.Http.Json;

namespace StockMarket.Client.Services.Implementations
{
    public class MarketHourApi : IMarketHoursContract
    {
        private readonly HttpClient _httpClient;

        public MarketHourApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<MarketHour>> GetAllMarketHours()
        {
            var result = await _httpClient.GetFromJsonAsync<List<MarketHour>>("api/MarketConfig/GetMarketHours");
            return result ?? new List<MarketHour>();
        }
        public async Task<bool> IsMarketOpen()
        {
            var result = await _httpClient.GetFromJsonAsync<bool>("api/MarketConfig/IsMarketOpen");
            return result;
        }

        public async Task<bool> UpdateMarketHours(List<MarketHour> marketHours)
        {
            var result = await _httpClient.PostAsJsonAsync("api/MarketConfig/UpdateMarketHour", marketHours);
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
