using Newtonsoft.Json;

namespace Nyctico.Actr.Client.Data
{
    public class Error
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { set; get; }

        public Error(string message)
        {
            Message = message;
        }
    }
}