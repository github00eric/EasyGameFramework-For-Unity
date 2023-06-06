#if VISUAL_SCRIPTING_ENABLE

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;

namespace EGF.Runtime.Behavior
{
    [UnitCategory("BehaviorTree/Composite")]
    [TypeIcon(typeof(If))]
    public abstract class CompositeNode : BehaviorTreeNode
    {
        private bool _nextAbort;
        
        [SerializeAs(nameof(BranchCount))]
        private int _branchCount = 2;
    
        [DoNotSerialize][Inspectable, InspectorLabel("Branch Count"), UnitHeaderInspectable("Branch")]
        public int BranchCount
        {
            get => _branchCount;
            set => _branchCount = value;
        }
        
        [DoNotSerialize] public ReadOnlyCollection<ControlOutput> NextTicks { get; private set; }
        [DoNotSerialize] public ValueOutput nextAbort;

        [DoNotSerialize] public ReadOnlyCollection<ValueInput> StateFeedbacks { get; private set; }

        protected override void Definition()
        {
            nextAbort = ValueOutput(nameof(nextAbort), flow => _nextAbort);
            
            base.Definition();
            
            var multiOutputs = new List<ControlOutput>();
            var multiFeedbacks = new List<ValueInput>();
            for (int i = 0; i < BranchCount; i++)
            {
                var output = ControlOutput(i.ToString());
                var feedback = ValueInput<BehaviorTreeState>(i.ToString());
            
                multiOutputs.Add(output);
                multiFeedbacks.Add(feedback);
                
                Requirement(feedback, tick);
            }
            NextTicks = multiOutputs.AsReadOnly();
            StateFeedbacks = multiFeedbacks.AsReadOnly();
        }
        
        protected override void Abort(Flow flow)
        {
            _nextAbort = true;
            InvokeAllNextNodes(flow);
            _nextAbort = false;
            
            base.Abort(flow);
        }

        protected List<BehaviorTreeState> InvokeAllNextNodes(Flow flow)
        {
            var stack = flow.PreserveStack();

            foreach (var nextTick in NextTicks)
            {
                flow.Invoke(nextTick);
                flow.RestoreStack(stack);
            }
            
            flow.DisposePreservedStack(stack);

            var childStates = new List<BehaviorTreeState>();
            foreach (var feedback in StateFeedbacks)
            {
                var feedbackState = flow.GetValue<BehaviorTreeState>(feedback);
                childStates.Add(feedbackState);
            }

            return childStates;
        }
        
        protected BehaviorTreeState InvokeNextNode(Flow flow, int index)
        {
            if (index < 0 || index > StateFeedbacks.Count) return BehaviorTreeState.Failure;
            
            var stack = flow.PreserveStack();
            flow.Invoke(NextTicks[index]);
            flow.RestoreStack(stack);
            flow.DisposePreservedStack(stack);
            
            var childState = flow.GetValue<BehaviorTreeState>(StateFeedbacks[index]);
            return childState;
        }
    }
}

#endif