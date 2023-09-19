using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RightPanel : UIView
{
    public GameObject mainPanel;
    public GameObject mainContent;

    public GameObject subPanel;
    public GameObject subContent;

    private RightMain[] mainPrefabs;
    private RightSub[] subPrefabs;

    void Start()
    {
        mainPanel.SetActive(false);

        mainPrefabs = Resources.LoadAll<RightMain>("RightMain");
        subPrefabs = Resources.LoadAll<RightSub>("RightSub");
    }

    internal T OnShowMainView<T>() where T : RightMain
    {
        mainPanel.SetActive(true);

        ClearMainView();

        CloseSubView();

        var prefab = mainPrefabs.Single(x => x is T);

        var mainView = Instantiate(prefab, mainContent.transform);
        mainView.showSub.AddListener((subType, OnSubConfirm) =>
        {
            subPanel.gameObject.SetActive(true);

            var subPrefab = subPrefabs.Single(x => x.GetType() == subType);
            var subView = Instantiate(subPrefab, subContent.transform);

            subView.confirm.AddListener(OnSubConfirm);
            subView.confirm.AddListener((obj) =>
            {
                CloseSubView();
            });

            ExecUICmd(new UpdateViewCommand());
        });

        return mainView as T;
    }

    public void CloseMainView()
    {
        CloseSubView();

        ClearMainView();

        mainPanel.SetActive(false);
    }


    public void CloseSubView()
    {
        ClearSubView();

        subPanel.gameObject.SetActive(false);
    }

    private void ClearMainView()
    {
        var mainView = mainContent.GetComponentsInChildren<RightMain>().SingleOrDefault();
        if (mainView != null)
        {
            Destroy(mainView.gameObject);
        }
    }

    private void ClearSubView()
    {
        var subView = subContent.GetComponentsInChildren<RightSub>().SingleOrDefault();
        if (subView != null)
        {
            Destroy(subView.gameObject);
        }
    }
}
