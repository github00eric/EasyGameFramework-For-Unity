using System.Collections;
using System.Collections.Generic;
using JCMG.Nodey;
using UnityEngine;

namespace EGF.Runtime
{
    /*
     * 成功装饰器
     * 子节点运行完成时一定返回成功
     * 用于某些场合下，运行失败会打断平行节点等其它节点的继续运行，使用成功装饰器可以避免这个状况
     */
    [CreateNodeMenu(CreatePath + "Success")]
    public class Success : DecoratorNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            switch (children[0].Update())
            {
                case State.Running:
                    return State.Running;
            }
            return State.Success;
        }
    }
}
