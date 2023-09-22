using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class LaborSelector : RightSub
{
    public Button confrimButton;

    private string selectedLaborId = null;

    protected void Start()
    {
        confrimButton.onClick.AddListener(() =>
        {
            confirm.Invoke(selectedLaborId);
        });
    }

    public void SetLaborItems(string[] keys)
    {
        var currItems = GetComponentsInChildren<LaborSelectorItem>(true);

        var needAddCount = keys.Length - currItems.Length;
        if (needAddCount > 0)
        {
            for(int i=0; i< needAddCount; i++)
            {
                var prototype = currItems.First();
                Instantiate(prototype, prototype.transform.parent);
            }
        }

        currItems = GetComponentsInChildren<LaborSelectorItem>(true);
        for (int i = 0; i<currItems.Length; i++)
        {
            if(i+1 > keys.Count())
            {
                currItems[i].gameObject.SetActive(false);
                continue;
            }

            currItems[i].gameObject.SetActive(true);
            currItems[i].Id = keys[i];
        }
    }

    public void OnSelectLaborItemChanged(bool flag)
    {
        selectedLaborId = GetComponentsInChildren<LaborSelectorItem>()
            .SingleOrDefault(x => x.toggle.isOn)?.Id;

        confrimButton.interactable = selectedLaborId != null;
    }
}
