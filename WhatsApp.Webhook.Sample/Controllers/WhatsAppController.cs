using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WhatsApp.Webhook.API;
using WhatsApp.Webhook.API.Configuration;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsApp.Webhook.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController : ControllerBase
    {
        private readonly WebhookHandShakeOptions _options;
        private readonly ILogger<WhatsAppController> _logger;
        private readonly IWhatsAppBusinessClient _whatsAppBusinessClient;
        public WhatsAppController(IOptions<WebhookHandShakeOptions> options, ILogger<WhatsAppController> logger, IWhatsAppBusinessClient whatsAppBusinessClient)
        {
            _options = options.Value;
            _logger = logger;
            _whatsAppBusinessClient = whatsAppBusinessClient;
        }

        [HttpGet, Route("webhook")]
        public IActionResult GetWebhook([FromQuery(Name = "hub.mode")] string? mode, [FromQuery(Name = "hub.verify_token")] string? verificationToken, [FromQuery(Name = "hub.challenge")] string? challenge)
        {
            if (mode == "subscribe" && verificationToken == _options.VerifyToken)
                return Ok(challenge);

            return StatusCode(StatusCodes.Status403Forbidden);

        }

        [HttpPost, Route("webhook")]
        public IActionResult PostWebhook([FromBody] WebhookMessage? message)
        {
            if (message is null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var alteracoes = message.entries.SelectMany(e => e.changes);

            _logger.LogInformation("Received webhook request: {RequestId} - Request contains {MessagesCount} messages.", message._object, alteracoes.Count());

            foreach (var alteracao in alteracoes)
            {
                foreach (var mensagem in alteracao.value.messages)
                {
                    _logger.LogInformation("Marking message '{MessageId}' as read", mensagem.id);
                    var markRead = new MarkMessageRequest() { MessageId = mensagem.id, Status = "read" };
                    _whatsAppBusinessClient.MarkMessageAsRead(markRead);

                    _logger.LogInformation("Responding to '{Sender} - {PhoneId}' from message received", alteracao.value.contacts.FirstOrDefault()?.profile?.name ?? string.Empty, mensagem.from);

                    var textMessage = new TextMessageRequest() { Text = new WhatsAppText() { Body = $"Olá {alteracao.value.contacts.FirstOrDefault()?.profile?.name ?? string.Empty}, recebemos sua mensagem, em breve retornaremos.",
                        PreviewUrl = false }, To = mensagem.from };
                    _whatsAppBusinessClient.SendTextMessage(textMessage);
                }

            }

            return Ok();
        }
    }
}
