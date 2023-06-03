#if VISUAL_SCRIPTING_ENABLE

using System.Collections;
using System.Collections.Generic;
using EGF.Runtime.Behavior;
using Unity.VisualScripting;
using UnityEngine;

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
            stateFeedback = ValueInput<State>("feedback");
        }

        protected State InvokeNextNode(Flow flow)
        {
            var stack = flow.PreserveStack();
            
            flow.Invoke(nextTick);
            flow.RestoreStack(stack);
            
            flow.DisposePreservedStack(stack);
            
            var childState = flow.GetValue<State>(stateFeedback);
            return childState;
        }
    }
}

#endif