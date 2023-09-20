using UnityEngine;
using UnityEngine.UI;

namespace Feudal.Scenes.Main
{
    public abstract class WorkHood : UIView
    {
        public (int x, int y) Position { get; set; }

        public abstract void SetLabor(string labor);
    }
}
