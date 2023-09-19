using Feudal.Scenes.Main;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TerrainDetailPanel : RightMain
{
    public Text title;

    public GameObject workDetailPanel;
    
    public (int x, int y) Position { get; set; }

    internal T SetCurrentWorkHood<T>() where T : WorkHood
    {
        workDetailPanel.SetActive(true);

        var workHoods = workDetailPanel.GetComponentsInChildren<WorkHood>(true);
        
        var currentWorkHood = workHoods.Single(x => x is T) as T;
        currentWorkHood.gameObject.SetActive(true);

        foreach (var workHood in workHoods.Where(x => x != currentWorkHood))
        {
            workHood.gameObject.SetActive(false);
        }

        var laborWork = workDetailPanel.GetComponentsInChildren<LaborWorkDetail>().Single();
        laborWork.Position = Position;
        laborWork.button.onClick.AddListener(() =>
        {
            showSub.Invoke(typeof(LaborSelector), OnSelectWorkHoodLabor);
        });

        return currentWorkHood;
    }

    private void OnSelectWorkHoodLabor(object obj)
    {
        if(!(obj is string laborId))
        {
            throw new Exception();
        }

        ExecUICmd(new DiscoverCommand(laborId, Position));
    }
}