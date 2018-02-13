using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nyctico.Actr.Client.Data
{
    public class Hook
    {
        [JsonProperty(PropertyName = "method")]
        public string Method { set; get; }

        [JsonProperty(PropertyName = "id")] public int Id { set; get; }

        [JsonProperty(PropertyName = "params")]
        public List<object> Parameters { set; get; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this) + "\x04";
        }
    }
}