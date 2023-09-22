using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public partial class ClanDetailPanel : RightMain
{
    public Action<(int x, int y)> onClickEstateItem;

    public string ClanId { get; private set; }

    private object[] parameters;
    public override object[] Parameters
    {
        get => parameters;
        set
        {
            parameters = value;
            if (parameters[0] is not string)
            {
                throw new System.Exception();
            }

            ClanId = (string)parameters[0];
        }
    }

    void Awake()
    {
        CollapseAll();
    }

    public void CollapseAll()
    {
        foreach (var toggle in GetComponentsInChildren<Toggle>())
        {
            toggle.isOn = false;
        }
    }

    public void SetEstateItems(string[] keys)
    {
        var items = GetComponentsInChildren<EstateItemInClanDetail>(true);

        var needAddCount = keys.Length - items.Length;
        if (needAddCount > 0)
        {
            for (int i = 0; i < needAddCount; i++)
            {
                var prototype = items.First();

                Instantiate(prototype, prototype.transform.parent);
            }
        }

        items = GetComponentsInChildren<EstateItemInClanDetail>(true);
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
