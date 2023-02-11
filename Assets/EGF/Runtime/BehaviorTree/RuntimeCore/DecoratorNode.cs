using JCMG.Nodey;
using NaughtyAttributes;
using UnityEngine;

namespace EGF.Runtime
{
    [NodeTint(0.4f,0.25f,0.05f)]
    public abstract class DecoratorNode : BTNode
    {
        public const string CreatePath = "BehaviorTree/Decorator/";
        
        [Header("Decorator")]
        [Input(ShowBackingValue.Never,ConnectionType.Override)]
        public Connect input;
        
        [Output(ShowBackingValue.Never,ConnectionType.Override)]
        public Connect output;
    }
}