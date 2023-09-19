using UnityEngine;
using UnityEngine.UI;

public class LaborWorkDetail : UIView
{
    public GameObject laborPanel;
    public Text laborTitle;
    public Button button;

    public (int x, int y) Position { get; set; }
}