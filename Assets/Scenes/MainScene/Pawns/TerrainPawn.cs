using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TerrainPawn : UIView
{
    public GameObject resource;
    public Grid grid;
    public TerrainPawnWorkHood workHood;
    public UnityEvent<Vector3Int> OnClicked;

    private Vector3Int position;
    public Vector3Int Position
    {
        get => position;
        set
        {
            position = value;

            this.transform.position = grid.GetCellCenterWorld(position);
        }
    }

    void OnEnable()
    {
        resource.SetActive(false);
    }

    public void SetResource(string resourceDesc)
    {
        if (resourceDesc == null)
        {
            resource.SetActive(false);
            return;
        }

        resource.SetActive(true);
        resource.GetComponentInChildren<Text>().text = resourceDesc;
    }

    public void OnClick()
    {
        OnClicked.Invoke(Position);
    }
}
