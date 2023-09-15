using UnityEngine;
using UnityEngine.UI;

public class LaborWorkDetail : UIView2
{
    public GameObject laborPanel;
    public Text laborTitle;

    public (int x, int y) Position { get; set; }
}