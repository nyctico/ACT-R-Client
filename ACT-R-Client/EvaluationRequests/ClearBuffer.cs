namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ClearBuffer : AbstractEvaluationRequest
    {
        public ClearBuffer(string buffer = null, string model = null) : base("clear-buffer",
            model)
        {
            Buffer = buffer;
        }

        public string Buffer { get; set; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Buffer);

            return list.ToArray();
        }
    }
}