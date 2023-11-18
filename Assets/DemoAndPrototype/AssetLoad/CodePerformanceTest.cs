using System.Diagnostics;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace EGF.Runtime
{
    public class CodePerformanceTest : MonoBehaviour
    {
        public int testCount = 10000000;

        public UnityEvent uEvent;
        [Button()]
        private void Test()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < testCount; i++)
            {
                uEvent?.Invoke();
            }
            
            sw.Stop();
            Debug.Log($"Test uEvent {testCount} times, cost {sw.ElapsedMilliseconds} ms");
        }
    }
}
