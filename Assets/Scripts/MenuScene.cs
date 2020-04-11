﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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

    public Text colorBuySetText;
    public Text trailBuySetText;
    public Text goldText;

    private MenuCamera menuCam;

    private int[] colorCost = new int[] { 0, 5, 5, 5, 10, 10, 10, 15, 15, 10 };
    private int[] trailCost = new int[] { 0, 20, 40, 40, 60, 60, 80, 80, 100, 100 };
    private int selectedColorIndex = 0;
    private int selectedTrailIndex = 0;
    private int activeColorIndex;
    private int activeTrailIndex;

    private Vector3 desiredMenuPosition;

    public AnimationCurve enteringLevelZoomCurver;
    public bool isEnteringLevel = false;
    public float zoomDuration = 3.0f;
    public float zoomTransition;

    private void Start()
    {
        menuCam = FindObjectOfType<MenuCamera>();

        SaveManager.Instance.state.gold = 999;

        SetCameraTo(Manager.Instance.menuFocus);

        UpdateGoldText();

        fadeGroup = FindObjectOfType<CanvasGroup>();

        fadeGroup.alpha = 1;

        InitShop();

        InitLevel();

        OnColorSelect(SaveManager.Instance.state.activeColor);
        SetColor(SaveManager.Instance.state.activeColor);

        OnTrailSelect(SaveManager.Instance.state.activeTrail);
        SetTrail(SaveManager.Instance.state.activeTrail);

        colorPanel.GetChild(SaveManager.Instance.state.activeColor).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        trailPanel.GetChild(SaveManager.Instance.state.activeTrail).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
    }

    private void Update()
    {
        fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;

        menuContainer.anchoredPosition3D = Vector3.Lerp(menuContainer.anchoredPosition3D, desiredMenuPosition, 0.1f);

        if (isEnteringLevel)
        {
            zoomTransition += (1 / zoomDuration) * Time.deltaTime;

            menuContainer.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 5, enteringLevelZoomCurver.Evaluate(zoomTransition));

            Vector3 newDesiredPosition = desiredMenuPosition * 5;

            RectTransform rt = levelPanel.GetChild(Manager.Instance.currentLevel).GetComponent<RectTransform>();

            newDesiredPosition -= rt.anchoredPosition3D * 5;

            menuContainer.anchoredPosition3D = Vector3.Lerp(desiredMenuPosition, newDesiredPosition, enteringLevelZoomCurver.Evaluate(zoomTransition));

            fadeGroup.alpha = zoomTransition;

            if (zoomTransition >= 1)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }

    private void InitShop()
    {
        if (colorPanel == null || trailPanel == null)
            Debug.LogError("You did not asign the color/trail panel in the inspector");

        int i = 0;
        foreach (Transform t in colorPanel)
        {
            int currentIndex = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnColorSelect(currentIndex));

            Image img = t.GetComponent<Image>();
            img.color = SaveManager.Instance.IsColorOwned(i)
                ? Manager.Instance.playerColors[currentIndex]
                : Color.Lerp(Manager.Instance.playerColors[currentIndex], new Color(0, 0, 0, 1), 0.25f);

            i++;
        }

        i = 0;

        foreach (Transform t in trailPanel)
        {
            int currentIndex = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnTrailSelect(currentIndex));

            Image img = t.GetComponent<Image>();
            img.color = SaveManager.Instance.IsTrailOwned(i) ? Color.white : new Color(0.7f, 0.7f, 0.7f);

            i++;
        }
    }

    private void InitLevel()
    {
        if (levelPanel == null)
            Debug.LogError("You did not asign the level panel in the inspector");

        int i = 0;
        foreach (Transform t in levelPanel)
        {
            int currentIndex = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnLevelSelect(currentIndex));

            Image img = t.GetComponent<Image>();

            if (i <= SaveManager.Instance.state.completedLevel)
            {
                if (i == SaveManager.Instance.state.completedLevel)
                {
                    img.color = Color.white;
                }
                else
                {
                    img.color = Color.green;
                }
            }
            else
            {
                b.interactable = false;

                img.color = Color.grey;
            }

            i++;
        }
    }


    private void SetCameraTo(int menuIndex)
    {
        NavigateTo(menuIndex);
        menuContainer.anchoredPosition3D = desiredMenuPosition;
    }

    private void NavigateTo(int menuIndex)
    {
        switch (menuIndex)
        {
            default:
            case 0:
                desiredMenuPosition = Vector3.zero;
                menuCam.BackToMainMenu();
                break;
            case 1:
                desiredMenuPosition = Vector3.right * 1280;
                menuCam.MoveToLevel();
                break;

            case 2:
                desiredMenuPosition = Vector3.left * 1280;
                menuCam.MoveToShop();
                break;
        }
    }

    private void SetColor(int index)
    {
        activeColorIndex = index;
        SaveManager.Instance.state.activeColor = index;

        Manager.Instance.playerMaterial.color = Manager.Instance.playerColors[index];

        colorBuySetText.text = "Current";

        SaveManager.Instance.Save();
    }

    private void SetTrail(int index)
    {
        activeTrailIndex = index;
        SaveManager.Instance.state.activeTrail = index;
        trailBuySetText.text = "Current";

        SaveManager.Instance.Save();
    }

    private void UpdateGoldText()
    {
        goldText.text = SaveManager.Instance.state.gold.ToString();
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

        if (selectedColorIndex == currentIndex)
            return;

        colorPanel.GetChild(currentIndex).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        colorPanel.GetChild(selectedColorIndex).GetComponent<RectTransform>().localScale = Vector3.one;

        selectedColorIndex = currentIndex;

        if (SaveManager.Instance.IsColorOwned(currentIndex))
        {
            if (activeColorIndex == currentIndex)
            {
                colorBuySetText.text = "Current";
            }
            else
            {
                colorBuySetText.text = "Select";
            }
        }
        else
        {
            colorBuySetText.text = "Buy : " + colorCost[currentIndex].ToString();
        }
    }

    private void OnTrailSelect(int currentIndex)
    {
        Debug.Log("Selcting Trail button : " + currentIndex);

        if (selectedTrailIndex == currentIndex)
            return;

        trailPanel.GetChild(currentIndex).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        trailPanel.GetChild(selectedTrailIndex).GetComponent<RectTransform>().localScale = Vector3.one;

        selectedTrailIndex = currentIndex;

        if (SaveManager.Instance.IsTrailOwned(currentIndex))
        {
            if (activeTrailIndex == currentIndex)
            {
                trailBuySetText.text = "Current";
            }
            else
            {
                trailBuySetText.text = "Select";
            }
        }
        else
        {
            trailBuySetText.text = "Buy : " + trailCost[currentIndex].ToString();
        }
    }

    private void OnLevelSelect(int currentIndex)
    {
        Manager.Instance.currentLevel = currentIndex;
        isEnteringLevel = true;
        Debug.Log("Selecting level : " + currentIndex);
    }

    public void OnColorBuySet()
    {
        Debug.Log("Buy/Set Color");

        if (SaveManager.Instance.IsColorOwned(selectedColorIndex))
        {
            SetColor(selectedColorIndex);
        }
        else
        {
            if (SaveManager.Instance.BuyColor(selectedColorIndex, colorCost[selectedColorIndex]))
            {
                SetColor(selectedColorIndex);
                colorPanel.GetChild(selectedColorIndex).GetComponent<Image>().color = Manager.Instance.playerColors[selectedColorIndex];
                UpdateGoldText();
            }
            else
            {
                Debug.Log("Not enough gold");
            }
        }
    }

    public void OnTrailBuySet()
    {
        Debug.Log("Buy/Set Trail");

        if (SaveManager.Instance.IsTrailOwned(selectedTrailIndex))
        {
            SetTrail(selectedTrailIndex);
        }
        else
        {
            if (SaveManager.Instance.BuyTrail(selectedTrailIndex, trailCost[selectedTrailIndex]))
            {
                SetTrail(selectedTrailIndex);
                trailPanel.GetChild(selectedTrailIndex).GetComponent<Image>().color = Color.white;
                UpdateGoldText();
            }
            else
            {
                Debug.Log("Not enough gold");
            }
        }
    }
}
