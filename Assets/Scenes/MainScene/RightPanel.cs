using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RightPanel : MonoBehaviour
{
    public GameObject currentContent;
    public GameObject subPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClose()
    {
        this.gameObject.SetActive(false);
    }

    public void OnCloseSub()
    {
        subPanel.gameObject.SetActive(false);
    }

    internal T SetCurrent<T>() where T : UIView
    {
        this.gameObject.SetActive(true);

        var views = currentContent.GetComponentsInChildren<UIView>();

        var current = views.Single(x => x is T);
        current.gameObject.SetActive(true);

        foreach(var view in views.Where(x=>x!=current))
        {
            view.gameObject.SetActive(false);
        }

        return current as T;
    }
}
