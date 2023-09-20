using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : UIView
{
    public Text title;
    public Slider percent;

    public string Id { get; set; }

    public void OnCancel()
    {
        ExecUICmd(new CancelTaskCommand(Id));
    }
}
