namespace WhatsappNet.Api.Services.OpenAI
{
    public interface IChatGPTService
    {
        Task<string> Execute(string textUser);
    }
}
