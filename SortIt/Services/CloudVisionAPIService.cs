using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace SortIt.Services
{
    // REST API teenus Google Cloud Vision API jaoks
    public class CloudVisionAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _url;

        public CloudVisionAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Loeb API võtme appsettings.json failist
            using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").Result;

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            _apiKey = config["GoogleVision:ApiKey"];
            _url = "https://vision.googleapis.com/v1/images:annotate?key=" + _apiKey;
        }

        public async Task<(string? label, double confidence)> DetectObjectAsync(string imagePath)
        {
            if (!File.Exists(imagePath))
                return (null, 0);

            // Loeb pildi byte massiiviks
            byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);

            // Teisendab Base64 formaati
            string base64 = Convert.ToBase64String(imageBytes);

            // Loome päringu objekti Google Cloud Vision API jaoks
            var requestObject = new
            {
                requests = new[]
                {
                    new
                    {
                        image = new { content = base64 },
                        features = new[]
                        {
                            new { type = "LABEL_DETECTION", maxResults = 5 }
                        }
                    }
                }
            };

            // Serialiseerib JSON-iks
            string jsonRequest = JsonSerializer.Serialize(requestObject);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Saadab POST päringu
            var response = await _httpClient.PostAsync(_url, content);

            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return (null, 0);

            // Parsib JSON vastuse
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);

            var labels = doc.RootElement
                .GetProperty("responses")[0]
                .GetProperty("labelAnnotations");

            // Võtab esimese tuvastatud objekti
            if (labels.GetArrayLength() > 0)
            {
                string label = labels[0].GetProperty("description").GetString();
                double score = labels[0].GetProperty("score").GetDouble();

                return (label, score);
            }

            return (null, 0);
        }
    }
}