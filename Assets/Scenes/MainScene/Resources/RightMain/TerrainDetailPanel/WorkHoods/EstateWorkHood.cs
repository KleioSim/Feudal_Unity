using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Feudal.Scenes.Main
{
    public class EstateWorkHood : WorkHood
    {
        public Text title;
        public Text productType;
        public Text productValue;

        public GameObject disableMask;

        public string estateId { get; set; }

        public override void SetLabor(string labor)
        {
            ExecUICmd.Invoke(new EstateStartWorkCommand(labor, estateId, Position));
        }
    }
}