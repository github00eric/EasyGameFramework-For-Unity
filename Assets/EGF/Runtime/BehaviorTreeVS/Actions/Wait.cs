#if VISUAL_SCRIPTING_ENABLE

using Unity.VisualScripting;
using UnityEngine;

namespace EGF.Runtime.Behavior
{
    public class Wait : ActionNode
    {
        float _startTime;
        
        [DoNotSerialize] public ValueInput waitTime;

        protected override void Definition()
        {
            base.Definition();
            waitTime = ValueInput<float>(nameof(waitTime), 1);
        }
        
        protected override void OnStart()
        {
            _startTime = Time.time;
        }

        protected override void OnStop()
        {
        }

        protected override BehaviorTreeState OnRunning(Flow flow)
        {
            var duration = flow.GetValue<float>(waitTime);
            float timeRemaining = Time.time - _startTime;
            if (timeRemaining > duration) {
                return BehaviorTreeState.Success;
            }
            return BehaviorTreeState.Running;
        }
    }
}

#endif