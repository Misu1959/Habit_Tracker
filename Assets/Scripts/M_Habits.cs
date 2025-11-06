using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Habits : MonoBehaviour
{
    public static M_Habits singleton;

    [Header("Prefabs")]
    [SerializeField] private GameObject prefabHabitYesOrNo;
    [SerializeField] private GameObject prefabHabitMeasurable;



    [HideInInspector] public List<Habit> habitList;



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
    }




    public void CreateHabit(HabitData newData)
    {
        GameObject prefabType = (newData.type == HabitType.yesOrNo) ? prefabHabitYesOrNo : prefabHabitMeasurable;

        Habit habit = Instantiate(prefabType).GetComponent<Habit>();
        habit.data = newData;
        
        
        habit.transform.SetParent(M_UI_Main.singleton.panelHabits.transform);
        habit.transform.localScale = Vector3.one;

        habitList.Add(habit);

    }


    public void DestroyHabit(Habit habit)
    {
        habitList.Remove(habit);
    }

}
