using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    private float fadeInSpeed = 0.33f;

    public Transform colorPanel;
    public Transform trailPanel;

    private void Start()
    {
        fadeGroup = FindObjectOfType<CanvasGroup>();

        fadeGroup.alpha = 1;

        InitShop();
    }

    private void Update()
    {
        fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;
    }

    private void InitShop()
    {
        if (colorPanel != null || trailPanel == null)
            Debug.Log("You did not asign the color/trail panelin the inspector");

        int i = 0;
        foreach (Transform t in colorPanel)
        {
            int currentIndex = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnColorSelect(currentIndex));
            i++;
        }

        i = 0;

        foreach (Transform t in trailPanel)
        {
            int currentIndex = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnTrailSelect(currentIndex));
            i++;
        }
    }

    public void OnPlayClick()
    {
        Debug.Log("Play button has been clicked!");
    }

    public void OnShopClick()
    {
        Debug.Log("Shop button has been clicked!");
    }

    private void OnColorSelect(int currentIndex)
    {
        Debug.Log("Selcting color button : " + currentIndex);
    }

    private void OnTrailSelect(int currentIndex)
    {
        Debug.Log("Selcting Trail button : " + currentIndex);
    }

    public void OnColorBuySet()
    {
        Debug.Log("Buy/Set Color");
    }

    public void OnTrailBuySet()
    {
        Debug.Log("Buy/Set Trail");
    }
}
