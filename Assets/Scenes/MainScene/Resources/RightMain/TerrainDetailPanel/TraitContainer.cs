﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TraitContainer : UIView
{
    public GameObject Content;

    public GameObject resource;

    public void SetEnable(bool flag)
    {
        Content.SetActive(flag);
    }

    //public void SetTraitItems(IEnumerable<Enum> traits)
    //{
    //    var currItems = Content.GetComponentsInChildren<TraitView>(true);

    //    var needAddCount = traits.Count() - currItems.Length;
    //    if (needAddCount > 0)
    //    {
    //        for (int i = 0; i < needAddCount; i++)
    //        {
    //            var prototype = currItems.First();
    //            Instantiate(prototype, prototype.transform.parent);
    //        }
    //    }

    //    currItems = Content.GetComponentsInChildren<TraitView>(true);
    //    for (int i = 0; i < currItems.Length; i++)
    //    {
    //        if (i + 1 > traits.Count())
    //        {
    //            currItems[i].gameObject.SetActive(false);
    //            continue;
    //        }

    //        currItems[i].gameObject.SetActive(true);
    //        currItems[i].trait = traits.ElementAt(i);
    //    }
    //}

    public void SetResource(string resourceName)
    {
        if(resourceName == null)
        {
            resource.SetActive(false);
        }

        resource.SetActive(true);
        resource.GetComponentInChildren<Text>().text = resourceName;
    }
}
