/*
 * 本文件用于演示：
 * 扩展和添加自定义 Unity Visual Scripting 类型
 *
 * 参考资料：
 * > https://docs.unity.cn/Packages/com.unity.visualscripting@1.7/manual/vs-custom-types.html
 * > https://docs.unity.cn/Packages/com.unity.visualscripting@1.7/manual/vs-add-remove-type-options.html
 * 
 * 打开菜单： Edit -> Project Setting -> Visual Scripting
 * HACK：新增自定义类型时，需要先在 Node Library 添加所在的程序集，然后在 Type Options 中添加自定义类型
 * 最后使用 Regenerate Nodes 重新生成节点
 *
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 使用 [Inspectable] 可以让自定义类型在 Unity Inspector窗口和 Visual Scripting Graph Inspector窗口中正常显示
[Serializable, Inspectable]
public class ExampleCustomType
{
    [Inspectable] public string name;
    [Inspectable] public Color color;
    [Inspectable] public int layer;
}
