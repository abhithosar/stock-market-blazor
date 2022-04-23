using StockMarket.Shared.Data.Context;
using StockMarket.Shared.Data.Models;
using System.Net.Http.Json;

namespace StockMarket.Client.Services.Implementations
{
    public class MoneyApi
    {
        private readonly HttpClient _httpClient;

        public MoneyApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddMoney(AddMoney money)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Money/AddMoney", money);
            //   if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }
        public async Task<bool> RemoveMoney(AddMoney money)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Money/RemoveMoney", money);
            //   if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<decimal> GetAvailableMoney()
        {
            var result = await _httpClient.GetFromJsonAsync<decimal>($"api/Portfolio/GetCashLedger");

            //if (result.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    var content = result.Content.ReadAsStringAsync();
            //    return Convert.ToDecimal(content);
            //}
            //   if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());

            return result;
        }
        public async Task<List<CashLedgerHistory>> GetTransactions()
        {
            var result = await _httpClient.GetFromJsonAsync<List<CashLedgerHistory>>($"api/Money/GetTransactions");

            return result;


        }
    }
}
