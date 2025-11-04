// See https://aka.ms/new-console-template for more information

using Client_Three;
using Microsoft.AspNetCore.SignalR.Client;

var token = await Login.GetJwtToken();

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
    Console.WriteLine("Client 3 Connected to SignalR Hub.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error connecting: {ex.Message}");
    return;
}

// 4. Send messages in a loop
while (true)
{
    var text = Console.ReadLine();

    await connection.InvokeAsync("SendNewMessage", "Client ", text);
}
