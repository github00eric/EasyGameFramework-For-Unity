using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Editor
{
    public abstract class BaseEgfUtilityWindow
    {
        protected BaseEgfUtilityWindow()
        {
            
        }

        public abstract void OnWindowCreated();
        protected virtual void OnSelectionChange()
        {
        
        }
    }
}
