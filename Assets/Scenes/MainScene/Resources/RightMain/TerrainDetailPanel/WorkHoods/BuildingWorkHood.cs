using System;
using UnityEngine;
using UnityEngine.UI;

namespace Feudal.Scenes.Main
{
    public class BuildingWorkHood : WorkHood
    {
        public Text title;
        public Slider percent;

        public GameObject disableMask;

        public Enum estateType { get; set; }

        public override void SetLabor(string labor)
        {
            ExecUICmd.Invoke(new EstateBuildStartCommand(labor, estateType, Position));
        }
    }
}

