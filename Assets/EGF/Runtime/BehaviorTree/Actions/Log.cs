using JCMG.Nodey;
using NaughtyAttributes;
using UnityEngine;

namespace EGF.Runtime
{
    [CreateNodeMenu(CreatePath + "Log")]
    public class Log : ActionNode
    {
        public string message;

        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            Debug.Log($"{message}");
            return State.Success;
        }
    }
}