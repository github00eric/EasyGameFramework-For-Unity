using UnityEngine;

namespace EGF.Runtime
{
    public static class MathTool
    {
        /// <summary>
        /// 浮点数转换定点数，会损失一些精度
        /// </summary>
        /// <param name="inNumber"></param>
        /// <param name="fig">保留的小数位</param>
        /// <returns></returns>
        public static float GetFixedNumber(float inNumber, int fig)
        {
            var fig10 = Mathf.Pow(10, fig);
            float result = Mathf.Round(inNumber * fig10) / fig10;
            return result;
        }
    }
}