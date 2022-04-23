using StockMarket.Shared.Data.Context;
using System.Net.Http.Json;

namespace StockMarket.Client.Services.Implementations
{
    public class OrdersApi
    {
        private readonly HttpClient _httpClient;

        public OrdersApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Order>> GetListOfOrders()
        {
            try
            {

                var result = await _httpClient.GetFromJsonAsync<List<Order>>($"api/Orders/GetAllOrders");
                // if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<OrderHistory>> GetAllArchivedOrders()
        {
            try
            {

                var result = await _httpClient.GetFromJsonAsync<List<OrderHistory>>($"api/Orders/GetAllArchivedOrders");
                // if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> CancelOrder(int orderId)
        {
            var result = await _httpClient.GetAsync($"api/Orders/CancelOrder?orderId={orderId.ToString()}");
            //   if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

    }
}
