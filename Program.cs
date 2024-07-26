using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Kernel.CreateBuilder();

        // Azure OpenAI를 사용하는 경우:
        // builder.AddAzureOpenAIChatCompletion("{deployment name}", "{end point}", "{key}");

        // OpenAI를 사용하는 경우:
        builder.AddOpenAIChatCompletion("gpt-3.5-turbo", "key");

        var kernel = builder.Build();

        var chatService = kernel.GetRequiredService<IChatCompletionService>();

        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("나는 요리 레시피를 안내하는 챗봇이야. 답변은 따뜻하게 해줘.");

        while (true)
        {
            Console.Write("User : ");
            var input = Console.ReadLine();
            Console.WriteLine();

            await Input(input);
        }

        async Task Input(string input)
        {
            chatHistory.AddUserMessage(input);

            Console.Write("Bot  : ");

            var result = chatService.GetStreamingChatMessageContentsAsync(chatHistory);

            await foreach (var text in result)
            {
                await Task.Delay(20);
                Console.Write(text.Content);
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
