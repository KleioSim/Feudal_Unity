using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClanStaticsPanel : RightMain
{
    public Action<string> onClickClanItem;

    public void SetClanItems(string[] keys)
    {
        var items = GetComponentsInChildren<ClanStaticsItem>(true);

        var needAddCount = keys.Length - items.Length;
        if (needAddCount > 0)
        {
            for (int i = 0; i < needAddCount; i++)
            {
                var prototype = items.First();

                Instantiate(prototype, prototype.transform.parent);
            }
        }

        items = GetComponentsInChildren<ClanStaticsItem>(true);
        for (int i = 0; i < items.Length; i++)
        {
            if (i + 1 > keys.Count())
            {
                items[i].gameObject.SetActive(false);
                continue;
            }

            var currItem = items[i];

            currItem.gameObject.SetActive(true);
            currItem.ClanId = keys[i];

            currItem.button.onClick.RemoveAllListeners();
            currItem.button.onClick.AddListener(() =>
            {
                onClickClanItem.Invoke(currItem.ClanId);
            });
        }
    }
}
