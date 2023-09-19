using UnityEngine;
using UnityEngine.UI;

public class LaborWorkDetail : UIView
{
    public GameObject laborPanel;
    public Text laborTitle;

    public Button AddLaborButton;
    public Button RemoveLaborButton;

    public string taskId { get; set; }

    public (int x, int y) Position { get; set; }
}