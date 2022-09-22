using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EGF.Runtime
{
    public class Navigation2View : MonoBehaviour
    {
        private Button button;
        public string target;
        public NavigationType navigationType;
        
        public enum NavigationType
        {
            Show,
            ShowAdd,
        }
        
        void Start()
        {
            button = GetComponent<Button>();
            if (!button)
            {
                Logcat.Error(this,"Get No Button.");
                return;
            }
            
            button.onClick.AddListener(Navigation);
            
        }

        void Navigation()
        {
            switch (navigationType)
            {
                case NavigationType.Show:
                    EgfEntry.GetModule<IUIManager>()?.Show(target);
                    break;
                case NavigationType.ShowAdd:
                    EgfEntry.GetModule<IUIManager>()?.ShowAdditive(target);
                    break;
            }
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(Navigation);
        }
    }
}
