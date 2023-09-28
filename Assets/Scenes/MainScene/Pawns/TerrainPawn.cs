using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TerrainPawn : UIView
{
    public GameObject resource;
    public Grid grid;
    public TerrainPawnWorkHood workHood;

    private (int x, int y) position;
    public (int x, int y) Position
    {
        get => position;
        set
        {
            position = value;

            this.transform.position = grid.GetCellCenterWorld(new Vector3Int(position.x, position.y));
        }
    }

    void OnEnable()
    {
        resource.SetActive(false);
        workHood.gameObject.SetActive(false);
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
}
