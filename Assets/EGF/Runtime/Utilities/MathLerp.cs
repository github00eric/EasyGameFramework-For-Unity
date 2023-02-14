using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Runtime
{
    public static class MathLerp
    {
        /// <summary>
        /// 渐近过渡 - 浮点数
        /// </summary>
        public static float StepLerp(float start, float end, float step)
        {
            var delta = end - start;
            if (Mathf.Abs(delta) > step)
            {
                return delta > 0 ? start + step : start - step;
            }
            else
            {
                return end;
            }
        }
        
        /// <summary>
        /// 渐近过渡 - Vector3
        /// </summary>
        public static Vector3 StepLerp(Vector3 start, Vector3 end, float step)
        {
            // 过渡到目标速度
            if (Vector3.Distance(start, end) > 0.1f)
            {
                var acc = (end - start).normalized * step;
                start = start + acc;
            }
            else
            {
                start = end;
            }

            return start;
        }
        
        /// <summary>
        /// 限制角度 (-180 到 180) 然后限制在 (min 到 max)
        /// </summary>
        public static float ClampAngle(float lfAngle, float min, float max)
        {
            if (lfAngle < -180f) lfAngle += 360f;
            if (lfAngle > 180f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, min, max);
        }
        
        /// <summary>
        /// 限制角度 (-180 到 180)
        /// </summary>
        public static float ClampAngle(float lfAngle)
        {
            if (lfAngle < -180f) lfAngle += 360f;
            if (lfAngle > 180f) lfAngle -= 360f;
            return lfAngle;
        }
    }
}
