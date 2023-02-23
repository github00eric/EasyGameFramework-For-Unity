using System.Collections.Generic;
using JCMG.Nodey;
using UnityEngine;

namespace EGF.Runtime
{
    /*
     * 简易平行节点 [SimpleParallel] 仿 UE5
     * 新增了 mainAction -主任务节点, 固定作为 children[0]
     * 主任务节点 mainAction 执行期间，同时执行其余节点
     * 
     * 其余节点执行完毕会等待主任务执行完毕
     * 主任务执行完毕会取消所有未完成的其余任务
     *
     * 可以用作行为树的反应式节点，（例如，在执行行为 A 时，能对行为 B 做出立即反应）
     * 在 mainAction 执行期间持续检测反应条件，需要做出反应时立即结束 mainAction 的执行，跳转到 SimpleParallel 外的节点
     */
    [CreateNodeMenu(CreatePath + "Simple Parallel")]
    public class SimpleParallel : Parallel
    {
        [Output(ShowBackingValue.Never,ConnectionType.Override)]
        public Connect mainAction;
        /*
         * 在 SimpleParallel 中，
         * 新增了 mainAction -主任务节点, 固定作为 children[0]
         * childrenLeftToExecute[0]~[1] 储存剩余 children[1]~[i+1] 的节点运行状态
         */
        protected override void OnStart()
        {
            childrenLeftToExecute.Clear();
            for (int i = 1; i < children.Count; i++)
            {
                childrenLeftToExecute.Add(State.Running);
            }
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            // 主任务
            var mainStatus = children[0].Update();
            
            // 其余任务
            bool stillRunning = false;
            for (int i = 0; i < childrenLeftToExecute.Count; ++i) {
                if (childrenLeftToExecute[i] == State.Running) {
                    var status = children[i+1].Update();
                    if (status == State.Running) {
                        stillRunning = true;
                    }
                    childrenLeftToExecute[i] = status;
                }
            }
            
            if (mainStatus == State.Running)
                return State.Running;

            // 主任务结束则其余节点也强制结束
            if (stillRunning)
                AbortRunningLeftChildren();

            return mainStatus;
        }

        public override List<BTNode> GetChildren()
        {
            var temp = base.GetChildren();
            if (TryGetPortNodes("mainAction", out var node) && node.Count > 0)
            {
                temp.Insert(0,node[0]);
            }

            children = temp;
            return children;
        }

        void AbortRunningLeftChildren()
        {
            for (int i = 0; i < childrenLeftToExecute.Count; ++i) {
                if (childrenLeftToExecute[i] == State.Running) {
                    children[i+1].Abort();
                }
            }
        }
    }
}