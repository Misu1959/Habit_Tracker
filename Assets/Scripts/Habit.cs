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
        Recolor();
        DisplayInfo();
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
                day.GetComponent<TextMeshProUGUI>().color = data.color;
        }

    }


    public void DisplayInfo()
    {
        textName.text = data.name;

        imageStreakFill.transform.localScale = new Vector3(data.currentAmount / data.targetAmount, 1, 1);


        if(data.type == HabitType.measurable)
        {
            foreach (Transform day in weekDays)
                day.GetComponent<TextMeshProUGUI>().text = data.currentAmount + "/" + data.targetAmount + "\n" + data.unit;
        }
    }

}
