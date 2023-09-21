using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClanStaticsItem : UIView
{
    public Text title;
    public Text clanType;
    public Text popCount;
    public Text estateCount;

    public Button button;

    public string ClanId { get; internal set; }
}
