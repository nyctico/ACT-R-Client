using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nyctico.Actr.Client.Data
{
    public class Result
    {
        [JsonProperty(PropertyName = "result")]
        public List<dynamic> ReturnList { set; get; }

        [JsonIgnore] public dynamic ReturnValue => ReturnList[0];

        [JsonProperty(PropertyName = "error")] public Error Error { set; get; }

        [JsonProperty(PropertyName = "id")] public int Id { set; get; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this) + "\x04";
        }
    }
}