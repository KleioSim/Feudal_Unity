using UnityEngine;
using UnityEngine.UI;

namespace Feudal.Scenes.Main
{
    public class DisoverWorkHood : WorkHood
    {
        public Text title;
        public Slider percent;

        public override void SetLabor(string labor)
        {
            ExecUICmd.Invoke(new DiscoverCommand(labor, Position));
        }
    }
}
