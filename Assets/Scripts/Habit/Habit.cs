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
        foreach (Button dayButton in weekDays.GetComponentsInChildren<Button>())
        {
            int curentVal = val;
            val++;

            DateTime day = M_Date.singleton.startOfCurrentWeek.AddDays(curentVal);


            dayButton.onClick.AddListener(M_UI_Main.singleton.OpenUpdateHabitMenu);
            dayButton.onClick.AddListener(() => M_UI_UpdateHabit.singleton.ChangeHabitToUpdate(this, day));

            dayButton.interactable = (day == M_Date.singleton.today);
            dayButton.GetComponent<Image>().raycastTarget = (day == M_Date.singleton.today);
        }
    }


    public void TurnButtonsOnOff(bool mode)
    {
        GetComponent<Button>().enabled = mode;
        foreach (Button dayButton in weekDays.GetComponentsInChildren<Button>())
        {
            dayButton.enabled = mode;
            dayButton.GetComponent<Image>().raycastTarget = mode;
        }
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

    private bool IsGoalAchieved(DateTime dayToCheck)
    {
        M_SaveLoad.LoadHabitDay(data.name, dayToCheck, out int completion, out float value);

        return completion > 0;
    }

    public void DisplayInfo()
    {
        textName.text = data.name;

        imageStreakFill.fillAmount = data.currentAmount / data.targetAmount;


        for (int i = 0; i < 7; i++)
        {

            DateTime dayOfWeek = M_Date.singleton.startOfCurrentWeek.AddDays(i);

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
                M_SaveLoad.LoadHabitDay(data.name, dayOfWeek, out int completion, out float value);

                TextMeshProUGUI weekDayText = weekDays.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
                weekDayText.text = value + "/" + data.targetAmount + "\n" + data.unit;
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
            DateTime dayOfWeek = M_Date.singleton.startOfCurrentWeek.AddDays(i);

            Color color = !IsGoalAchieved(dayOfWeek) ? Color.white : data.color;


            if (dayOfWeek == M_Date.singleton.today)
                color.a = 1;
            else if (dayOfWeek < M_Date.singleton.today)
                color.a = .33f;
            else
                color.a = .5f;



            Transform day = weekDays.GetChild(i);

            if (data.type == HabitType.yesOrNo)
                day.GetComponent<Image>().color = color;
            else
                day.GetComponentInChildren<TextMeshProUGUI>().color = color;
        }

    }

}
