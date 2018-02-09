using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class DeleteChunk : AbstractEvaluationRequest
    {
        public DeleteChunk(string name, bool useModel = false, string model = null) : base("delete-chunk", useModel,
            model)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Name);

            return list;
        }
    }
}