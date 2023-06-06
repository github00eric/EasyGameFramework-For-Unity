#if VISUAL_SCRIPTING_ENABLE

using Unity.VisualScripting;

namespace EGF.Runtime.Behavior
{
    [UnitCategory("BehaviorTree")]
    [TypeIcon(typeof(BoltUnityEvent))]
    public class RootNode : BehaviorTreeNode
    {
        private bool _nextAbort;

        [DoNotSerialize] public ControlInput reset;
        [DoNotSerialize] public ControlOutput nextTick;
        [DoNotSerialize] public ValueOutput nextAbort;

        [DoNotSerialize] public ValueInput stateFeedback;

        protected override void Definition()
        {
            nextAbort = ValueOutput(nameof(nextAbort), flow => _nextAbort);

            base.Definition();
            reset = ControlInput(nameof(reset), flow =>
            {
                Abort(flow);
                return null;
            });
            nextTick = ControlOutput(nameof(nextTick));
            stateFeedback = ValueInput<BehaviorTreeState>("feedback");
            
            Requirement(stateFeedback, tick);
        }

        protected override void Abort(Flow flow)
        {
            _nextAbort = true;
            InvokeNextNode(flow);
            _nextAbort = false;
            
            base.Abort(flow);
        }

        protected override ControlOutput Tick(Flow flow)
        {
            if (currentState != BehaviorTreeState.Running) return null;         // 仅 Root节点 需要这个，其余子节点由Tick直接重启
            
            return base.Tick(flow);
        }

        BehaviorTreeState InvokeNextNode(Flow flow)
        {
            var stack = flow.PreserveStack();
            
            flow.Invoke(nextTick);
            flow.RestoreStack(stack);
            
            flow.DisposePreservedStack(stack);
            
            var childState = flow.GetValue<BehaviorTreeState>(stateFeedback);
            return childState;
        }

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override BehaviorTreeState OnRunning(Flow flow)
        {
            var subState = InvokeNextNode(flow);
            return subState;
        }
    }
}

#endif
