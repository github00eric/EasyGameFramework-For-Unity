#if VISUAL_SCRIPTING_ENABLE

using Unity.VisualScripting;

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

        protected override BehaviorTreeState OnRunning(Flow flow)
        {
            var childState = InvokeNextNode(flow);
            
            switch (childState) {
                case BehaviorTreeState.Running:
                    break;
                case BehaviorTreeState.Failure:
                    if (_restartOnFailure) {
                        return BehaviorTreeState.Running;
                    } else {
                        return BehaviorTreeState.Failure;
                    }
                case BehaviorTreeState.Success:
                    if (_restartOnSuccess) {
                        return BehaviorTreeState.Running;
                    } else {
                        return BehaviorTreeState.Success;
                    }
            }
            return BehaviorTreeState.Running;
        }
    }
}

#endif