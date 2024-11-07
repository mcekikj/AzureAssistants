using Azure.AI.OpenAI;
using Azure;
using Microsoft.Extensions.Configuration;
using AssistantsWrapper.Functions;

namespace AssistantsWrapper.Helpers
{
    public static class OpenAIClientHelper
    {
        public static (OpenAIClient client, ChatCompletionsOptions chatCompletionsOptions, string model) InitializeOpenAIClient(IConfiguration configuration)
        {
            Uri azureOpenAIEndpoint = new(configuration["AzureOpenAI:Endpoint"]);
            string azureOpenAIKey = configuration["AzureOpenAI:Key"];
            string azureOpenAIModel = configuration["AzureOpenAI:Model"];

            CmdHelper.PrintMessage("Initializing OpenAI client...", ConsoleColor.Cyan);
            OpenAIClient client = new(azureOpenAIEndpoint, new AzureKeyCredential(azureOpenAIKey));

            ChatCompletionsOptions chatCompletionsOptions = new();
            string systemPrompt = "You are an expert assistant that provides detailed information about various venues.";
            AddMessageToChat(chatCompletionsOptions, systemPrompt, ChatRole.System);

            var getVenueInfoFunctionDefinition = GetVenueInfoFunction.GetFunctionDefinition();
            chatCompletionsOptions.Functions.Add(getVenueInfoFunctionDefinition);
            CmdHelper.PrintMessage("Function definition added: " + getVenueInfoFunctionDefinition.Name, ConsoleColor.Green);

            return (client, chatCompletionsOptions, azureOpenAIModel);
        }

        public static async Task<ChatChoice> GetLLMResponse(OpenAIClient client, ChatCompletionsOptions options, string model)
        {
            CmdHelper.PrintMessage("Calling LLM for response...", ConsoleColor.Cyan);
            try
            {
                ChatCompletions response = await client.GetChatCompletionsAsync(model, options);
                return response.Choices[0];
            }
            catch (Exception ex)
            {
                CmdHelper.PrintMessage("Error calling LLM: " + ex.Message, ConsoleColor.Red);
                throw;
            }
        }

        public static void AddMessageToChat(ChatCompletionsOptions options, string content, ChatRole role)
        {
            options.Messages.Add(new ChatMessage(role, content));
        }
    }
}