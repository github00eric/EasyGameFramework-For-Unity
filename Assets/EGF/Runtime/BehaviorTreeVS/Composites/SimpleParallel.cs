#if VISUAL_SCRIPTING_ENABLE

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace EGF.Runtime.Behavior
{
    public class SimpleParallel : Parallel
    {
        [DoNotSerialize] public ControlOutput mainTask;

        [DoNotSerialize] public ValueInput mainState;
        
        protected override void Definition()
        {
            mainTask = ControlOutput(nameof(mainTask));
            mainState = ValueInput<BehaviorTreeState>(nameof(mainState));
            
            base.Definition();
            
            Requirement(mainState, tick);
        }

        BehaviorTreeState InvokeMainTask(Flow flow)
        {
            var stack = flow.PreserveStack();
            flow.Invoke(mainTask);
            flow.RestoreStack(stack);
            flow.DisposePreservedStack(stack);
            
            var state = flow.GetValue<BehaviorTreeState>(mainState);
            return state;
        }

        protected override BehaviorTreeState OnRunning(Flow flow)
        {
            // 主任务
            var mainStatus = InvokeMainTask(flow);
            
            // 其余任务
            bool stillRunning = false;
            for (int i = 0; i < BranchCount; ++i)
            {
                if (childrenLeftToExecute[i] != BehaviorTreeState.Running) continue;
                
                var status = InvokeNextNode(flow, i);
                if (status == BehaviorTreeState.Running) {
                    stillRunning = true;
                }
                childrenLeftToExecute[i] = status;
            }
            
            if (mainStatus == BehaviorTreeState.Running)
                return BehaviorTreeState.Running;

            // 主任务结束则其余节点也强制结束
            if (stillRunning)
                Abort(flow);

            return mainStatus;
        }
    }
}

#endif