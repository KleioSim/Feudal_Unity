using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskContainer : UIView
{
    public TaskItem defaultItem;

    public void SetTaskItems(string[] keys)
    {
        var currItems = GetComponentsInChildren<TaskItem>(true);

        var needAddCount = keys.Length - currItems.Length;
        if (needAddCount > 0)
        {
            for (int i = 0; i < needAddCount; i++)
            {
                Instantiate(defaultItem, defaultItem.transform.parent);
            }
        }

        currItems = GetComponentsInChildren<TaskItem>(true);
        for (int i = 0; i < currItems.Length; i++)
        {
            if (i+1 > keys.Count())
            {
                currItems[i].gameObject.SetActive(false);
                continue;
            }

            currItems[i].gameObject.SetActive(true);
            currItems[i].Id = keys[i];
        }
    }
}
