using JCMG.Nodey;
using UnityEngine;

namespace EGF.Runtime
{
    [CreateNodeMenu(CreatePath + "Timeout")]
    public class Timeout : DecoratorNode
    {
        public float duration = 1.0f;
        public bool timeoutAsSuccess = false;
        float startTime;

        protected override void OnStart() {
            startTime = Time.time;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (Time.time - startTime > duration)
            {
                return timeoutAsSuccess ? State.Success : State.Failure;
            }

            return Children[0].Update();
        }
    }
}