#if VISUAL_SCRIPTING_ENABLE

using Unity.VisualScripting;

namespace EGF.Runtime.Behavior
{
    [UnitCategory("BehaviorTree")]
    [TypeIcon(typeof(BoltUnityEvent))]
    public class RootNode : BehaviorTreeNode
    {
        [DoNotSerialize] public ControlOutput nextTick;

        [DoNotSerialize] public ValueInput stateFeedback;

        protected override void Definition()
        {
            base.Definition();
            nextTick = ControlOutput(nameof(nextTick));
            stateFeedback = ValueInput<BehaviorTreeState>("feedback");
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
