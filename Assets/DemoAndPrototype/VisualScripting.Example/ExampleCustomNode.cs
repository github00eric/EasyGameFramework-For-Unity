/*
 * 本文件用于演示：
 * 扩展和自定义 Unity Visual Scripting 节点
 *
 * 参考资料：
 * > https://zhuanlan.zhihu.com/p/588386637
 *
 * HACK：每次新增或移除自定义节点后，需要重新生成节点库
 * 使用菜单： Edit -> Project Setting -> Visual Scripting -> Regenerate Nodes 重新生成节点
 *
 * 
 */
using Unity.VisualScripting;

// 自定义展示标题
[UnitTitle("Add Value Example")]
// 创建时，节点将展示在搜索栏的 Example -> CustomNode 目录下
[UnitCategory("Example/CustomNode")]
// 使用 [TypeIcon] 可以设定节点的 Icon，不过不能使用自定义 Icon，只能从 Unity 已有的图标选择，
// 基本支持所有UnityEngine类型，可以从创建节点窗口预览不同类型的图标
[TypeIcon(typeof(Add<int>))]
public class ExampleCustomNode : Unit
{
    // DoNotSerialize 这个 Attribute 应该修饰在每个端口上，因为端口并不需要序列化，这是官方提倡的
    #region 端口定义
    
    // 控制流输入输出端口。
    // 在实际执行时，流会从 ControlInputPort 流入，并执行节点内部定义的函数。在执行结束后，从节点逻辑指定的 ControlOutputPort 流出。
    // 如果节点有一个 ControlInputPort，则至少需要有一个 ControlOutputPort。
    [DoNotSerialize] public ControlInput inputTrigger;
    [DoNotSerialize] public ControlOutput outputTrigger;
    
    // 值输入输出端口
    [DoNotSerialize] public ValueInput inputValue1;
    [DoNotSerialize] public ValueInput inputValue2;
    [DoNotSerialize] public ValueOutput outputValue;
    
    #endregion
    
    private float _result; // 用来保存计算的结果
    
    // 创建时执行 Definition
    // 用于节点各个端口的定义
    protected override void Definition()
    {
        
        // 这里，第一个参数是该输入端口的名字，也可以称为 key
        // ControlInput等函数会对第一个参数进行校验，校验要求同类型的 key不能重复，否则会报错
        // 例如，如果该节点有两个输入端口都叫 input，或有两个输出端口都叫 output，这是不满足校验需求的
        // 但如果一个输入端口和一个输出端口都叫 value，这是可以接受的
        // 这里的第二个参数需要传一个函数，可以使用 lambda 函数，也可以自己声明一个函数；要求必须具有一个 Flow 类型的输入参数，并返回一个 ControlOutput 类型结果
        inputTrigger = ControlInput("InputTrigger", flow =>
        {
            // 在流执行的过程中想要获取该节点的其他值输入端口的值，需要调用GetValue函数，并传入对应的值输入端口作为参数
            _result = flow.GetValue<float>(inputValue1) + flow.GetValue<float>(inputValue2);
            return outputTrigger;
        });
        outputTrigger = ControlOutput("OutputTrigger");
        
        inputValue1 = ValueInput<float>("InputValue1");
        inputValue2 = ValueInput(typeof(float), "InputValue2");
        // 上述两种写法都是可以的，但我更推荐第一种，因为可以方便地指定默认值，就像这样：
        // InputValue1 = ValueInput<float>("InputValue1",2.0f);
        // 使用后一种当然也可以指定默认值，但只支持字符串形式的传参，会在内部进行额外的类型转换，性能和可读性都不佳，且不能处理比较复杂的类型，不推荐用
        
        // 和 ControlInput 函数类似，ValueOutput 函数的第二个参数也需要传入一个函数，需要具有一个 Flow类型的参数，并返回该输出端口对应的结果
        // 需要注意的是，事实上，计算的过程不一定要放在 ControlInput 的函数内，写在 OutputValue 内也是同样可行的
        // 但由于下游节点每次获取该端口的值时都要执行一次计算的函数才能获取到值，因此仅适用于一些逻辑和运算简单且不会对源数据造成额外更改的函数
        outputValue = ValueOutput<float>("OutputValue", flow => _result);
    }
}
