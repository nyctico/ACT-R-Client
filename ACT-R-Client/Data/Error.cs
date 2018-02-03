using Newtonsoft.Json;

namespace Nyctico.Actr.Client.Data
{
    public class Error
    {
        public Error(string message)
        {
            Message = message;
        }

        [JsonProperty(PropertyName = "message")]
        public string Message { set; get; }
    }
}