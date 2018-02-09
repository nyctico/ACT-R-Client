using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class TriggerReward : AbstractEvaluationRequest
    {
        public TriggerReward(string reward, bool maintenance = false, string model = null) :
            base("trigger-reward",
                model)
        {
            Reward = reward;
            Maintenance = maintenance;
        }

        public string Reward { get; set; }
        public bool Maintenance { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Reward);
            list.Add(Maintenance);

            return list;
        }
    }
}