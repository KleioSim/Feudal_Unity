using UnityEngine.UI;

public class LaborSelectorItem : UIView
{
    public string Id { get; internal set; }

    public Text laborName;
    public Text CountInfo;

    public Toggle toggle;
}