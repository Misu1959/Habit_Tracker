using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HabitStats : MonoBehaviour
{
    [Header("\nIn scene objects")]
    [SerializeField] private TextMeshProUGUI labelDate;
    [SerializeField] private TextMeshProUGUI labelCompletion;
    [SerializeField] private TextMeshProUGUI labelValue;


    public void DisplayStats(HabitType type, int completion, float value, string dateFormat, DateTime startDate, DateTime endDate = default(DateTime))
    {
        string datePeriod = " ";

        if(endDate != default(DateTime))
            datePeriod = startDate.ToString("%d MMM yyyy") + " - " + endDate.ToString("%d MMM yyyy");
        else
            datePeriod = startDate.ToString(dateFormat);

        labelDate.text = datePeriod;


        labelCompletion.text = completion.ToString();
        labelValue.text = value.ToString();

        labelValue.transform.parent.gameObject.SetActive(type == HabitType.measurable);
    }
}
