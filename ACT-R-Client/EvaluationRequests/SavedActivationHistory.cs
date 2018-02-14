namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SavedActivationHistory : AbstractEvaluationRequest
    {
        public SavedActivationHistory(string model = null) : base("saved-activation-history",
            model)
        {
        }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}