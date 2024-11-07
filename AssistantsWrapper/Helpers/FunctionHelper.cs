using Azure.AI.OpenAI;
using System.Text.Json;
using static AssistantsWrapper.Functions.GetVenueInfoFunction;

namespace AssistantsWrapper.Helpers
{
    public class FunctionHelper
    {
        public static void ProcessFunctionCall(string arguments, ChatCompletionsOptions chatCompletionsOptions)
        {
            try
            {
                var input = JsonSerializer.Deserialize<VenueInput>(arguments, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;
                CmdHelper.PrintMessage("Deserialized function arguments: " + arguments, ConsoleColor.Green);

                var functionResultData = GetVenue(input.Name);
                var functionResponseMessage = new ChatMessage(
                    ChatRole.Function,
                    JsonSerializer.Serialize(functionResultData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                {
                    Name = Name
                };

                chatCompletionsOptions.Messages.Add(functionResponseMessage);
                CmdHelper.PrintMessage("Added function response to chat history.", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                CmdHelper.PrintMessage("Error processing function call: " + ex.Message, ConsoleColor.Red);
                throw;
            }
        }
    }
}