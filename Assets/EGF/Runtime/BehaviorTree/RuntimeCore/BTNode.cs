using System;
using System.Collections;
using System.Collections.Generic;
using JCMG.Nodey;
using NaughtyAttributes;
using UnityEngine;

namespace EGF.Runtime
{
    public abstract class BTNode : Node
    {
        public enum State {
            Running,
            Failure,
            Success
        }
        
        [Serializable]
        public class Connect
        {
        }

        public State state = State.Running;
        [HideInInspector] public bool started = false;
        public Blackboard blackboard;
        public bool drawGizmos = false;
        
        // [HideInInspector] public string guid = System.Guid.NewGuid().ToString();
        // [HideInInspector] public Vector2 position;

        protected List<BTNode> children = new List<BTNode>();

        public Context context;
        
        public override object GetValue(NodePort port)
        {
            return null;
        }
        
        public State Update() {

            if (!started) {
                OnStart();
                started = true;
            }

            state = OnUpdate();

            if (state != State.Running) {
                OnStop();
                started = false;
            }

            return state;
        }

        public void Abort() {
            BehaviorTree.Traverse(this, (node) => {
                node.started = false;
                node.state = State.Running;
                node.OnStop();
            });
        }

        // 行为树
        public virtual List<BTNode> GetChildren()
        {
            children.Clear();
            
            var outputPort = GetOutputPort("output");
            
            if (outputPort == null)
            {
                return children;
            }
            
            var childrenPort = outputPort.GetConnections();
            foreach (var port in childrenPort)
            {
                if (port.IsConnected)
                {
                    children.Add(port.node as BTNode);
                }
            }

            if(children.Count>1)
                children.Sort(CompareNodePriority);
            
            return children;
        }

        // 比较运行权重，位置更上的优先运行
        int CompareNodePriority(BTNode node1, BTNode node2)
        {
            if (!node1 && !node2)
                return 0;
            else if (!node1)
                return -1;
            else if (!node2)
                return 1;

            var value1 = node1.position.y;
            var value2 = node2.position.y;

            return value1.CompareTo(value2);
        }

        public virtual void OnDrawGizmos() { }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
    }
}
