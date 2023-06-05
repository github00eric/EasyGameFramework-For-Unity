#if VISUAL_SCRIPTING_ENABLE

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace EGF.Runtime.Behavior
{
    [TypeIcon(typeof(Timer))]
    public class Timeout : DecoratorNode
    {
        float _startTime;

        [SerializeAs(nameof(Duration))] private float _duration = 1.0f;
        [SerializeAs(nameof(TimeoutAsSuccess))] private bool _timeoutAsSuccess = false;

        [DoNotSerialize][Inspectable]
        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        [DoNotSerialize][Inspectable]
        public bool TimeoutAsSuccess
        {
            get => _timeoutAsSuccess;
            set => _timeoutAsSuccess = value;
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
            if (Time.time - _startTime > _duration)
            {
                Abort(flow);
                return _timeoutAsSuccess ? BehaviorTreeState.Success : BehaviorTreeState.Failure;
            }

            return InvokeNextNode(flow);
        }
    }
}

#endif
