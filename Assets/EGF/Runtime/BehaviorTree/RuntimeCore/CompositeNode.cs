using System.Collections.Generic;
using JCMG.Nodey;
using NaughtyAttributes;
using UnityEngine;

namespace EGF.Runtime
{
    [NodeTint(0.3f,0.4f,0)]
    public abstract class CompositeNode : BTNode
    {
        public const string CreatePath = "BehaviorTree/Composite/";
        
        [Header("Composite")]
        [Space]
        [Input(ShowBackingValue.Never,ConnectionType.Override)]
        public Connect input;
        
        [Output(ShowBackingValue.Never,ConnectionType.Override,TypeConstraint.None,true)]
        // [Output()]
        public List<Connect> output = new List<Connect>();

        // public override List<BTNode> GetChildren()
        // {
        //     children.Clear();
        //     
        //     // var outputPorts = GetOutputPort("output");
        //
        //     foreach (var port in DynamicOutputs)
        //     {
        //         if (port.IsConnected)
        //         {
        //             children.Add(port.node as BTNode);
        //         }
        //     }
        //
        //     return children;
        // }
    }
}