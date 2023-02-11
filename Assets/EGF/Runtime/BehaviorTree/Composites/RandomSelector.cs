using JCMG.Nodey;
using UnityEngine;

namespace EGF.Runtime
{
    [CreateNodeMenu(CreatePath + "RandomSelector")]
    public class RandomSelector : CompositeNode
    {
        private int current;

        protected override void OnStart() {
            current = Random.Range(0, Children.Count);
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            var child = Children[current];
            return child.Update();
        }
    }
}