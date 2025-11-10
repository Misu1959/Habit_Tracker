using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M_UI_Main : MonoBehaviour
{
    public static M_UI_Main singleton;

    [Header("\nIn scene objects")]
    [SerializeField] private Button buttonOpenCreateHabitMenu;
    [SerializeField] private Button buttonOpenSorHabitsMenu;


    [field:SerializeField] public Transform panelHabits { get; private set; }

    [field: SerializeField] public Transform panelCreateHabit { get; private set; }
    [field: SerializeField] public Transform panelDisplayHabit { get; private set; }
    [field: SerializeField] public Transform panelUpdateHabit { get; private set; }

    [field: SerializeField] public Transform panelSortHabits { get; private set; }
    [field:SerializeField] public  Transform panelColorPicker { get; private set; }





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
        panelUpdateHabit.GetComponent<Button>().onClick.AddListener(CloseUpdateHabitMenu);
        panelColorPicker.GetComponent<Button>().onClick.AddListener(CloseColorPickerMenu);
        panelSortHabits.GetComponent<Button>().onClick.AddListener(CloseSortHabitsMenu);


        SetButtonOpenCreateHabitMenu();
        SetButtonOpenSortHabitsMenu();
    }


    #region Setup
    private void SetButtonOpenCreateHabitMenu()
    {
        buttonOpenCreateHabitMenu.onClick.AddListener(OpenCreateHabitMenu);
        buttonOpenCreateHabitMenu.onClick.AddListener(M_UI_CreateHabit.singleton.ResetInputs);
    }

    private void SetButtonOpenSortHabitsMenu()
    {
        buttonOpenSorHabitsMenu.onClick.AddListener(OpenSortHabitsMenu);
    }



    #endregion


    #region Methods


    public void OpenCreateHabitMenu() => panelCreateHabit.gameObject.SetActive(true);
    public void CloseCreateHabitMenu() => panelCreateHabit.gameObject.SetActive(false);

    public void OpenUpdateHabitMenu() => panelUpdateHabit.gameObject.SetActive(true);
    public void CloseUpdateHabitMenu() => panelUpdateHabit.gameObject.SetActive(false);


    public void OpenSortHabitsMenu() => panelSortHabits.gameObject.SetActive(true);
    public void CloseSortHabitsMenu() => panelSortHabits.gameObject.SetActive(false);

    public void OpenColorPickerMenu() => panelColorPicker.gameObject.SetActive(true);
    public void CloseColorPickerMenu() => panelColorPicker.gameObject.SetActive(false);







    public void TurnLayoutOn()
    {
        panelHabits.GetComponent<ContentSizeFitter>().enabled = true;
        panelHabits.GetComponent<VerticalLayoutGroup>().enabled = true;
    }
    public void TurnLayoutOff()
    {
        panelHabits.GetComponent<ContentSizeFitter>().enabled = false;
        panelHabits.GetComponent<VerticalLayoutGroup>().enabled = false;
    }
    public void RefreshLayout()
    {
        if (panelHabits.GetComponent<VerticalLayoutGroup>().enabled)
        {
            TurnLayoutOff();

            for (int i = 0; i < M_Habits.singleton.habitList.Count; i++)
                M_Habits.singleton.habitList[i].transform.SetSiblingIndex(i);

            Invoke(nameof(TurnLayoutOn), .025f);
        }
        else
        {
            TurnLayoutOn();

            for (int i = 0; i < M_Habits.singleton.habitList.Count; i++)
                M_Habits.singleton.habitList[i].transform.SetSiblingIndex(i);

            Invoke(nameof(TurnLayoutOff), .025f);
        }
    }

    #endregion
}
