using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nyctico.Actr.Client.Data
{
    public class Result
    {
        [JsonProperty(PropertyName = "result")]
        public List<object> AllRetruns { set; get; }

        [JsonIgnore]
        public long ReturnLong
        {
            get { return (long) AllRetruns[0]; }
        }

        [JsonIgnore]
        public double ReturnDouble
        {
            get { return (double) AllRetruns[0]; }
        }

        [JsonIgnore]
        public string ReturnString
        {
            get { return (string) AllRetruns[0]; }
        }

        [JsonIgnore]
        public object ReturnObject
        {
            get { return AllRetruns[0]; }
        }

        [JsonIgnore]
        public JArray ReturnValue
        {
            get { return (JArray) AllRetruns[0]; }
        }

        [JsonIgnore]
        public List<object> ReturnList
        {
            get { return ((JArray) AllRetruns[0]).ToObject<List<object>>(); }
        }

        [JsonProperty(PropertyName = "error")] public Error Error { set; get; }

        [JsonProperty(PropertyName = "id")] public int Id { set; get; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this) + "\x04";
        }
    }
}