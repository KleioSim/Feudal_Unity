using UnityEngine;
using UnityEngine.UI;

public class EstateStaticsItem : UIView
{
    public Text title;
    public Text worker;
    public Text outputValue;
    public Text outputType;

    public Button button;

    public GameObject outputDisableMask;

    public string EstateId { get; internal set; }
    public (int x, int y) Position { get;  set; }
}