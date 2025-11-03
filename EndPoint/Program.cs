//// See https://aka.ms/new-console-template for more information
//using EndPoint;
//using Microsoft.AspNetCore.SignalR.Client;

//var connection = new HubConnectionBuilder()
//    .WithUrl("https://localhost:7055/UpdateDeliveryStatus")
//    .WithAutomaticReconnect()
//    .Build();

//connection.On<UpdateDeliveryStatusHubDTO>("getDeliveryState", dto =>
//{
//    Console.WriteLine($"Status: {dto.Status}, Courier: {dto.CourierFullName}");
//});

//await connection.StartAsync();
//Console.WriteLine("Connected to SignalR hub.");



//using Microsoft.AspNetCore.SignalR.Client;

using EndPoint;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;
class Program
{
    static async Task Main(string[] args)
    {
        var token = await GetJwtToken();

        // 1. Create the connection
        var connection = new HubConnectionBuilder()
        .WithUrl("https://localhost:7274/chathub", options =>
        {
            options.AccessTokenProvider = async () => token;
        }).WithAutomaticReconnect()
            .Build();




        // 2. Set up a handler for incoming messages
        connection.On<MessageDto>("getNewMessage", dto =>
        {
            Console.WriteLine($"Sender: {dto.Sender},message :{dto.Message} Time: {dto.SendTime}");
        });

        try
        {
            // 3. Connect
            await connection.StartAsync();
            Console.WriteLine("Connected to SignalR Hub.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting: {ex.Message}");
            return;
        }

        // 4. Send messages in a loop
        while (true)
        {
            // Console.Write("Message: ");
            var text = Console.ReadLine();

            //  if (string.IsNullOrEmpty(text)) break;

            await connection.InvokeAsync("SendNewMessage", "Client ", text);
        }

        // 5. Stop
        await connection.StopAsync();
    }

    private static async Task<string> GetJwtToken()
    {
        try
        {
            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync("http://10.192.34.86/login/auth", new
            {
                username = "920313857",
                password = "Milad_4787"
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

