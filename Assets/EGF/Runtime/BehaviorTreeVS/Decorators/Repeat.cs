using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace EGF.Runtime.Behavior
{
    [TypeIcon(typeof(Update))]
    public class Repeat : DecoratorNode
    {
        [SerializeAs(nameof(FailureRestart))] private bool _restartOnFailure;
        [SerializeAs(nameof(SuccessRestart))] private bool _restartOnSuccess;

        [DoNotSerialize][Inspectable]
        public bool FailureRestart
        {
            get => _restartOnFailure;
            set => _restartOnFailure = value;
        }
        
        [DoNotSerialize][Inspectable]
        public bool SuccessRestart
        {
            get => _restartOnSuccess;
            set => _restartOnSuccess = value;
        }
        
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnRunning(Flow flow)
        {
            var childState = InvokeNextNode(flow);
            
            switch (childState) {
                case State.Running:
                    break;
                case State.Failure:
                    if (_restartOnFailure) {
                        return State.Running;
                    } else {
                        return State.Failure;
                    }
                case State.Success:
                    if (_restartOnSuccess) {
                        return State.Running;
                    } else {
                        return State.Success;
                    }
            }
            return State.Running;
        }
    }
}
