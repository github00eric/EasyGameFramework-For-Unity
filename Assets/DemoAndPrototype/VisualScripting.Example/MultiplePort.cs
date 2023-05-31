/*
 * 测试创建多个控制输出端口
 * 参考 Library/PackageCache/com.unity.visualscripting@1.7.6/Runtime/VisualScripting.Flow/Framework/Control/Sequence.cs
 */
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Example/CustomNode")]
public class MultiplePort : Unit
{
    [SerializeAs(nameof(OutputCount))]
    private int _outputCount = 2;
    
    [DoNotSerialize][Inspectable, InspectorLabel("Branch Count"), UnitHeaderInspectable("Branch")]
    public int OutputCount
    {
        get => _outputCount;
        set => _outputCount = value;
    }

    [DoNotSerialize][PortLabelHidden] public ControlInput parentNode;

    [DoNotSerialize] public ReadOnlyCollection<ControlOutput> Outputs { get; private set; }
    // [DoNotSerialize] public List<ControlOutput> Outputs { get; private set; }        // 可以直接用 List<>


    protected override void Definition()
    {
        parentNode = ControlInput("parent", Enter);

        var multiOutputs = new List<ControlOutput>();

        for (int i = 0; i < OutputCount; i++)
        {
            var output = ControlOutput(i.ToString());
            
            multiOutputs.Add(output);
        }
        
        Outputs = multiOutputs.AsReadOnly();
        // Outputs = multiOutputs;
    }

    private ControlOutput Enter(Flow flow)
    {
        var stack = flow.PreserveStack();
        
        foreach (var output in Outputs)
        {
            flow.Invoke(output);
        
            flow.RestoreStack(stack);
        }
        
        flow.DisposePreservedStack(stack);
        
        return null;
    }
}
