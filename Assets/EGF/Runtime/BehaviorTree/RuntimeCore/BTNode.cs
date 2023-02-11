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

        private List<BTNode> children = new List<BTNode>();
        protected List<BTNode> Children => children;

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
        public List<BTNode> GetChildren()
        {
            children.Clear();
            
            var outputPort = GetOutputPort("output");
            if (outputPort == null) return children;
            
            var childrenPort = outputPort.GetConnections();

            foreach (var port in childrenPort)
            {
                if (port.IsConnected)
                {
                    children.Add(port.node as BTNode);
                }
            }

            return children;
        }

        public virtual void OnDrawGizmos() { }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
    }
}
