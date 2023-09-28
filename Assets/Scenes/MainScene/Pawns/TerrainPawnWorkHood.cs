﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class TerrainPawnWorkHood : UIView
{
    public GameObject clanLabor;
    public GameObject productMask;

    public Text productType;
    public Text productValue;

    void OnEnable()
    {
        productMask.SetActive(false);
        clanLabor.SetActive(false);
    }

    public void SetProduct((string Type, decimal Count)? productInfo)
    {
        if(productInfo == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        this.gameObject.SetActive(true);
        productType.text = productInfo.Value.Type;
        productValue.text = productInfo.Value.Count.ToString();
    }

    public void SetLabor(string laborName)
    {
        if(laborName == null)
        {
            clanLabor.SetActive(false);
            productMask.SetActive(true);
            return;
        }

        clanLabor.SetActive(true);
        productMask.SetActive(false);

        clanLabor.GetComponentInChildren<Text>().text = laborName;
    }
}