#if VISUAL_SCRIPTING_ENABLE

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * 平行节点 [Parallel]
 * 同时执行所有子节点
 * 任意节点失败将全部停止并返回失败
 * 所有节点运行成功则停止并返回成功
 */
namespace EGF.Runtime.Behavior
{
    public class Parallel : CompositeNode
    {
        protected readonly List<BehaviorTreeState> childrenLeftToExecute = new List<BehaviorTreeState>();
        
        protected override void OnStart()
        {
            childrenLeftToExecute.Clear();
            for (int i = 0; i < BranchCount; i++)
            {
                childrenLeftToExecute.Add(BehaviorTreeState.Running);
            }
        }

        protected override void OnStop()
        {
        }

        protected override BehaviorTreeState OnRunning(Flow flow)
        {
            bool stillRunning = false;
            for (int i = 0; i < BranchCount; ++i)
            {
                if (childrenLeftToExecute[i] == BehaviorTreeState.Running)
                {
                    var status = InvokeNextNode(flow, i);
                    if (status == BehaviorTreeState.Failure) {
                        Abort(flow);
                        return BehaviorTreeState.Failure;
                    }

                    if (status == BehaviorTreeState.Running) {
                        stillRunning = true;
                    }

                    childrenLeftToExecute[i] = status;
                }
            }

            return stillRunning ? BehaviorTreeState.Running : BehaviorTreeState.Success;
        }
    }
}

#endif