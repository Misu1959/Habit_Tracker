using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Habit : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite badgeEmpty;
    [SerializeField] private Sprite badgeUncompleted;
    [SerializeField] private Sprite badgeCompleted;



    [HideInInspector] public HabitData data;

    [Header("\nIn scene objects")]
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private Image imageStreakFill;

    [SerializeField] private Transform weekDays;


    private void Start()
    {
        Setup();
        DisplayInfo();
    }

    private void Setup()
    {
        SetButtons();
    }


    private void SetButtons()
    {
        GetComponent<Button>().onClick.AddListener(M_UI_Main.singleton.OpenDisplayHabitMenu);
        GetComponent<Button>().onClick.AddListener(() => M_UI_DisplayHabit.singleton.SetHabitToDisplay(this));
        


        int val = 0;
        foreach (Transform dayTransform in weekDays)
        {
            int curentVal = val;
            val++;

            DateTime day = M_Date.singleton.startOfWeek.AddDays(curentVal);


            if (day != DateTime.Today)
                dayTransform.GetComponent<Button>().interactable = false;
            else
            {
                dayTransform.GetComponent<Button>().interactable = true;
                
                dayTransform.GetComponent<Button>().onClick.AddListener(M_UI_Main.singleton.OpenUpdateHabitMenu);
                dayTransform.GetComponent<Button>().onClick.AddListener(() => M_UI_UpdateHabit.singleton.ChangeHabitToUpdate(this, day));
            }
        }
    }


    public void TurnButtonsOnOff(bool mode)
    {
        GetComponent<Button>().enabled = mode;
        foreach (Transform dayTransform in weekDays)
            dayTransform.GetComponent<Button>().enabled = mode;
    }



    public void Recolor(Color newColor)
    {
        data.UpdateColor(newColor);
        M_SaveLoad.UpdateHabitColor(data.name, data.color);

        DisplayColor();
    }

    public void UpdateData(float newAmount)
    {
        data.Update(newAmount);
        DisplayInfo();
    }

    private bool IsGoalAchieved(DateTime dayToCheck) => M_SaveLoad.GetHabitInfo(data.name, dayToCheck) < data.targetAmount ? false : true;


    public void DisplayInfo()
    {
        textName.text = data.name;

        imageStreakFill.fillAmount = data.currentAmount / data.targetAmount;


        for (int i = 0; i < 7; i++)
        {

            DateTime dayOfWeek = M_Date.singleton.startOfWeek.AddDays(i);

            if (data.type == HabitType.yesOrNo)
            {
                Image weekDayImage = weekDays.GetChild(i).GetComponent<Image>();

                if (dayOfWeek <= M_Date.singleton.today)
                    weekDayImage.sprite = !IsGoalAchieved(dayOfWeek) ? badgeUncompleted : badgeCompleted;
                else
                    weekDayImage.sprite = badgeEmpty;
            }
            else
            {
                TextMeshProUGUI weekDayText = weekDays.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
                weekDayText.text = M_SaveLoad.GetHabitInfo(data.name, dayOfWeek) + "/" + data.targetAmount + "\n" + data.unit;
            }
        }

        DisplayColor();
    }

    private void DisplayColor()
    {
        textName.color = data.color;
        imageStreakFill.color = data.color;

        for (int i = 0; i < 7; i++)
        {
            DateTime dayOfWeek = M_Date.singleton.startOfWeek.AddDays(i);

            Color32 color;



            if (dayOfWeek <= M_Date.singleton.today)
                color = !IsGoalAchieved(dayOfWeek) ? Color.gray : data.color;
            else
                color = Color.white;


            Transform day = weekDays.GetChild(i);

            if (data.type == HabitType.yesOrNo)
                day.GetComponent<Image>().color = color;
            else
                day.GetComponentInChildren<TextMeshProUGUI>().color = color;
        }

    }

}
