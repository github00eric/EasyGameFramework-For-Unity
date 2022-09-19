/*
 * CodeReflection
 * 代码常用反射工具
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Editor
{
    public static class CodeReflection
    {
        /// <summary>
        /// 获取一个类在所在程序集中的所有子类
        /// </summary>
        /// <param name="parentType"></param>
        /// <returns></returns>
        public static List<Type> GetSubClassTypes(Type parentType)
        {
            var result = new List<Type>();
            var assembly = parentType.Assembly;
            var allTypeInAssembly = assembly.GetTypes();
            foreach (var itemType in allTypeInAssembly)
            {
                var baseType = itemType.BaseType;
                if (baseType != null && baseType == parentType)
                {
                    result.Add(itemType);
                }
            }

            return result;
        }
    }
}
