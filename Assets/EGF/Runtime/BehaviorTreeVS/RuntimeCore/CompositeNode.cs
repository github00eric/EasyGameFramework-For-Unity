#if VISUAL_SCRIPTING_ENABLE

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

namespace EGF.Runtime.Behavior
{
    [UnitCategory("BehaviorTree/Composite")]
    public abstract class CompositeNode : BehaviorTreeNode
    {
        [SerializeAs(nameof(BranchCount))]
        private int _branchCount = 2;
    
        [DoNotSerialize][Inspectable, InspectorLabel("Branch Count"), UnitHeaderInspectable("Branch")]
        public int BranchCount
        {
            get => _branchCount;
            set => _branchCount = value;
        }
        
        [DoNotSerialize] public ReadOnlyCollection<ControlOutput> NextTicks { get; private set; }

        [DoNotSerialize] public ReadOnlyCollection<ValueInput> StateFeedbacks { get; private set; }

        protected override void Definition()
        {
            base.Definition();
            
            var multiOutputs = new List<ControlOutput>();
            var multiFeedbacks = new List<ValueInput>();
            for (int i = 0; i < BranchCount; i++)
            {
                var output = ControlOutput(i.ToString());
                var feedback = ValueInput<State>(i.ToString());
            
                multiOutputs.Add(output);
                multiFeedbacks.Add(feedback);
            }
            NextTicks = multiOutputs.AsReadOnly();
            StateFeedbacks = multiFeedbacks.AsReadOnly();
        }

        protected List<State> InvokeAllNextNodes(Flow flow)
        {
            var stack = flow.PreserveStack();

            foreach (var nextTick in NextTicks)
            {
                flow.Invoke(nextTick);
                flow.RestoreStack(stack);
            }
            
            flow.DisposePreservedStack(stack);

            var childStates = new List<State>();
            foreach (var feedback in StateFeedbacks)
            {
                var feedbackState = flow.GetValue<State>(feedback);
                childStates.Add(feedbackState);
            }

            return childStates;
        }
        
        protected State InvokeNextNode(Flow flow, int index)
        {
            if (index < 0 || index > StateFeedbacks.Count) return State.Failure;
            
            var stack = flow.PreserveStack();
            flow.Invoke(NextTicks[index]);
            flow.RestoreStack(stack);
            flow.DisposePreservedStack(stack);
            
            var childState = flow.GetValue<State>(StateFeedbacks[index]);
            return childState;
        }
    }
}

#endif