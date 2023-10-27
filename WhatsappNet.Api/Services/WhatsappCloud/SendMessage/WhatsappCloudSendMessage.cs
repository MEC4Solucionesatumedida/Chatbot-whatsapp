using System.Text;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;

namespace WhatsappNet.Api.Services.WhatsappCloud.SendMessage
{
    public class WhatsappCloudSendMessage : IWhatsappCloudSendMessage
    {

        public async Task<bool> Execute(object model)
        {
            var client = new HttpClient();
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
            using (var content = new ByteArrayContent(byteData))
            {
                string endpoint = "https://graph.facebook.com";
                string phoneNumberID = "131982263336415";
                string accessToken = "EAASJVKGusgABOyrsabmoiuNCWQzclzdk4dONGUZBOnGZCdNYxSQmdS4KyLEj7lulmNu8d0YSsAzroTjZAZCJigboihEBaERzzFF4LwETZBpGsTfb5qhPZBUrvv7QakyMVbryiK8MH5geE6585JCv7te2ZBIU80tmcxVcUXx1xt3gGZCxx8Y6NlizIH813hsBn1Ee";
                string uri = $"{endpoint}/v17.0/{phoneNumberID}/messages";

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;

                }
                return false;


            }

        }
    }
}
