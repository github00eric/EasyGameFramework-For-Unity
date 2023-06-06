#if VISUAL_SCRIPTING_ENABLE

using Unity.VisualScripting;

namespace EGF.Runtime.Behavior
{
    [UnitCategory("BehaviorTree/Decorator")]
    [UnitSubtitle("BT Decorator")]
    public abstract class DecoratorNode : BehaviorTreeNode
    {
        private bool _nextAbort;
        
        [DoNotSerialize] public ControlOutput nextTick;
        [DoNotSerialize] public ValueOutput nextAbort;

        [DoNotSerialize] public ValueInput stateFeedback;
        
        protected override void Definition()
        {
            nextAbort = ValueOutput(nameof(nextAbort), flow => _nextAbort);

            base.Definition();
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