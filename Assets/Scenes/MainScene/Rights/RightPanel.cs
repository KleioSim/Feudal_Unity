using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RightPanel : UIView
{
    public GameObject mainPanel;
    public GameObject mainContent;

    public GameObject subPanel;
    public GameObject subContent;

    public Button PrevMainViewButton;
    public Button NextMainViewButton;

    private RightMain[] mainPrefabs;
    private RightSub[] subPrefabs;


    private int currMainViewIndex;
    private List<RightMain> mainCaches;

    void Start()
    {
        mainPanel.SetActive(false);

        mainPrefabs = Resources.LoadAll<RightMain>("RightMain");
        subPrefabs = Resources.LoadAll<RightSub>("RightSub");

        mainCaches = new List<RightMain>();

        PrevMainViewButton.onClick.AddListener(PrevMainView);
        NextMainViewButton.onClick.AddListener(NextMainView);

        PrevMainViewButton.interactable = false;
        NextMainViewButton.interactable = false;
    }

    void OnDestroy()
    {
        mainCaches.Clear();
    }

    internal T OnShowMainView<T>(params object[] parameters) where T : RightMain
    {

        CloseSubView();

        foreach (var mainView in mainCaches)
        {
            mainView.gameObject.SetActive(false);
        }

        mainPanel.SetActive(true);

        var existView = mainCaches.SingleOrDefault(x => x.GetType() == typeof(T) && Enumerable.SequenceEqual(x.Parameters, parameters));
        if(existView != null)
        {
            mainCaches.Remove(existView);
            mainCaches.Add(existView);
            existView.gameObject.SetActive(true);

            currMainViewIndex = mainCaches.Count - 1;

            UpdateNextPreButton();
            return existView as T;
        }

        var prefab = mainPrefabs.Single(x => x is T);

        var newView = Instantiate(prefab, mainContent.transform);
        newView.showSub.AddListener((subType, OnSubConfirm) =>
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

        newView.Parameters = parameters;

        mainCaches.Add(newView);
        currMainViewIndex = mainCaches.Count - 1;

        UpdateNextPreButton();

        return newView as T;
    }

    public void CloseMainView()
    {
        CloseSubView();
        foreach (var mainView in mainCaches)
        {
            Destroy(mainView.gameObject);
        }
        mainCaches.Clear();

        mainPanel.SetActive(false);
        
    }

    public void PrevMainView()
    {
        foreach(var mainView in mainCaches)
        {
            mainView.gameObject.SetActive(false);
        }

        currMainViewIndex--;
        mainCaches[currMainViewIndex].gameObject.SetActive(true);

        UpdateNextPreButton();

        ExecUICmd.Invoke(new UpdateViewCommand());
    }

    public void NextMainView()
    {
        foreach (var mainView in mainCaches)
        {
            mainView.gameObject.SetActive(false);
        }

        currMainViewIndex++;
        mainCaches[currMainViewIndex].gameObject.SetActive(true);

        UpdateNextPreButton();

        ExecUICmd.Invoke(new UpdateViewCommand());
    }

    public void CloseSubView()
    {
        ClearSubView();

        subPanel.gameObject.SetActive(false);
    }

    private void ClearSubView()
    {
        var subView = subContent.GetComponentsInChildren<RightSub>().SingleOrDefault();
        if (subView != null)
        {
            Destroy(subView.gameObject);
        }
    }

    private void UpdateNextPreButton()
    {
        NextMainViewButton.interactable = currMainViewIndex < mainCaches.Count - 1;

        PrevMainViewButton.interactable = currMainViewIndex > 0;
    }
}