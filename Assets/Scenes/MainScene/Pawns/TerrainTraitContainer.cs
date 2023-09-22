using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainTraitContainer : UIView
{
    public void SetTraitItems((int x, int y)[] positions)
    {
        var currItems = GetComponentsInChildren<TerrainTraitItem>(true);

        var needAddCount = positions.Length - currItems.Length;
        if (needAddCount > 0)
        {
            for (int i = 0; i < needAddCount; i++)
            {
                Instantiate(currItems.First(), currItems.First().transform.parent);
            }
        }

        currItems = GetComponentsInChildren<TerrainTraitItem>(true);
        for (int i = 0; i < currItems.Length; i++)
        {
            if (i + 1 > positions.Count())
            {
                currItems[i].gameObject.SetActive(false);
                continue;
            }

            currItems[i].gameObject.SetActive(true);
            currItems[i].Position = positions[i];
        }
    }
}
