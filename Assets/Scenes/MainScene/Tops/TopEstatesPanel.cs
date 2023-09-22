using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TopEstatesPanel : UIView
{
    void Awake()
    {
        var currItems = GetComponentsInChildren<TopEstateItem>(true);
        foreach (var item in currItems)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void SetProductItems(Enum[] keys)
    {
        var currItems = GetComponentsInChildren<TopEstateItem>(true);

        var needAddCount = keys.Length - currItems.Length;
        if (needAddCount > 0)
        {
            for (int i = 0; i < needAddCount; i++)
            {
                var prototype = currItems.First();
                Instantiate(prototype, prototype.transform.parent);
            }
        }

        currItems = GetComponentsInChildren<TopEstateItem>(true);
        for (int i = 0; i < currItems.Length; i++)
        {
            if (i + 1 > keys.Count())
            {
                currItems[i].gameObject.SetActive(false);
                continue;
            }

            currItems[i].gameObject.SetActive(true);
            currItems[i].EstateType = keys[i];
        }
    }
}
