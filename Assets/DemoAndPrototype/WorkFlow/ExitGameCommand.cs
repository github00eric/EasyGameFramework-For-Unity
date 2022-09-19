using EGF.Runtime;
using UnityEngine;

public struct ExitGameCommand : ICommand
{
    public void Execute()
    {
        Debug.Log("收到退出游戏命令！！");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
