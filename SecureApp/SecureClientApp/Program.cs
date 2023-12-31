﻿using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SecureClientApp
{
    internal class Program
    {
        private const string filePath = "C:\\source\\CsharpSecurity\\SecureNETCoreAPIwithBearerAuthentication\\SecureApp\\SecureClientApp\\appsettings.json";

        static void Main(string[] args)
        {
            Console.WriteLine("Making the call...");
            RunAsync(filePath).GetAwaiter().GetResult();
        }

        private static async Task RunAsync(string filePath)
        {
            AuthConfig config = AuthConfig.ReadFromJsonFile(filePath);

            IConfidentialClientApplication app;

            app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                .WithClientSecret(config.ClientSecret)
                .WithAuthority(new Uri(config.Authority))
                .Build();

            string[] ResourceIds = new string[] { config.ResourceID };

            AuthenticationResult result = null;
            try
            {
                result = await app.AcquireTokenForClient(ResourceIds).ExecuteAsync();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Token acquired \n");
                Console.WriteLine(result.AccessToken);
                Console.ResetColor();
            }
            catch (MsalClientException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

            if (string.IsNullOrEmpty(result.AccessToken)) return;

            HttpClient httpClient = SetHttpClient(result);

            HttpResponseMessage response = await httpClient.GetAsync(config.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to call the Web Api: {response.StatusCode}");
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Content: {content}");
            }

            Console.ResetColor();
        }

        private static HttpClient SetHttpClient(AuthenticationResult result)
        {
            var httpClient = new HttpClient();
            var defaultRequestHeaders = httpClient.DefaultRequestHeaders;

            if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);
            return httpClient;
        }
    }
}
