using Azure.AI.OpenAI;

namespace AssistantsWrapper.Helpers
{
    public static class CmdHelper
    {
        public static string GetUserInput(string prompt)
        {
            PrintMessage(prompt, ConsoleColor.Yellow);
            return Console.ReadLine();
        }

        public static void PrintPrompt(ChatCompletionsOptions options)
        {
            var roleColors = new Dictionary<string, ConsoleColor>
            {
                { ChatRole.User.ToString(), ConsoleColor.Yellow },
                { ChatRole.System.ToString(), ConsoleColor.Green },
                { ChatRole.Function.ToString(), ConsoleColor.Magenta },
                { ChatRole.Assistant.ToString(), ConsoleColor.Cyan }
            };

            foreach (var message in options.Messages)
            {
                ConsoleColor color = roleColors.ContainsKey(message.Role.ToString())
                    ? roleColors[message.Role.ToString()]
                    : ConsoleColor.White;

                PrintMessage($"Role: {message.Role}, Content: {message.Content}", color);
            }
        }

        public static void PrintMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}