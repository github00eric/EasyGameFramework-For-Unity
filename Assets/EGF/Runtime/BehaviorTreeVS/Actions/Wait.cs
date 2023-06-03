#if VISUAL_SCRIPTING_ENABLE

using Unity.VisualScripting;
using UnityEngine;

namespace EGF.Runtime.Behavior
{
    public class Wait : ActionNode
    {
        float _startTime;
        
        [Inspectable,InspectorLabel("Wait Time"),UnitHeaderInspectable("Wait Time")] 
        public float duration = 1;
        
        protected override void OnStart()
        {
            _startTime = Time.time;
        }

        protected override void OnStop()
        {
        }

        protected override BehaviorTreeState OnRunning(Flow flow)
        {
            float timeRemaining = Time.time - _startTime;
            if (timeRemaining > duration) {
                return BehaviorTreeState.Success;
            }
            return BehaviorTreeState.Running;
        }
    }
}

#endif