using System;
using System.Collections.Generic;
using System.Diagnostics;
using JCMG.Nodey;
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
            return children[0].Update();
        }

        // ----------------------------------------------------------------------------------------------------
        #region BlackboardPreview | 黑板数据可视化
#if UNITY_EDITOR
        public List<BbKeyObjectPair> blackboardDataPreview;

        [Serializable]
        public struct BbKeyObjectPair
        {
            public string key;
            public string value;
        }
        

        [Conditional("UNITY_EDITOR")]
        private void DataVisualize()
        {
            if (blackboardDataPreview == null)
                blackboardDataPreview = new List<BbKeyObjectPair>();
            
            blackboardDataPreview.Clear();
            foreach (var element in blackboard.BlackboardDataList)
            {
                var dataString = element.Value.DataString;
                BbKeyObjectPair data = new BbKeyObjectPair() {key = element.Key, value = dataString};
                blackboardDataPreview.Add(data);
            }
        }
#endif
        #endregion
    }
}