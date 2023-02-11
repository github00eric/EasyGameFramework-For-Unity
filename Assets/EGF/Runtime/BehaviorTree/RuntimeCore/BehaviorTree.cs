using System;
using System.Collections.Generic;
using JCMG.Nodey;
using NaughtyAttributes;
using UnityEngine;

namespace EGF.Runtime
{
    [CreateAssetMenu]
    public class BehaviorTree : NodeGraph
    {
        public RootNode rootNode;
        public BTNode.State treeState = BTNode.State.Running;
        
        private Blackboard blackboard = new Blackboard();
        
        public BTNode.State Update()
        {
            if (!rootNode)
                return BTNode.State.Failure;
            
            if (rootNode.state == BTNode.State.Running) {
                treeState = rootNode.Update();
            }
            return treeState;
        }
        
        public static List<BTNode> GetChildren(BTNode parent) {
            // List<BTNode> children = new List<BTNode>();
            //
            // switch (parent)
            // {
            //     case DecoratorNode decorator when decorator.child != null:
            //         children.Add(decorator.child);
            //         break;
            //     
            //     case RootNode rootNode when rootNode.child != null:
            //         children.Add(rootNode.child);
            //         break;
            //     
            //     case CompositeNode composite:
            //         return composite.children;
            // }
            //
            // return children;

            return parent.GetChildren();
        }
        
        public static void Traverse(BTNode node, Action<BTNode> visiter)
        {
            if (!node) return;
            
            visiter.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visiter));
        }
        
        public static BehaviorTree Clone(BehaviorTree prototype) {
            BehaviorTree tree = Instantiate(prototype);
            foreach (var node in tree.nodes)
            {
                if (node is RootNode root)
                    tree.rootNode = root;
            }
            return tree;
        }
        
        public void Bind(Context context) {
            Traverse(rootNode, node => {
                node.context = context;
                node.blackboard = blackboard;
            });
        }
        
    }
}
