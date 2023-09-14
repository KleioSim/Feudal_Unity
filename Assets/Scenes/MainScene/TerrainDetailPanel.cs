using Feudal.Scenes.Main;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TerrainDetailPanel : RightMain
{
    public TerrainWorkDetail workDetail;
    public Text title;

    public (int x, int y) Position { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnShowLaborSelector()
    {

    }
}

public class UIView : MonoBehaviour
{
    public static Action<UICommand> ExecUICmd;
}
