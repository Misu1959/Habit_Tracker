using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M_UI_Main : MonoBehaviour
{
    public static M_UI_Main singleton;

    [Header("In scene objects")]
    [SerializeField] private Button buttonOpenCreateHabitMenu;
    [SerializeField] private Button buttonOpenSorHabitsMenu;


    [SerializeField] private Transform weekDays;
    [SerializeField] private Transform displayToday;

    [field:SerializeField] public Transform panelHabits { get; private set; }

    [field: SerializeField] public Transform panelCreateHabit { get; private set; }
    [field: SerializeField] public Transform panelDisplayHabit { get; private set; }
    [field: SerializeField] public Transform panelUpdateHabit { get; private set; }

    [field: SerializeField] public Transform panelSortHabits { get; private set; }
    [field:SerializeField] public  Transform panelColorPicker { get; private set; }





    private void Awake()
    {
        Application.targetFrameRate = 60;
        Initialize();
    }

    void Start()
    {
        Setup();
        DisplayDaysOfWeek();


    }

    private void Initialize()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(this);
    }

    private void Setup()
    {
        panelUpdateHabit.GetComponent<ClickHandler>().action = CloseUpdateHabitMenu;
        panelColorPicker.GetComponent<ClickHandler>().action = CloseColorPickerMenu;
        panelSortHabits.GetComponent<ClickHandler>().action = CloseSortHabitsMenu;

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



    public void OpenDisplayHabitMenu() => panelDisplayHabit.gameObject.SetActive(true);
    public void CloseDisplayHabitMenu() => panelDisplayHabit.gameObject.SetActive(false);

    public void OpenUpdateHabitMenu() => panelUpdateHabit.gameObject.SetActive(true);
    public void CloseUpdateHabitMenu() => panelUpdateHabit.gameObject.SetActive(false);



    public void OpenSortHabitsMenu() => panelSortHabits.gameObject.SetActive(true);
    public void CloseSortHabitsMenu() => panelSortHabits.gameObject.SetActive(false);

    public void OpenColorPickerMenu() => panelColorPicker.gameObject.SetActive(true);
    public void CloseColorPickerMenu() => panelColorPicker.gameObject.SetActive(false);







    private void DisplayDaysOfWeek()
    {
        for (int i = 0; i < 7; i++)
        {
            DateTime dayOfWeek = M_Date.singleton.startOfWeek.AddDays(i);
            weekDays.GetChild(i).GetComponent<TextMeshProUGUI>().text = dayOfWeek.ToString("ddd") + "\n" + dayOfWeek.ToString("dd");

            if(dayOfWeek == M_Date.singleton.today)
            {
                displayToday.SetParent(weekDays.GetChild(i));
                displayToday.localPosition = Vector3.zero;
            }
        }
    }




    public void TurnLayoutOn()
    {
        panelHabits.GetComponent<ContentSizeFitter>().enabled = true;
        panelHabits.GetComponent<VerticalLayoutGroup>().enabled = true;
    }
    public void TurnLayoutOff()
    {
        panelHabits.GetComponent<VerticalLayoutGroup>().enabled = false;
        panelHabits.GetComponent<ContentSizeFitter>().enabled = false;
    }

    public IEnumerator RefreshLayout()
    {
        if (panelHabits.GetComponent<VerticalLayoutGroup>().enabled)
        {
            TurnLayoutOff();


            for (int i = 0; i < M_Habits.singleton.habitList.Count; i++)
                M_Habits.singleton.habitList[i].transform.SetSiblingIndex(i);

            yield return null;
            TurnLayoutOn();
        }
        else
        {
            TurnLayoutOn();

            for (int i = 0; i < M_Habits.singleton.habitList.Count; i++)
                M_Habits.singleton.habitList[i].transform.SetSiblingIndex(i);

            yield return null;
            TurnLayoutOff();
        }
    }

    #endregion
}
