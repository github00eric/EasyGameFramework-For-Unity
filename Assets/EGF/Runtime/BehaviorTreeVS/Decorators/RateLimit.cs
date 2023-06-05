#if VISUAL_SCRIPTING_ENABLE

using Unity.VisualScripting;
using UnityEngine;

namespace EGF.Runtime.Behavior
{
    [TypeIcon(typeof(Timer))]
    public class RateLimit : DecoratorNode
    {
        private float _timer = 0.2f;
        [SerializeAs(nameof(RateTime))] private float _rateTime;

        [DoNotSerialize][Inspectable]
        public float RateTime
        {
            get => _rateTime;
            set => _rateTime = value;
        }

        protected override void OnStart()
        {
            _timer = Time.time;
        }

        protected override void OnStop()
        {
        }

        protected override BehaviorTreeState OnRunning(Flow flow)
        {
            if (Time.time - _timer < _rateTime)
            {
                return BehaviorTreeState.Running;
            }
            
            _timer = Time.time;
            return InvokeNextNode(flow);
        }
    }
}

#endif