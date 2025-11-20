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
        for (int i = 0; i < M_SaveLoad.LoadNrOfHabits(); i++)
            CreateHabit(M_SaveLoad.LoadHabitData(i), true);
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
            M_SaveLoad.SaveNrOfHabits();
            M_SaveLoad.SaveHabitData(habit);
        }
    }


    public void DestroyHabit(Habit habit)
    {
        M_SaveLoad.DeleteHabitData(habit.data.name);

        habitList.Remove(habit);
        Destroy(habit.gameObject);

        StartCoroutine(RemData());
    }

    IEnumerator RemData()
    {
        yield return new WaitForSeconds(.1f);
        M_SaveLoad.SaveNrOfHabits();
    }
}
