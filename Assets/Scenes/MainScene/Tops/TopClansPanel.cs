using System;
using System.Linq;

public class TopClansPanel : UIView
{
    void Awake()
    {
        var currItems = GetComponentsInChildren<TopClanItem>(true);
        foreach (var item in currItems)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void SetClanItems(Enum[] keys)
    {
        var currItems = GetComponentsInChildren<TopClanItem>(true);

        var needAddCount = keys.Length - currItems.Length;
        if (needAddCount > 0)
        {
            for (int i = 0; i < needAddCount; i++)
            {
                var prototype = currItems.First();
                Instantiate(prototype, prototype.transform.parent);
            }
        }

        currItems = GetComponentsInChildren<TopClanItem>(true);
        for (int i = 0; i < currItems.Length; i++)
        {
            if (i + 1 > keys.Count())
            {
                currItems[i].gameObject.SetActive(false);
                continue;
            }

            currItems[i].gameObject.SetActive(true);
            currItems[i].ClanType = keys[i];
        }
    }
}
