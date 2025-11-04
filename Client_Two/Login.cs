using System.Net.Http.Json;

namespace Client
{
    public class Login
    {
        public static async Task<string> GetJwtToken()
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.PostAsJsonAsync("http://10.192.34.86/login/auth", new
                {
                    username = "920314375",
                    password = "Ali@5801633"
                });

                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result.Token;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }
        public class LoginResponse
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }

            public string Username { get; set; }
            public string[] Role { get; set; }
            public string Token { get; set; }  // ✅ We only need this
            public string StoreId { get; set; }
        }
    }
}
