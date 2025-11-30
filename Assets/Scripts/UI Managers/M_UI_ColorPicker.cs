using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.Events;

public class M_UI_ColorPicker : MonoBehaviour
{
    public static M_UI_ColorPicker singleton;


    [Header("In scene objects")]
    [SerializeField] private Image imageHabitColor;

    [SerializeField] private Transform colorsList;
    [SerializeField] private Transform colorCheck;


    private void Awake() => Initialize();

    void Start() => Setup();

    private void Initialize()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(this);
    }

    private void Setup()
    {
        foreach (Transform color in colorsList)
        {
            color.GetComponent<Button>().onClick.AddListener(() => ChangeSelectedColor(color.GetSiblingIndex()));
            color.GetComponent<Button>().onClick.AddListener(M_UI_Main.singleton.CloseColorPickerMenu);
            color.GetComponent<Button>().onClick.AddListener(AddRecolorAction);
        }

    }

    private void AddRecolorAction()
    {
        if (!M_UI_Main.singleton.panelDisplayHabit.gameObject.activeInHierarchy)
        {
            M_UI_Main.singleton.panelCreateHabit.GetComponent<PageColorizer>().Colorize(imageHabitColor.color);
            return;
        }

        M_UI_Main.singleton.panelDisplayHabit.GetComponent<PageColorizer>().Colorize(imageHabitColor.color);

        M_UI_DisplayHabit.singleton.GetDisplayHabit().Recolor(imageHabitColor.color);
    }
    


    public void ChangeSelectedColor(int colorIndex)
    {
        colorCheck.SetParent(colorsList.GetChild(colorIndex));
        colorCheck.localPosition = Vector2.zero;

        imageHabitColor.color = colorsList.GetChild(colorIndex).GetComponent<Image>().color;
    }

    public void ChangeSelectedColor(Color color)
    {
        int colorIndex;

        for (colorIndex = 0; colorIndex < colorsList.childCount; colorIndex++)
            if (colorsList.GetChild(colorIndex).GetComponent<Image>().color == color)
                break;

        ChangeSelectedColor(colorIndex);
    }
}
