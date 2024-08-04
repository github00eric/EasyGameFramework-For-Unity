# EasyGameFramework

> ## 注意 
>
> 该仓库暂停维护，我目前正在制作一款关于少女前线的TPS同人游戏，链接[点这里](https://blogcn.standby4xeee.top/projects/Project_GF_TPS)，我会在游戏制作过程中根据使用感受逐步改进 EasyGameFramework，并在游戏结束后再优化目前的EGF。

## 介绍

用于快速原型游戏制作的游戏引擎中间代码框架。开发策略是易用度优先。不支持ECS下使用

- 包含功能：
- 解耦：
    - IOC容器 EGFEntry
    - 事件系统
    - 命令
    - 绑定数据
- 工具：
    - 日志工具
    - 对象池
    - 常用坐标转换
    - 数值过渡
    - 动画扩展代码，能指定状态机直接跳转，且可异步等待，极大简化动画状态机
    - （Editor）代码修改项目宏定义
    - （开源）JCMG Nodey，一个泛用可视化图形节点框架
    - （开源）Naughty Attributes, 将 unity 的序列化对象在 Inspector 的展示优化。
    - （开源）UniTask，Unity异步编程支持
- 模块：
    - 音频播放模块
    - 资源加载模块
    - UI基本框架
    - 行为树运行组件和常见行为树节点
- 模板基类：
    - 单例模板
    - UI页面模板
    
## 安装

Unity 的 Package Manager 支持安装 Git url 包的版本 (Unity >= 2019.3.4f1, Unity >= 2020.1a21). 可以使用 https://github.com/github00eric/EasyGameFramework-For-Unity.git?path=Assets/EGF 安装到 Package Manager。

或者可以克隆该仓库，复制 ./Assets/EGF 路径的文件夹到工程中完成安装。

## **依赖库：**
- Addressables

## 使用说明
- 当前仅供学习参考用途，还处在早期版本，未经实际项目考验。
- ~~打开 "EGF Utility -> Open Utilities" 菜单，可以查看 EGF 的一些简要使用说明，并使用 EGF 的编辑器工具~~ \> Odin插件已移除，后续会新增新的说明和教程方式
