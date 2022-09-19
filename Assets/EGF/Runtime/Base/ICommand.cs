using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Runtime
{
    /// <summary>
    /// 命令是对一系列数据和操作的封装。
    /// </summary>
    public interface ICommand
    {
        void Execute();
    }
}
