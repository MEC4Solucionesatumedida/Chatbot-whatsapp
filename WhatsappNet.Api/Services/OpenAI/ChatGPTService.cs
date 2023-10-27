using Microsoft.AspNetCore.SignalR.Protocol;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using System.Diagnostics;
using System.Reflection;

namespace WhatsappNet.Api.Services.OpenAI

{
    public class ChatGPTService : IChatGPTService
    {
        public async Task<string> Execute(string textUser)
        {
            try
            {

                string apiKey = "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";
                var openAiService = new OpenAIAPI(apiKey);
                var completition = new CompletionRequest
                {
                    Model = "gpt-3.5-turbo-instruct",

                    Temperature = 0,
                    
                    Prompt = textUser,
                    NumChoicesPerPrompt = 5,
                       MaxTokens = 256
                    //Model= "gpt-3.5-turbo-instruct",
                    //Prompt= "Eres un agente de cotización a partir de ahora deberás pedir tanto cantidad como el nombre del producto, y limitarte a decir que fueron registrados y no dar sugerencias de precio. Una ves que te manden los productos y la cantidad mandas tantos mensajes como productos que te envíen junto su cantidad. Ejemplo quiero cotizar 3 mesas y 3 botellas, tu mandarías, si claro 3 mesas, y mandarías otro mensaje 3 botellas.",
                    //Temperature= 0,
                    //MaxTokens= 256,
                    //NumChoicesPerPrompt = 1,
                };
                //var completion = new CompletionRequest
                //{
                //    Prompt = "Eres un agente de cotización a partir de ahora deberás pedir tanto cantidad como el nombre del producto, y limitarte a decir que fueron registrados y no dar sugerencias de precio. Una ves que te manden los productos y la cantidad mandas tantos mensajes como productos que te envíen junto su cantidad. Ejemplo quiero cotizar 3 mesas y 3 botellas, tu mandarías, si claro 3 mesas, y mandarías otro mensaje 3 botellas.",
                //    Model = "text-davinci-003",
                //    NumChoicesPerPrompt = 100,
                //    MaxTokens = 200
                ////};
                var result = await openAiService.Completions.CreateCompletionAsync(completition);
                // var result = await openAiService.Chat.CreateChatCompletionAsync();
                //var result = await openAiService.Chat.DefaultChatRequestArgs(completition); 

                if (result != null && result.Completions.Count > 0)
                    return result.Completions[0].Text;
                return "lo siento sucedio un problema";
            }
            catch (Exception ex)
            {
                return "lo siento sucedio un problema";
            }
        }
    }
}
