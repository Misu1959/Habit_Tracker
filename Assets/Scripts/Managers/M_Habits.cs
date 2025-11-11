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
    private int nrOfHabits;



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
        nrOfHabits = M_SaveLoad.LoadNrOfHabits();

        for (int i = 0; i < nrOfHabits; i++)
            CreateHabit(M_SaveLoad.LoadHabitData(i+1), true);
    }




    public void CreateHabit(HabitData newData, bool loadHabit)
    {

        GameObject prefabType = (newData.type == HabitType.yesOrNo) ? prefabHabitYesOrNo : prefabHabitMeasurable;

        Habit habit = Instantiate(prefabType).GetComponent<Habit>();
        habit.data = newData;


        habit.transform.SetParent(M_UI_Main.singleton.panelHabits.transform);
        habit.transform.localScale = Vector3.one;

        habitList.Add(habit);


        habit.UpdateData(M_SaveLoad.GetHabitInfo(newData.name, DateTime.Today));

        if (!loadHabit)
        {
            nrOfHabits++;
            
            M_SaveLoad.SaveNrOfHabits(nrOfHabits);
            M_SaveLoad.SaveHabitData(nrOfHabits, newData);
        }
    }


    public void DestroyHabit(Habit habit)
    {
        M_SaveLoad.DeleteHabitData(habit.data.name);
        Destroy(habit.gameObject);
        habitList.Remove(habit);
    }

}
