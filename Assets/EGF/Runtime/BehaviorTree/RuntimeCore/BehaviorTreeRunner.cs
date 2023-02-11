using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Runtime
{
    
    // TODO: 后续增加：单步运行调试模式
    public class BehaviorTreeRunner : MonoBehaviour
    {

        // The main behaviour tree asset
        public BehaviorTree tree;

        public bool behaviorEnable;

        // Storage container object to hold game object subsystems
        Context context;

        void Start() {
            context = CreateBehaviourTreeContext();
            tree = BehaviorTree.Clone(tree);
            tree.Bind(context);
        }

        void Update() {
            if (behaviorEnable && tree) {
                tree.Update();
            }
        }

        Context CreateBehaviourTreeContext() {
            return Context.CreateFromGameObject(gameObject);
        }

        private void OnDrawGizmosSelected() {
            if (!tree) {
                return;
            }

            // BehaviorTree.Traverse(tree.rootNode, (n) => {
            //     if (n.drawGizmos) {
            //         n.OnDrawGizmos();
            //     }
            // });
        }
    }
}
