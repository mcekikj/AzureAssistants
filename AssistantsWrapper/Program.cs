using Azure.AI.OpenAI;
using AssistantsWrapper.Helpers;
using AssistantsWrapper.Functions;

namespace AssistantsWrapper
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            CmdHelper.PrintMessage("--- Starting program ---", ConsoleColor.Cyan);
            var configuration = ConfigurationHelper.LoadConfiguration();
            var (client, chatCompletionsOptions, azureOpenAIModel) = OpenAIClientHelper.InitializeOpenAIClient(configuration);

            string executionSourceMessage = string.Empty;
            bool exit = false;

            Console.CancelKeyPress += (sender, e) =>
            {
                CmdHelper.PrintMessage("\nCtrl + C pressed. Exiting...", ConsoleColor.Red);
                e.Cancel = true;
                exit = true;
            };

            while (!exit)
            {
                string question = CmdHelper.GetUserInput("Enter your question (or press Ctrl + C to exit): ");
                if (string.IsNullOrWhiteSpace(question))
                {
                    CmdHelper.PrintMessage("No input provided, please try again.", ConsoleColor.Yellow);
                    continue;
                }

                OpenAIClientHelper.AddMessageToChat(chatCompletionsOptions, question, ChatRole.User);
                CmdHelper.PrintMessage("User question added: " + question, ConsoleColor.Green);
                CmdHelper.PrintPrompt(chatCompletionsOptions);

                var responseChoice = await OpenAIClientHelper.GetLLMResponse(client, chatCompletionsOptions, azureOpenAIModel);

                while (responseChoice.FinishReason == CompletionsFinishReason.FunctionCall)
                {
                    executionSourceMessage = string.Empty;
                    CmdHelper.PrintMessage("LLM identified a function call: " + responseChoice.Message.FunctionCall.Name, ConsoleColor.Red);
                    chatCompletionsOptions.Messages.Add(responseChoice.Message);

                    if (responseChoice.Message.FunctionCall.Name == GetVenueInfoFunction.Name)
                    {
                        CmdHelper.PrintMessage("Processing function: " + GetVenueInfoFunction.Name, ConsoleColor.Green);
                        FunctionHelper.ProcessFunctionCall(responseChoice.Message.FunctionCall.Arguments, chatCompletionsOptions);
                    }

                    executionSourceMessage = "Flow was executed WITH calling any user-defined function.";
                    responseChoice = await OpenAIClientHelper.GetLLMResponse(client, chatCompletionsOptions, azureOpenAIModel);
                }

                if (executionSourceMessage.Equals(string.Empty))
                    executionSourceMessage = "Flow was executed WITHOUT calling any user-defined function.";

                CmdHelper.PrintMessage("Final response received: " + executionSourceMessage, ConsoleColor.Green);
                CmdHelper.PrintMessage("Final response received: " + responseChoice.Message.Content, ConsoleColor.Green);
            }

            CmdHelper.PrintMessage("--- Program has exited ---", ConsoleColor.Cyan);
        }
    }
}