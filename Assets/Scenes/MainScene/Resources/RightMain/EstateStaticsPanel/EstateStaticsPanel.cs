using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EstateStaticsPanel : RightMain
{
    public Action<(int x, int y)> onClickEstateItem;

    public void SetEstateItems(string[] keys)
    {
        var items = GetComponentsInChildren<EstateStaticsItem>(true);

        var needAddCount = keys.Length - items.Length;
        if (needAddCount > 0)
        {
            for (int i = 0; i < needAddCount; i++)
            {
                var prototype = items.First();

                Instantiate(prototype, prototype.transform.parent);
            }
        }

        items = GetComponentsInChildren<EstateStaticsItem>(true);
        for (int i = 0; i < items.Length; i++)
        {
            if (i + 1 > keys.Count())
            {
                items[i].gameObject.SetActive(false);
                continue;
            }

            var currItem = items[i];

            currItem.gameObject.SetActive(true);
            currItem.EstateId = keys[i];

            currItem.button.onClick.RemoveAllListeners();
            currItem.button.onClick.AddListener(() =>
            {
                onClickEstateItem.Invoke(currItem.Position);
            });
        }
    }
}
