/*
 * 可以替代透明图片，让组件能接受UI点击等事件
 */
using UnityEngine;
using UnityEngine.UI;

namespace EGF.Runtime
{
    [RequireComponent(typeof(CanvasRenderer))]
    [AddComponentMenu("EgfUI/Empty4Raycast", 10)]
    public class Empty4Raycast : MaskableGraphic
    {
        protected Empty4Raycast()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}
