using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : UIView
{
    public Text text;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class UIView : MonoBehaviour
{
    public static Action<UICommand> ExecUICmd { get; set; }

    private object objId;
    public object ObjId
    {
        get => objId;
        set
        {
            if (objId == value)
            {
                return;
            }

            objId = value;
        }
    }
}

