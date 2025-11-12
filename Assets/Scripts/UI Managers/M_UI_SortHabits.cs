using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using static UnityEditor.Progress;


public enum SortingType
{
    manually,
    byName,
    byType,
    byColor,
    byStatus
};

public enum SortingWay
{
    none,
    both,
    up,
    down
};



public class M_UI_SortHabits : MonoBehaviour
{
    public static M_UI_SortHabits singleton;

    [Header("Sprites")]
    [SerializeField] private Sprite spriteSortBoth; 
    [SerializeField] private Sprite spriteSortUp; 
    [SerializeField] private Sprite spriteSortDown;


    [Header("\nIn scene objects")]
    [SerializeField] private Image imageSelectedSortionType;

    [SerializeField] private Button buttonSortManually;
    [SerializeField] private Button buttonSortByName;
    [SerializeField] private Button buttonSortByType;
    [SerializeField] private Button buttonSortByColor;
    [SerializeField] private Button buttonSortByStatus;


    private SortingType sortType;
    private SortingWay sortWay;

    private void Awake() => Initialize();

    IEnumerator Start()
    {
        Setup();

        yield return null;

        M_SaveLoad.LoadSortingData(out SortingType loadedSortType, out SortingWay loadedSortWay);

        ChangeSortingData(loadedSortType, loadedSortWay);
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
        SetButtonSortManually();
        SetButtonSortByName();
        SetButtonSortByType();
        SetButtonSortByColor();
        SetButtonSortByStatus();
    }


    private void SetButtonSortManually()
    {
        buttonSortManually.onClick.AddListener(M_UI_Main.singleton.CloseSortHabitsMenu);
        buttonSortManually.onClick.AddListener(() => ChangeSortingData(SortingType.manually));
    }


    private void SetButtonSortByName()
    {
        buttonSortByName.onClick.AddListener(M_UI_Main.singleton.CloseSortHabitsMenu);
        buttonSortByName.onClick.AddListener(() => ChangeSortingData(SortingType.byName));
    }

    private void SetButtonSortByType()
    {
        buttonSortByType.onClick.AddListener(M_UI_Main.singleton.CloseSortHabitsMenu);
        buttonSortByType.onClick.AddListener(() => ChangeSortingData(SortingType.byType));
    }

    private void SetButtonSortByColor()
    {
        buttonSortByColor.onClick.AddListener(M_UI_Main.singleton.CloseSortHabitsMenu);
        buttonSortByColor.onClick.AddListener(() => ChangeSortingData(SortingType.byColor));
    }

    private void SetButtonSortByStatus()
    {
        buttonSortByStatus.onClick.AddListener(M_UI_Main.singleton.CloseSortHabitsMenu);
        buttonSortByStatus.onClick.AddListener(() => ChangeSortingData(SortingType.byStatus));
    }



    public void ChangeSortingData(SortingType newSortingType, SortingWay newSortingWay = SortingWay.none)
    {
        ChangeSortingWay(newSortingType, newSortingWay);
        ChangeSortingType(newSortingType);

        Sort();
    }
    private void ChangeSortingType(SortingType newSortingType)
    {
        sortType = newSortingType;

        imageSelectedSortionType.transform.SetParent(buttonSortManually.transform.parent.GetChild((int)sortType));
        imageSelectedSortionType.transform.localPosition = new Vector2(imageSelectedSortionType.transform.localPosition.x, 0);
    }
    private void ChangeSortingWay(SortingType newSortingType, SortingWay newSortingWay)
    {
        if (newSortingWay == SortingWay.none)
        {
            if (newSortingType == SortingType.manually)
                sortWay = SortingWay.both;
            else if (newSortingType != sortType)
                sortWay = SortingWay.up;
            else
                sortWay = (sortWay == SortingWay.up) ? SortingWay.down : SortingWay.up;
        }
        else
            sortWay = newSortingWay;

        switch (sortWay)
        {
            case SortingWay.both:
                imageSelectedSortionType.sprite = spriteSortBoth;
                break;
            case SortingWay.up:
                imageSelectedSortionType.sprite = spriteSortUp;
                break;
            case SortingWay.down:
                imageSelectedSortionType.sprite = spriteSortDown;
                break;
        }
    }



    public void Sort()
    {
        switch(sortType)
        {
            case SortingType.manually:
                break;
            case SortingType.byName:
                SortByName();
                break;
            case SortingType.byType:
                SortByType();
                break;
            case SortingType.byColor:
                SortByColor();
                break;
            case SortingType.byStatus:
                SortByStatus();
                break;
        }
        StartCoroutine(M_UI_Main.singleton.RefreshLayout());

        M_SaveLoad.SaveSortingData(sortType, sortWay);

        foreach (Habit habit in M_Habits.singleton.habitList)
            M_SaveLoad.SaveHabitName(habit);
    }


    private void SortByName()
    {

        if(sortWay == SortingWay.up)
            M_Habits.singleton.habitList = M_Habits.singleton.habitList.OrderBy(habit => habit.data.name).ToList();
        else
            M_Habits.singleton.habitList = M_Habits.singleton.habitList.OrderByDescending(habit => habit.data.name).ToList();
    }

    private void SortByType()
    {

        if (sortWay == SortingWay.up)
            M_Habits.singleton.habitList.Sort((habit1, habit2) => CompareType(habit1, habit2, false));
        else
            M_Habits.singleton.habitList.Sort((habit1, habit2) => CompareType(habit2, habit1, true));
    }


    private void SortByColor()
    {
        if (sortWay == SortingWay.up)
            M_Habits.singleton.habitList.Sort((habit1, habit2) => CompareBrightness(habit1, habit2, false));
        else
            M_Habits.singleton.habitList.Sort((habit1, habit2) => CompareBrightness(habit2, habit1, true));
    }

    private void SortByStatus()
    {
        if (sortWay == SortingWay.up)
            M_Habits.singleton.habitList.Sort((habit1, habit2) => CompareStatus(habit1, habit2, false));
        else
            M_Habits.singleton.habitList.Sort((habit1, habit2) => CompareStatus(habit2, habit1, true));
    }



    private int CompareType(Habit habit1, Habit habit2,bool reverse)
    {
        int valueComparison = habit1.data.type.CompareTo(habit2.data.type);

        if (valueComparison == 0)
        {
            if (reverse)
                return habit2.data.name.CompareTo(habit1.data.name);
            else
                return habit1.data.name.CompareTo(habit2.data.name);
        }

        return valueComparison;
    }


    private int CompareBrightness(Habit habit1, Habit habit2,bool reverse)
    {
        float brightness1 = 0.2126f * habit1.data.color.r + 0.7152f * habit1.data.color.g + 0.0722f * habit1.data.color.b;
        float brightness2 = 0.2126f * habit2.data.color.r + 0.7152f * habit2.data.color.g + 0.0722f * habit2.data.color.b;

        int valueComparison =  brightness2.CompareTo(brightness1);

        if (valueComparison == 0)
        {
            if (reverse)
                return habit2.data.name.CompareTo(habit1.data.name);
            else
                return habit1.data.name.CompareTo(habit2.data.name);
        }

        return valueComparison;
    }


    private int CompareStatus(Habit habit1, Habit habit2, bool reverse)
    {
        int valueComparison = habit1.data.completionValue.CompareTo(habit2.data.completionValue);

        if (valueComparison == 0)
        {
            if(reverse)
                return habit2.data.name.CompareTo(habit1.data.name);
            else
                return habit1.data.name.CompareTo(habit2.data.name);
        }
        
        return valueComparison;
    }



}
