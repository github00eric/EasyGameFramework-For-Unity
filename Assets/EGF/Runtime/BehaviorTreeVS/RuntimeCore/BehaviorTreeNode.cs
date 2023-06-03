#if VISUAL_SCRIPTING_ENABLE

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace EGF.Runtime.Behavior
{
    public abstract class BehaviorTreeNode : Unit
    {
        [Serializable]
        public enum State {
            Running,
            Failure,
            Success
        }
        
        private State _state = State.Running;
        private bool _started;

        [DoNotSerialize][Inspectable,InspectorLabel("Current State"),UnitHeaderInspectable()]
        public State currentState => _state;
        // protected 

        [DoNotSerialize] public ControlInput tick;

        // [DoNotSerialize] public ValueInput cacheBlackboard;
        [DoNotSerialize] public ValueOutput stateDataOutput;
        
        // public abstract List<BehaviorTreeNode> GetChildren();
        // public abstract void Abort();
        
        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnRunning(Flow flow);
        
        
        protected override void Definition()
        {
            tick = ControlInput(nameof(tick), Tick);
            
            stateDataOutput = ValueOutput<State>("state", flow => _state);
        }

        private ControlOutput Tick(Flow flow)
        {
            if (currentState == State.Running)
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
            if (_state != State.Running) {
                OnStop();
                _started = false;
            }
        }
    }
}

#endif
