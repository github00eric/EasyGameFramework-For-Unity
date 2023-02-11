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
        
        [Output(ShowBackingValue.Never,ConnectionType.Multiple,TypeConstraint.Strict,true)]
        public List<Connect> output = new List<Connect>();
    }
}