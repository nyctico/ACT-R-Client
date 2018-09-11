using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nyctico.Actr.Client.Data
{
    public class Result
    {
        [JsonProperty(PropertyName = "result")]
        public dynamic[] AllRetruns { set; get; }

        [JsonIgnore] public long ReturnLong => (long) AllRetruns[0];

        [JsonIgnore] public double ReturnDouble => (double) AllRetruns[0];

        [JsonIgnore] public string ReturnString => (string) AllRetruns[0];

        [JsonIgnore] public object ReturnObject => AllRetruns[0];

        [JsonIgnore] public JArray ReturnValue => (JArray) AllRetruns[0];

        [JsonIgnore] public dynamic[] ReturnList => ((JArray) AllRetruns[0]).ToObject<dynamic[]>();

        [JsonProperty(PropertyName = "error")] public Error Error { set; get; }

        [JsonProperty(PropertyName = "id")] public int Id { set; get; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this) + "\x04";
        }
    }
}