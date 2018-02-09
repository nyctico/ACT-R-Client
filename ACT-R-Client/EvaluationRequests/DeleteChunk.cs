using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class DeleteChunk : AbstractEvaluationRequest
    {
        public DeleteChunk(string name, string model = null) : base("delete-chunk",
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