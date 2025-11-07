using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PoolBinanceMonitor.Services
{
    public class BinanceMiningService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _secretKey;

        public BinanceMiningService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("BINANCE_API_KEY");
            _secretKey = Environment.GetEnvironmentVariable("BINANCE_SECRET_KEY");
            if (string.IsNullOrWhiteSpace(_apiKey) || string.IsNullOrWhiteSpace(_secretKey)) { 
                throw new InvalidOperationException("Variáveis de ambiente BINANCE_API_KEY e BINANCE_SECRET_KEY não encontradas.");
            }
        }

        public async Task<string> GetUserMiningStatusAsync(string algo, string userName)
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            string queryString = $"algo={algo}&userName={userName}&timestamp={timestamp}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
            string signature = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(queryString)))
                .Replace("-", "").ToLower();

            string url = $"https://api.binance.com/sapi/v1/mining/statistics/user/status?{queryString}&signature={signature}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", _apiKey);

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            return $"Status: {response.StatusCode}\n{content}";
        }
    }
}
