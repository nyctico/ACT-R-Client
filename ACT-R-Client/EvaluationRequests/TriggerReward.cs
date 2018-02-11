using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class TriggerReward : AbstractEvaluationRequest
    {
        public TriggerReward(long rewardValue, bool maintenance = false, string model = null) :
            base("trigger-reward",
                model)
        {
            RewardValue = rewardValue;
            Maintenance = maintenance;
        }

        public long RewardValue { get; set; }
        public bool Maintenance { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(RewardValue);
            list.Add(Maintenance);

            return list;
        }
    }
}