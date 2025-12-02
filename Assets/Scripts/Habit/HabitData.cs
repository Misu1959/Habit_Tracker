using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public enum HabitType
{
    yesOrNo,
    measurable
}


public class HabitData
{
    public DateTime creationDate {  get; private set; }

    public HabitType type { get; private set; }
    public Color color { get; private set; }

    public string name { get; private set; }
    public string question { get; private set; }
    
    public string unit { get; private set; }
    public float targetAmount { get; private set; }
    public float currentAmount { get; private set; }


    public float completionValue => (targetAmount == 0) ? 1 : currentAmount / targetAmount;

    public HabitData(DateTime _creationDate, HabitType _type, Color _color, string _name, string _question, string _unit, float _targetAmount)
    {
        creationDate = _creationDate;
        
        type         = _type;
        color        = _color;
        name         = _name;
        question     = _question;
        unit         = _unit;
        targetAmount = _targetAmount;

        M_SaveLoad.LoadHabitDay(name,M_Date.singleton.today,out int completion,out float value);
    
        currentAmount = value;
    }

    public void Update(float newAmount) =>  currentAmount = newAmount;
    public void UpdateColor(Color newColor) => color = newColor;
}
