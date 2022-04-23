using StockMarket.Shared.Contracts;
using StockMarket.Shared.Data.Context;
using StockMarket.Shared.Data.Models;
using System.Net.Http.Json;
using RunningDayStockLedger = StockMarket.Shared.Data.Models.RunningDayStockLedger;
using Stock = StockMarket.Shared.Data.Models.Stock;

namespace StockMarket.Client.Services.Implementations
{
    public class StockApi : IStocksController
    {
        private readonly HttpClient _httpClient;

        public StockApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CreateStock(Stock stock)
        {
            var result = await _httpClient.PostAsJsonAsync("api/stocks/CreateStock", stock);
         //   if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }
        
        public async Task<bool> BuyStock(BuyStock stock)
        {
            var result = await _httpClient.PostAsJsonAsync("api/stocks/BuyStock", stock);
         //   if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }
        public async Task<bool> SellStock(SellStock stock)
        {
            var result = await _httpClient.PostAsJsonAsync("api/stocks/SellStock", stock);
         //   if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> DeleteStock(string stockId)
        {
            var result = await _httpClient.GetAsync($"api/stocks/DeleteStock?stockId={stockId}");
           // if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }
        public async Task<Stock> GetStock(string stockId)
        {
            try
            {

                var result = await _httpClient.GetFromJsonAsync<Stock>($"api/stocks/GetStock?id={stockId}");
                // if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Stock>> GetAllStocks()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Stock>>("api/stocks/GetAllStocks");
            return result;
        }
        public async Task<List<Portfolio>> GetPortfolio()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Portfolio>>("api/Portfolio/GetPortfolio");
            return result;
        }
        public async Task<List<Stock>> GetAllStocksByIds(List<int> ids)
        {
            var result = await _httpClient.GetFromJsonAsync<List<Stock>>("api/stocks/GetAllStocksByIds");
            return result;
        }

        public async Task<List<RunningDayStockLedger>> GetAllRunningDayLedger()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<RunningDayStockLedger>>("api/stocks/GetRunningDayStocks");
                return result;
            }
            catch (Exception ex)
            {
                return null;
               
            }
           
        }
    }
}
