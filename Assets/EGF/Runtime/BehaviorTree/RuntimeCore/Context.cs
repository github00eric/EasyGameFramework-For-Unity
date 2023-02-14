using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Runtime
{
    public class Context
    {
        public GameObject gameObject;
        public Transform transform;
        
        // Add other game specific systems here
        // ......

        public static Context CreateFromGameObject(GameObject gameObject) {
            // Fetch all commonly used components
            Context context = new Context();
            context.gameObject = gameObject;
            context.transform = gameObject.transform;
            
            // Add whatever else you need here...
            // ......

            return context;
        }
    }
}
