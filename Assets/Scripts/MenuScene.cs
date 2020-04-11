using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    private float fadeInSpeed = 0.33f;

    public RectTransform menuContainer;
    public Transform levelPanel;

    public Transform colorPanel;
    public Transform trailPanel;

    private Vector3 desiredMenuPosition;

    private void Start()
    {
        fadeGroup = FindObjectOfType<CanvasGroup>();

        fadeGroup.alpha = 1;

        InitShop();

        InitLevel();
    }

    private void Update()
    {
        fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;

        menuContainer.anchoredPosition3D = Vector3.Lerp(menuContainer.anchoredPosition3D, desiredMenuPosition, 0.1f);
    }

    private void InitShop()
    {
        if (colorPanel != null || trailPanel == null)
            Debug.Log("You did not asign the color/trail panel in the inspector");

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

    private void InitLevel()
    {
        if (levelPanel != null)
            Debug.Log("You did not asign the level panel in the inspector");

        int i = 0;
        foreach (Transform t in levelPanel)
        {
            int currentIndex = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnLevelSelect(currentIndex));
            i++;
        }
    }

    private void NavigateTo(int menuIndex)
    {
        switch (menuIndex)
        {
            default:
            case 0:
                desiredMenuPosition = Vector3.zero;
                break;
            case 1:
                desiredMenuPosition = Vector3.right * 1280;
                break;

            case 2:
                desiredMenuPosition = Vector3.left * 1280;
                break;
        }
    }

    public void OnPlayClick()
    {
        NavigateTo(1);
        Debug.Log("Play button has been clicked!");
    }

    public void OnShopClick()
    {
        NavigateTo(2);
        Debug.Log("Shop button has been clicked!");
    }

    public void OnBackClick()
    {
        NavigateTo(0);
        Debug.Log("Back button has been clicked!");
    }

    private void OnColorSelect(int currentIndex)
    {
        Debug.Log("Selcting color button : " + currentIndex);
    }

    private void OnTrailSelect(int currentIndex)
    {
        Debug.Log("Selcting Trail button : " + currentIndex);
    }

    private void OnLevelSelect(int currentIndex)
    {
        Debug.Log("Selecting level : " + currentIndex);
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
