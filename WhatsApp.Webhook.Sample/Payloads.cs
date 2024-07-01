

using Newtonsoft.Json;

namespace WhatsApp.Webhook.API
{

    public class WebhookMessage
    {
        [JsonConstructor]
        public WebhookMessage()
        {
            
        }

        [JsonProperty("object")]
        public string _object { get; set; }

        [JsonProperty("entry")]
        public Entry[] entries { get; set; }
    }

    public class Entry
    {
        [JsonConstructor]
        public Entry()
        {
            
        }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("changes")]
        public Change[] changes { get; set; }
    }

    public class Change
    {
        [JsonConstructor]
        public Change()
        {
            
        }

        [JsonProperty("value")]
        public Value value { get; set; }

        [JsonProperty("field")]
        public string field { get; set; }
    }

    public class Value
    {
        [JsonConstructor]
        public Value()
        {            
        }

        [JsonProperty("messaging_product")]
        public string messaging_product { get; set; }

        [JsonProperty("metadata")]
        public Metadata metadata { get; set; }

        [JsonProperty("contacts")]
        public Contact[] contacts { get; set; }

        [JsonProperty("messages")]
        public Message[] messages { get; set; }
    }

    public class Metadata
    {
        [JsonConstructor]
        public Metadata()
        {
            
        }

        [JsonProperty("display_phone_number")]
        public string display_phone_number { get; set; }

        [JsonProperty("phone_number_id")]
        public string phone_number_id { get; set; }
    }

    public class Contact
    {
        [JsonConstructor]
        public Contact()
        {
            
        }

        [JsonProperty("profile")]
        public Profile profile { get; set; }

        [JsonProperty("wa_id")]
        public string wa_id { get; set; }
    }

    public class Profile
    {
        [JsonConstructor]
        public Profile()
        {
            
        }

        [JsonProperty("name")]
        public string name { get; set; }
    }




    #region Tipos de mensagem
    public class Message
    {
        [JsonConstructor]
        public Message()
        {
            
        }
        [JsonProperty("from")]
        public string from { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("timestamp")]
        public string timestamp { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
    }

    public class TextMessage : Message
    {
        [JsonConstructor]
        public TextMessage() : base()
        {
            
        }

        [JsonProperty("text")]
        public Text text { get; set; }
    }
    public class Text
    {
        [JsonConstructor]
        public Text()
        {
            
        }

        [JsonProperty("body")]
        public string body { get; set; }
    }


    public class ReactionMessage : Message
    {

        [JsonConstructor]
        public ReactionMessage()
        {
            
        }

        [JsonProperty("reaction")]
        public Reaction reaction { get; set; }
    }

    public class Reaction
    {
        [JsonConstructor]
        public Reaction()
        {
            
        }
        [JsonProperty("message_id")]
        public string message_id { get; set; }

        [JsonProperty("emoji")]
        public string emoji { get; set; }
    }


    public class UnknownMessage: Message
    {
        [JsonConstructor]
        public UnknownMessage()
        {
            
        }

        [JsonProperty("errors")]
        public Error[] errors { get; set; }
    }

    public class Error
    {
        [JsonConstructor]
        public Error()
        {
            
        }
        [JsonProperty("code")]
        public int code { get; set; }

        [JsonProperty("details")]
        public string details { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }
    }

    #endregion
}
