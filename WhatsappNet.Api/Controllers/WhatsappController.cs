using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using WhatsappNet.Api.Services.OpenAI;
using WhatsappNet.Api.Services.WhatsappCloud.SendMessage;
using WhatsappNet.Api.Util;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WhatsappNet.Api.Controllers
{
    [ApiController]
    [Route("api/whatsapp")]
    public class WhatsappController : Controller
    {
        private readonly IWhatsappCloudSendMessage _whatsappCloudSendMessage;
        private readonly IUtil _util;
        private readonly IChatGPTService _chatGPTService;
        public WhatsappController(IWhatsappCloudSendMessage whatsappCloudSendMessage, IUtil util, IChatGPTService chatGPTService)
        {

            _whatsappCloudSendMessage = whatsappCloudSendMessage;
            _util = util;
            _chatGPTService = chatGPTService;

        }
        [HttpGet("test")]
        public async Task<IActionResult> Sample()
        {
            var data = new
            {
                messaging_product= "whatsapp",
                to= "59169434797",
                type="text",
                text = new
                {
                    body="hola app"
                }
            };
            var result = await _whatsappCloudSendMessage.Execute(data);

            return Ok("ok sample");
        }
        [HttpGet]
        public IActionResult VerifyToken()
        {
            string AccessToken = "ET2324T741TE7YDHB7G2487GNCM";

            var token = Request.Query["hub.verify_token"].ToString();
            var challenge = Request.Query["hub.challenge"].ToString();

            if (challenge != null && token != null && token == AccessToken)
            {
                return Ok(challenge);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ReceivedMessage([FromBody] WhatsAppCloudModel body)
        {
            try
            {
                var Message = body.Entry[0]?.Changes[0]?.Value?.Messages[0];
                if (Message != null)
                {
                    var userNumber = Message.From;
                    var userText = GetUserText(Message);

                    List<object> listObjectMessage= new List<object>();

                    
                    var resposeChatGPT = await _chatGPTService.Execute(userText);
                    var objectMessage = _util.TextMessage(resposeChatGPT, userNumber);
                    listObjectMessage.Add(objectMessage);


                    if (userText.ToUpper().Contains("HOLA"))
                            {
                        var objectMessage1 = _util.TextMessage("soy el bot, ¿como puedo ayudarte?😊", userNumber);
                        var objectMessage2 = _util.ImageMessage("https://www.mec4.com.bo/wp-content/uploads/2021/07/mec4pq.png", userNumber);
                        listObjectMessage.Add(objectMessage2);
                        listObjectMessage.Add(objectMessage1);

                    }
                    else if (userText.ToUpper().Contains("Buenos dias")||userText.ToUpper().Contains("Buenas tardes"))
                    {
                        var objectMessage1 = _util.TextMessage("Hola soy el bot, ¿como puedo ayudarte?😊", userNumber);
                        var objectMessage2 = _util.ImageMessage("https://www.mec4.com.bo/wp-content/uploads/2021/07/mec4pq.png", userNumber);
                        listObjectMessage.Add(objectMessage2);
                        listObjectMessage.Add(objectMessage1);

                    }
                    else
                    {
                        //var objectMessage1 = _util.TextMessage("", userNumber);
                        //listObjectMessage.Add(objectMessage1);
                    }
                    foreach(var item in listObjectMessage)
                    {
                        await _whatsappCloudSendMessage.Execute(item);
                    }

                    

                }

                return Ok("EVENT_RECEIVED");
            }catch (Exception ex)
            {
                return Ok("EVENT_RECEIVED");
            }
        }
        private string GetUserText(Message message)
        {
            string TypeMessage =message.Type;
            if (TypeMessage.ToUpper() == "TEXT")
            {
                return message.Text.Body;
            }
            else if (TypeMessage.ToUpper() == "INTERACTIVE")
            {
                string interactiveType = message.Interactive.Type;
                if (interactiveType.ToUpper() == "LIST_REPLY")
                {
                    return message.Interactive.List_Reply.Title;
                }
                else if (interactiveType.ToUpper() == "BUTTON_REPLY")
                {
                    return message.Interactive.Button_Reply.Title;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
            
    }
}
