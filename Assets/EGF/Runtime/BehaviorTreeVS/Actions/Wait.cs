using System.Collections;
using System.Collections.Generic;
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

        protected override State OnRunning(Flow flow)
        {
            float timeRemaining = Time.time - _startTime;
            if (timeRemaining > duration) {
                return State.Success;
            }
            return State.Running;
        }
    }
}
