using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Runtime
{
    
    /// <summary>
    /// Coordinate 是处理 位置点 的坐标系之间的相互转换的工具代码：
    /// <para>世界坐标 World</para>
    /// <para>屏幕坐标 Screen</para>
    /// <para>UI坐标 UI：</para>
    /// 这里不是指 RectTransform 的 PosXYZ，而是 RectTransform.position，和 canvas  有关，可用于3DUI计算。
    /// <para> </para>
    /// viewCamera: 观察空间对象的相机
    /// <para>uiCamera: 观察ui对象的相机</para>
    /// </summary>
    public static class Coordinate
    {
        public static Vector3 World2Screen(Vector3 worldVector3, Camera viewCamera)
        {
            return viewCamera.WorldToScreenPoint(worldVector3);
        }

        /// <summary>
        /// 屏幕坐标系
        /// 转
        /// 世界坐标系
        /// </summary>
        /// <param name="viewCamera"></param>
        /// <param name="screenVector2"></param>
        /// <param name="positionZ"></param>
        /// <returns></returns>
        public static Vector3 Screen2World(Vector2 screenVector2, Camera viewCamera, float positionZ )
        {
            return viewCamera.ScreenToWorldPoint(new Vector3(screenVector2.x, screenVector2.y, positionZ));
        }
        
        /// <summary>
        /// UI坐标系（RectTransform.position)
        /// 转
        /// 屏幕坐标系
        /// </summary>
        /// <param name="uiCamera">ui相机</param>
        /// <param name="uiPosition"></param>
        /// <returns></returns>
        public static Vector2 UI2Screen(Vector3 uiPosition, Camera uiCamera)
        {
            return RectTransformUtility.WorldToScreenPoint(uiCamera, uiPosition);
        }

        /// <summary>
        /// 屏幕坐标系
        /// 转
        /// UI坐标系（RectTransform.position)
        /// <para></para>
        /// </summary>
        /// <param name="uiCamera">ui相机，当 Canvas 的 renderMode 为 RenderMode.ScreenSpaceOverlay 时，ui相机可以传 null</param>
        /// <param name="rt">可以是目标 RectTransform，也可以是目标父级的 RectTransform</param>
        /// <param name="screenVector2"></param>
        /// <returns></returns>
        public static Vector3 Screen2UI(Vector2 screenVector2, Camera uiCamera, RectTransform rt)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenVector2, uiCamera, out Vector3 result);
            return result;
        }

        /// <summary>
        /// 屏幕坐标系
        /// 转
        /// UI局部坐标系（RectTransform.anchoredPosition)
        /// <para></para>
        /// </summary>
        /// <param name="uiCamera">ui相机</param>
        /// <param name="parentRect">需要提供目标父级的 RectTransform</param>
        /// <param name="screenVector2"></param>
        public static Vector2 Screen2UIAnchoredPosition(Vector2 screenVector2, Camera uiCamera, RectTransform parentRect)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenVector2, uiCamera, out Vector2 result);
            return result;
        }

        public static Vector3 UI2World(Vector3 uiPosition, Camera viewCamera, Camera uiCamera, float viewPositionZ)
        {
            var temp = UI2Screen(uiPosition, uiCamera);
            return Screen2World(temp, viewCamera, viewPositionZ);
        }

        public static Vector3 World2UI(Vector3 worldPosition, Camera viewCamera, Camera uiCamera, RectTransform rt)
        {
            var position = World2Screen(worldPosition, viewCamera);
            return Screen2UI(position, uiCamera, rt);
        }

        public static Vector2 World2UIAnchoredPosition(Vector3 worldPosition, Camera viewCamera, Camera uiCamera, RectTransform rt)
        {
            Vector2 position = World2Screen(worldPosition, viewCamera);
            return Screen2UIAnchoredPosition(position, uiCamera, rt);
        }
    }
}
