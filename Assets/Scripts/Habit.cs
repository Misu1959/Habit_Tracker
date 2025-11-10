using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Habit : MonoBehaviour
{
    [HideInInspector] public HabitData data;


    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private Image imageStreakFill;

    [SerializeField] private Transform weekDays;


    private void Start()
    {
        Setup();

        
        Recolor();
        DisplayInfo();
    }

    private void Setup()
    {
        SetButtons();
    }

    private void SetButtons()
    {
        foreach (Transform day in weekDays)
        {
            day.GetComponent<Button>().onClick.AddListener(M_UI_Main.singleton.OpenUpdateHabitMenu);
            day.GetComponent<Button>().onClick.AddListener(() => M_UI_UpdateHabit.singleton.ChangeHabitToUpdate(this));
        }
    }

    public void Recolor()
    {
        textName.color = data.color;
        imageStreakFill.color = data.color;


        if (data.type == HabitType.yesOrNo)
        {
            foreach (Transform day in weekDays)
                day.GetComponent<Image>().color = data.color;
        } 
        else
        {
            foreach (Transform day in weekDays)
                day.GetComponentInChildren<TextMeshProUGUI>().color = data.color;
        }

    }


    public void DisplayInfo()
    {
        textName.text = data.name;

        imageStreakFill.fillAmount = data.currentAmount / data.targetAmount;


        if(data.type == HabitType.measurable)
        {
            foreach (Transform day in weekDays)
                day.GetComponentInChildren<TextMeshProUGUI>().text = data.currentAmount + "/" + data.targetAmount + "\n" + data.unit;
        }
    }

}
