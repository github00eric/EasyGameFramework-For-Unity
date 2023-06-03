#if VISUAL_SCRIPTING_ENABLE

using System;
using Unity.VisualScripting;

namespace EGF.Runtime.Behavior
{
    
    [Serializable]
    public enum BehaviorTreeState {
        Running,
        Failure,
        Success
    }
    
    public abstract class BehaviorTreeNode : Unit
    {
        
        private BehaviorTreeState _state = BehaviorTreeState.Running;
        private bool _started;

        [DoNotSerialize][Inspectable,InspectorLabel("Current State"),UnitHeaderInspectable()]
        public BehaviorTreeState currentState => _state;
        // protected 

        [DoNotSerialize] public ControlInput tick;

        // [DoNotSerialize] public ValueInput cacheBlackboard;
        [DoNotSerialize] public ValueOutput stateDataOutput;
        
        // public abstract List<BehaviorTreeNode> GetChildren();
        // public abstract void Abort();
        
        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract BehaviorTreeState OnRunning(Flow flow);
        
        
        protected override void Definition()
        {
            tick = ControlInput(nameof(tick), Tick);
            
            stateDataOutput = ValueOutput<BehaviorTreeState>("state", flow => _state);
        }

        private ControlOutput Tick(Flow flow)
        {
            if (currentState == BehaviorTreeState.Running)
                TickInternal(flow);
            return null;
        }
        
        private void TickInternal(Flow flow) {
            // start
            if (!_started) {
                OnStart();
                _started = true;
            }

            // running
            _state = OnRunning(flow);

            // stop
            if (_state != BehaviorTreeState.Running) {
                OnStop();
                _started = false;
            }
        }
    }
}

#endif
