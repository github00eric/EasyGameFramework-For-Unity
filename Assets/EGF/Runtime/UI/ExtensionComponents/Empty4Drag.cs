/*
 * 让 UI 可拖拽的组件
 * 需要启用 Raycast Target
 */

using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EGF.Runtime
{
    [AddComponentMenu("EgfUI/Empty4Drag", 10)]
    public class Empty4Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector2 startPos;                           //控件初始位置
        private Vector2 mouseStartPos;                      //鼠标初始位置
        private RectTransform parentRec;                    //控件移动参考Rect
        private RectTransform thisRec;                      //控件自身Rect

        public UnityEvent onEndDrag;

        private void Awake()
        {
            thisRec = GetComponent<RectTransform>();
        }

        private RectTransform GetParentRectTransform()
        {
            if (parentRec == null && transform.parent != null)
                parentRec = transform.parent.GetComponent<RectTransform>();
            
            return parentRec;
        }
        
        //开始拖拽
        public void OnBeginDrag(PointerEventData eventData)
        {
            startPos = thisRec.anchoredPosition;
            
            // 屏幕空间鼠标位置eventData.position 转换为在画布空间的位置
            Camera uiCamera = eventData.pressEventCamera;
            mouseStartPos = Coordinate.Screen2UIAnchoredPosition(eventData.position, uiCamera, GetParentRectTransform());
        }
        //拖拽中
        public void OnDrag(PointerEventData eventData)
        {
            // 屏幕空间鼠标位置eventData.position 转换为在画布空间的位置
            Camera uiCamera = eventData.pressEventCamera;
            Vector2 newMousePos = Coordinate.Screen2UIAnchoredPosition(eventData.position, uiCamera, GetParentRectTransform());
            
            //鼠标移动在画布空间的位置增量
            Vector2 deltaPos = newMousePos - mouseStartPos;
            //原始位置增加位置增量即为现在位置
            thisRec.anchoredPosition = startPos + deltaPos;
        }
        //结束拖拽
        public  void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag?.Invoke();
            Debug.Log("Drag over");
        }    
    }
}
