using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace EGF.Runtime
{
    public class Blackboard
    {
        private Dictionary<string, BlackboardData> blackboardData;

        private BlackboardData GetData(string key)
        {
            if (blackboardData.ContainsKey(key))
                return blackboardData[key];

            return null;
        }
        
        public T Get<T>(string key) where T: IEquatable<T>
        {
            BlackboardData result = GetData(key);
            if (result is BlackboardDataType<T> type)
                return type.value;
            
            return default;
        }
        
        public void Set<T>(string key, T value) where T: notnull, IEquatable<T>
        {
            if (!blackboardData.ContainsKey(key))
            {
                // 黑板中未注册key为{key}的键值对
                BlackboardDataType<T> adding = new BlackboardDataType<T>(value);
                blackboardData.Add(key, adding);
                return;
            }
            
            BlackboardDataType<T> data = blackboardData[key] as BlackboardDataType<T>;
            // 有 Key 没 BlackboardDataType<T> 
            if (data == null || data.value.Equals(value))
            {
                return;
            }
            
            data.value = value;
        }

        public Object GetObject(string key)
        {
            BlackboardData result = GetData(key);
            if (result is BlackboardReferenceData type)
                return type.value;
            
            return default;
        }
        
        public void SetObject(string key, Object value)
        {
            if (!blackboardData.ContainsKey(key))
            {
                // 黑板中未注册key为{key}的键值对
                BlackboardReferenceData adding = new BlackboardReferenceData(value);
                blackboardData.Add(key, adding);
                return;
            }
            
            BlackboardReferenceData data = blackboardData[key] as BlackboardReferenceData;
            // 有 Key 没 BlackboardReferenceData 那就是不支持
            if (data == null || data.Equals(value))
            {
                return;
            }
            
            data.value = value;
        }
        
    }

    // 定义以下代码，通过提前一次性装箱，来大幅减少重复装箱、拆箱
    // 增加编译量、减少重复装箱、拆箱引发的 GC
    // https://www.lfzxb.top/blackboard-0gc-write/
    // ----------------------------------------------------------------------------------------------------

    #region BlackboardData

    public abstract class BlackboardData
    {
    }

    public class BlackboardDataType<T>: BlackboardData where T: IEquatable<T>
    {
        public T value;
        public BlackboardDataType(T value)
        {
            this.value = value;
        }
    }

    public class BlackboardReferenceData: BlackboardData, IEquatable<BlackboardReferenceData>, IEquatable<Object>
    {
        public Object value;
    
        public BlackboardReferenceData(Object obj)
        {
            value = obj;
        }
    
        public bool Equals(BlackboardReferenceData other)
        {
            if (other == null || other.value == null)
            {
                return value == null;
            }
            
            var id1 = value.GetInstanceID();
            var id2 = other.value.GetInstanceID();
            return id1 == id2;
        }
    
        public bool Equals(Object other)
        {
            if (other == null)
            {
                return value == null;
            }
            
            var id1 = value.GetInstanceID();
            var id2 = other.GetInstanceID();
            return id1 == id2;
        }
    }

    #endregion
    
    
}
