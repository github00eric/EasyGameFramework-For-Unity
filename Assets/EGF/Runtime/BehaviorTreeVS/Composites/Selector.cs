#if VISUAL_SCRIPTING_ENABLE

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * 选择器会依次执行多个节点
 * 失败会继续下一个节点，直到有节点成功
 */
namespace EGF.Runtime.Behavior
{
    public class Selector : CompositeNode
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
            for (int i = _current; i < BranchCount; ++i)
            {
                _current = i;
                var state = InvokeNextNode(flow, i);

                switch (state) {
                    case BehaviorTreeState.Running:
                        return BehaviorTreeState.Running;
                    case BehaviorTreeState.Success:
                        return BehaviorTreeState.Success;
                    case BehaviorTreeState.Failure:
                        continue;
                }
            }

            return BehaviorTreeState.Failure;
        }
    }
}

#endif