using JCMG.Nodey;
using UnityEngine;

namespace EGF.Runtime
{
    /*
     * 选择器会依次执行多个节点
     * 失败会继续下一个节点，直到有节点成功
     */
    [CreateNodeMenu(CreatePath + "Selector")]
    public class Selector : CompositeNode
    {
        protected int current;

        protected override void OnStart() {
            current = 0;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            for (int i = current; i < children.Count; ++i) {
                current = i;
                var child = children[current];

                switch (child.Update()) {
                    case State.Running:
                        return State.Running;
                    case State.Success:
                        return State.Success;
                    case State.Failure:
                        continue;
                }
            }

            return State.Failure;
        }
    }
}