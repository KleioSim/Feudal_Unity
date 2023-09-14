using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RightPanel : MonoBehaviour
{
    public GameObject mainContent;

    public GameObject subPanel;
    public GameObject subContent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal T SetCurrentMain<T>() where T : RightMain
    {
        this.gameObject.SetActive(true);

        var views = mainContent.GetComponentsInChildren<RightMain>();

        var current = views.Single(x => x is T);
        current.gameObject.SetActive(true);

        foreach(var view in views.Where(x=>x!=current))
        {
            view.gameObject.SetActive(false);
        }

        return current as T;
    }

    public void OnShowSubView(RightSub rightSub)
    {
        subPanel.SetActive(true);

        rightSub.gameObject.SetActive(true);

        foreach(var sub in subContent
            .GetComponentsInChildren<RightSub>()
            .Where(x=>x != rightSub))
        {
            sub.gameObject.SetActive(false);
        }
    }
}
