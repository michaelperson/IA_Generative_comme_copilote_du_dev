// OrderService.cs
// Code proposé dans la Pull Request : récupération et traitement des commandes en attente.
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OrderProcessing
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupère les commandes en attente depuis l'API et les traite
        public async Task<int> FetchAndProcessOrdersAsync()
        {
            var resultat = 0;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var response = await _httpClient.GetAsync("https://api.example.com/orders/pending");
                    var body = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<string>>(body);
                    resultat += orders.Count;
                    return resultat;
                }
                catch (Exception)
                {
                }

                await Task.Delay(30000);
            }

            return resultat;
        }
    }
}
