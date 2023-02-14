using JCMG.Nodey;
using UnityEngine;

namespace EGF.Runtime
{
    [CreateNodeMenu(CreatePath + "PauseEditor")]
    public class PauseEditor : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Break();
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return State.Success;
        }
    }
}