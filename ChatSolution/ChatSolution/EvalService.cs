using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace EvalServiceLibrary
{
    [DataContract]
    public class Eval
    {
        [DataMember]
        public string Submitter;
        [DataMember]
        public DateTime Timesent;
        [DataMember]
        public string Comments;
    }

    [ServiceContract]
    public interface IEvalService
    {
        [OperationContract]
        void SubmitEval(Eval eval);
        [OperationContract]
        List<Eval> GetEvals();
    }

    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class EvalService : IEvalService
    {
        List<Eval> evals = new List<Eval>();

        public void SubmitEval(Eval eval)
        {
            evals.Add(eval);
        }

        public List<Eval> GetEvals()
        {
            return evals;
        }
    }

}
