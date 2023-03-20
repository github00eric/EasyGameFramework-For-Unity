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
        
        public readonly Blackboard blackboard = new Blackboard();
        
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
            return parent.GetChildren();
        }
        
        public static void Traverse(BTNode node, Action<BTNode> traverseAction)
        {
            if (!node) return;
            
            traverseAction.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, traverseAction));
        }
        
        public static BehaviorTree Clone(BehaviorTree prototype) {
            BehaviorTree tree = prototype.Copy() as BehaviorTree;
            if (!tree) return null;
            
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
