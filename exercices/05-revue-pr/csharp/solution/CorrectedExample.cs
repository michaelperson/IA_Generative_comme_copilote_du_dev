// CorrectedExample.cs
// Version corrigée à la suite de la revue de code (voir REVIEW.md).
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OrderProcessing
{
    /// <summary>
    /// Récupère les commandes en attente depuis l'API des commandes et les traite.
    /// </summary>
    public class OrderService
    {
        // Nombre de tentatives avant abandon en cas d'échec de l'appel API.
        private const int MaxRetryAttempts = 3;

        // Délai d'attente entre deux tentatives (en millisecondes).
        private const int RetryDelayMilliseconds = 30_000;

        private readonly HttpClient _httpClient;
        private readonly ILogger<OrderService> _logger;

        public OrderService(HttpClient httpClient, ILogger<OrderService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Récupère les commandes en attente depuis l'API et retourne le nombre de commandes traitées.
        /// Réessaie jusqu'à <see cref="MaxRetryAttempts"/> fois en cas d'échec, avec un délai fixe entre
        /// chaque tentative. Utilise System.Text.Json (déjà inclus dans le runtime .NET), ce qui évite
        /// d'introduire une dépendance externe supplémentaire (Newtonsoft.Json) pour un besoin déjà couvert.
        /// </summary>
        /// <param name="cancellationToken">Jeton permettant à l'appelant d'annuler l'opération en cours.</param>
        public async Task<int> FetchAndProcessOrdersAsync(CancellationToken cancellationToken)
        {
            var processedCount = 0;

            for (var attempt = 0; attempt < MaxRetryAttempts; attempt++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var response = await _httpClient.GetAsync(
                        "https://api.example.com/orders/pending", cancellationToken);
                    var body = await response.Content.ReadAsStringAsync(cancellationToken);
                    var orders = JsonSerializer.Deserialize<List<string>>(body);
                    processedCount += orders?.Count ?? 0;
                    return processedCount;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Échec de la tentative {Attempt}/{MaxAttempts} de récupération des commandes en attente",
                        attempt + 1,
                        MaxRetryAttempts);
                }

                await Task.Delay(RetryDelayMilliseconds, cancellationToken);
            }

            return processedCount;
        }
    }
}
