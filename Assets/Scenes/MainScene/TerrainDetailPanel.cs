using Feudal.Scenes.Main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainDetailPanel : UIView
{
    public Text title;

    public GameObject laborPanel;
    public Text laborTitle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal T SetWorkHood<T>() where T : WorkHood
    {
        throw new NotImplementedException();
    }
}
