using System;
using System.Collections.Generic;
using System.Diagnostics;
using JCMG.Nodey;
using NaughtyAttributes;
using UnityEngine;

namespace EGF.Runtime
{
    [CreateNodeMenu("BehaviorTree/Root")][NodeWidth(500)][NodeTint(0.5f,0.1f,0.2f)]
    public class RootNode : BTNode
    {
        [Header("Root")]
        [Output(ShowBackingValue.Never,ConnectionType.Override)]
        public Connect output;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            DataVisualize();
            return Children[0].Update();
        }

        // ----------------------------------------------------------------------------------------------------
        #region BlackboardPreview | 黑板数据可视化
#if UNITY_EDITOR
        public List<BbKeyValuePair> blackboardDataPreview;
        
        [Serializable]
        public struct BbKeyValuePair
        {
            public string key;
            public BlackboardData value;
        }

        [Conditional("UNITY_EDITOR")]
        private void DataVisualize()
        {
            // blackboardDataPreview = ....
        }
#endif
        #endregion
    }
}