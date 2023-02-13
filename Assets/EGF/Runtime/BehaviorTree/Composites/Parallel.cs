using System.Collections.Generic;
using System.Linq;
using JCMG.Nodey;
using UnityEngine;

namespace EGF.Runtime
{
    /*
     * 平行节点 [Parallel]
     * 同时执行所有子节点
     * 任意节点失败将全部停止并返回失败
     * 所有节点运行成功则停止并返回成功
     */
    [CreateNodeMenu(CreatePath + "Parallel")]
    public class Parallel : CompositeNode
    {
        protected readonly List<State> childrenLeftToExecute = new List<State>();

        protected override void OnStart() {
            childrenLeftToExecute.Clear();
            children.ForEach(a => {
                childrenLeftToExecute.Add(State.Running);
            });
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            bool stillRunning = false;
            for (int i = 0; i < childrenLeftToExecute.Count; ++i) {
                if (childrenLeftToExecute[i] == State.Running) {
                    var status = children[i].Update();
                    if (status == State.Failure) {
                        AbortRunningChildren();
                        return State.Failure;
                    }

                    if (status == State.Running) {
                        stillRunning = true;
                    }

                    childrenLeftToExecute[i] = status;
                }
            }

            return stillRunning ? State.Running : State.Success;
        }

        void AbortRunningChildren() {
            for (int i = 0; i < childrenLeftToExecute.Count; ++i) {
                if (childrenLeftToExecute[i] == State.Running) {
                    children[i].Abort();
                }
            }
        }
    }
}