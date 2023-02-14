using JCMG.Nodey;
using NaughtyAttributes;
using UnityEngine;

namespace EGF.Runtime
{
    [NodeTint(0.15f,0.2f,0.5f)]
    public abstract class ActionNode : BTNode
    {
        public const string CreatePath = "BehaviorTree/Action/";
        
        [Header("Action")] 
        [Input(ShowBackingValue.Never,ConnectionType.Override)]
        public Connect input;
    }
}