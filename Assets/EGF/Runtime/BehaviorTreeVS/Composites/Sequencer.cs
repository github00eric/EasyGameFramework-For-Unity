using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

        protected override State OnRunning(Flow flow)
        {
            for (int i = _current; i < NextTicks.Count; ++i)
            {
                _current = i;
                var childState = InvokeNextNode(flow,i);
                
                switch (childState)
                {
                    case State.Running:
                        return State.Running;
                    case State.Failure:
                        return State.Failure;
                    case State.Success:
                        continue;
                }
            }

            return State.Success;
        }
    }
}
