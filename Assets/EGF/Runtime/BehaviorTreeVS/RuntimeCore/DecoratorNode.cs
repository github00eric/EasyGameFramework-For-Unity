#if VISUAL_SCRIPTING_ENABLE

using Unity.VisualScripting;

namespace EGF.Runtime.Behavior
{
    [UnitCategory("BehaviorTree/Decorator")]
    public abstract class DecoratorNode : BehaviorTreeNode
    {
        [DoNotSerialize] public ControlOutput nextTick;

        [DoNotSerialize] public ValueInput stateFeedback;
        
        protected override void Definition()
        {
            base.Definition();
            nextTick = ControlOutput(nameof(nextTick));
            stateFeedback = ValueInput<BehaviorTreeState>("feedback");
        }

        protected BehaviorTreeState InvokeNextNode(Flow flow)
        {
            var stack = flow.PreserveStack();
            
            flow.Invoke(nextTick);
            flow.RestoreStack(stack);
            
            flow.DisposePreservedStack(stack);
            
            var childState = flow.GetValue<BehaviorTreeState>(stateFeedback);
            return childState;
        }
    }
}

#endif