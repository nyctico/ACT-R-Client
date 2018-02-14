﻿namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class UndefineModule : AbstractEvaluationRequest
    {
        public UndefineModule(string moduleName, string model = null) : base("undefine-module",
            model)
        {
            ModuleName = moduleName;
        }

        public string ModuleName { get; set; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ModuleName);

            return list.ToArray();
        }
    }
}