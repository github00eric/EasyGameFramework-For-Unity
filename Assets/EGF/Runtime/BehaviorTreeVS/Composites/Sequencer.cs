#if VISUAL_SCRIPTING_ENABLE

using Unity.VisualScripting;

namespace EGF.Runtime.Behavior
{
    [TypeIcon(typeof(Sequence))]
    public class Sequencer : CompositeNode
    {
        private int _current;

        protected override void OnStart()
        {
            _current = 0;
        }

        protected override void OnStop()
        {
        }

        protected override BehaviorTreeState OnRunning(Flow flow)
        {
            for (int i = _current; i < NextTicks.Count; ++i)
            {
                _current = i;
                var childState = InvokeNextNode(flow,i);
                
                switch (childState)
                {
                    case BehaviorTreeState.Running:
                        return BehaviorTreeState.Running;
                    case BehaviorTreeState.Failure:
                        return BehaviorTreeState.Failure;
                    case BehaviorTreeState.Success:
                        continue;
                }
            }

            return BehaviorTreeState.Success;
        }
    }
}

#endif