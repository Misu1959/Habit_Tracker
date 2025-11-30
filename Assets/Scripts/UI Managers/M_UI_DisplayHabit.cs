using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using Unity.Mathematics;

public class M_UI_DisplayHabit : MonoBehaviour
{
    private enum StatusType
    {
        Completion,
        Value
    }


    public static M_UI_DisplayHabit singleton;

    private Habit habitToDisplay;


    [Header("Intro Panel")]
    [SerializeField] private Button buttonGoBack;
    [SerializeField] private Button buttonRecolorHabit;
    [SerializeField] private Button buttonDeleteHabit;

    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textStreak;
    [SerializeField] private TextMeshProUGUI textTotalCompletions;
    [SerializeField] private TextMeshProUGUI textTotalValue;



    [SerializeField] private ScrollRect scrollRectPages;

    public Habit GetDisplayHabit() => habitToDisplay;
    public void SetHabitToDisplay(Habit newHabitToDisplay)
    {
        habitToDisplay = newHabitToDisplay;
        RefreshDisplayData();
    }

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
        SetButtonGoBack();
        SetButtonRecolorHabit();
        SetButtonDeleteHabit();

        SetComparisonPage();
        SetHistoryPage();
        SetCalendarPage();
    }



    #region Setup

    private void SetButtonGoBack()
    {
        buttonGoBack.onClick.AddListener(M_UI_Main.singleton.CloseDisplayHabitMenu);
        buttonGoBack.onClick.AddListener(() => scrollRectPages.horizontalNormalizedPosition = 0);
    }

    private void SetButtonRecolorHabit()
    {
        buttonRecolorHabit.onClick.AddListener(M_UI_Main.singleton.OpenColorPickerMenu);
    }

    private void SetButtonDeleteHabit()
    {
        buttonDeleteHabit.onClick.AddListener(() => M_Habits.singleton.DestroyHabit(habitToDisplay));
        buttonDeleteHabit.onClick.AddListener(M_UI_Main.singleton.CloseDisplayHabitMenu);
        buttonDeleteHabit.onClick.AddListener(() => StartCoroutine(M_UI_Main.singleton.RefreshLayout()));

    }



    #endregion
    
    #region Methods

    private void RefreshDisplayData()
    {
        M_UI_ColorPicker.singleton.ChangeSelectedColor(habitToDisplay.data.color);
        M_UI_Main.singleton.panelDisplayHabit.GetComponent<PageColorizer>().Colorize(habitToDisplay.data.color);

        dropdownComparisonType.value = 0;
        calendarDate = DateTime.Today;



        DisplayHabitName();
        DisplayHabitInfo();

        DisplayTypeDropdowns();


        DisplayComparisonPage(StatusType.Completion);
        DisplayHistoryPage();
        DisplayCalendarPage();


    }

    private void DisplayHabitName() => textName.text = habitToDisplay.data.name;

    private void DisplayHabitInfo()
    {
        M_SaveLoad.LoadHabitStatsTotal(habitToDisplay.data.name, out int totalCompletions, out float totalValue);
        M_SaveLoad.LoadHabitStreak(habitToDisplay.data.name, out DateTime startDate, out DateTime endDate, out int streakCompletion, out float streakValue);

        
        textStreak.text = "0";
        textTotalCompletions.text = totalCompletions.ToString();
        textTotalValue.text = totalValue.ToString();

    }


    private void DisplayTypeDropdowns()
    {
        if(habitToDisplay.data.type == HabitType.yesOrNo)
        {
            dropdownComparisonType.gameObject.SetActive(false);
            dropdownHistoryType.gameObject.SetActive(false);

            textTotalValue.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            dropdownComparisonType.gameObject.SetActive(true);
            dropdownHistoryType.gameObject.SetActive(true);

            textTotalValue.transform.parent.gameObject.SetActive(true);
        }
    }



    #endregion



    #region Stats Comparison 

    [Header("\tComparison page")]
    [SerializeField]  private TMP_Dropdown dropdownComparisonType;
    
    [SerializeField] private Transform comparisonStreak;
    [SerializeField] private Transform comparisonDay;
    [SerializeField] private Transform comparisonWeek;
    [SerializeField] private Transform comparisonMonth;
    [SerializeField] private Transform comparisonYear;


    private void SetComparisonPage()
    {
        SetDropdownComparisonType();
    }

    private void SetDropdownComparisonType()
        => dropdownComparisonType.onValueChanged.AddListener((int option) => DisplayComparisonPage((StatusType)option));


    private void DisplayComparisonPage(StatusType statusType)
    {
        DisplayComparisonStreak(statusType);
        DisplayComparisonDay(statusType);
        DisplayComparisonWeek(statusType);
        DisplayComparisonMonth(statusType);
        DisplayComparisonYear(statusType);
    }

    private void DisplayComparisonStreak(StatusType statusType)
    {
        M_SaveLoad.LoadHabitStreak(habitToDisplay.data.name, out DateTime streakStartDate, out DateTime streakEndDate, out int streakCompletions, out float streakValue);
        M_SaveLoad.LoadHabitBestStreak(habitToDisplay.data.name, out DateTime bestStreakStartDate, out DateTime bestStreakEndDate, out int bestStreakCompletions, out float bestStreakValue);

        if (statusType == StatusType.Completion)
        {
            DisplayStats(comparisonStreak.GetChild(0), streakStartDate, streakEndDate, streakCompletions, bestStreakCompletions, M_Date.DAY_FORMAT);
            DisplayStats(comparisonStreak.GetChild(1), bestStreakStartDate, bestStreakEndDate, bestStreakCompletions, bestStreakCompletions, M_Date.DAY_FORMAT);
        }
        else
        {
            DisplayStats(comparisonStreak.GetChild(0), streakStartDate, streakEndDate, streakValue, bestStreakValue, M_Date.DAY_FORMAT);
            DisplayStats(comparisonStreak.GetChild(1), bestStreakStartDate, bestStreakEndDate, bestStreakValue, bestStreakValue, M_Date.DAY_FORMAT);
        }
    }
    private void DisplayComparisonDay(StatusType statusType)
    {
        M_SaveLoad.LoadHabitDay(habitToDisplay.data.name, M_Date.singleton.today, out int dayCompletion, out float dayValue);
        M_SaveLoad.LoadHabitBestDay(habitToDisplay.data.name, out DateTime bestDay, out int bestDayCompletion, out float bestDayValue);

        if (statusType == StatusType.Completion)
        {
            DisplayStats(comparisonDay.GetChild(0), M_Date.singleton.today, M_Date.singleton.today.AddDays(1), dayCompletion, bestDayCompletion, M_Date.DAY_FORMAT);
            DisplayStats(comparisonDay.GetChild(1), bestDay, bestDay.AddDays(1), bestDayCompletion, bestDayCompletion, M_Date.DAY_FORMAT);
        }
        else
        {
            DisplayStats(comparisonDay.GetChild(0), M_Date.singleton.today, M_Date.singleton.today.AddDays(1), dayValue, bestDayValue, M_Date.DAY_FORMAT);
            DisplayStats(comparisonDay.GetChild(1), bestDay, bestDay.AddDays(1), bestDayValue, bestDayValue, M_Date.DAY_FORMAT);
        }
    }
    private void DisplayComparisonWeek(StatusType statusType)
    {
        M_SaveLoad.LoadHabitWeek(habitToDisplay.data.name, M_Date.singleton.today, out int weekCompletions, out float weekValue);
        M_SaveLoad.LoadHabitBestWeek(habitToDisplay.data.name, out DateTime bestWeekStartDate, out int bestWeekCompletions, out float bestWeekValue);

        if (statusType == StatusType.Completion)
        {
            DisplayStats(comparisonWeek.GetChild(0), M_Date.singleton.startOfCurrentWeek, M_Date.singleton.startOfCurrentWeek.AddDays(7), weekCompletions, bestWeekCompletions, M_Date.DAY_FORMAT);
            DisplayStats(comparisonWeek.GetChild(1), bestWeekStartDate, bestWeekStartDate.AddDays(7), bestWeekCompletions, bestWeekCompletions, M_Date.DAY_FORMAT);
        }
        else
        {
            DisplayStats(comparisonWeek.GetChild(0), M_Date.singleton.startOfCurrentWeek, M_Date.singleton.startOfCurrentWeek.AddDays(7), weekValue, bestWeekValue, M_Date.DAY_FORMAT);
            DisplayStats(comparisonWeek.GetChild(1), bestWeekStartDate, bestWeekStartDate.AddDays(7), bestWeekValue, bestWeekValue, M_Date.DAY_FORMAT);
        }
    }
    private void DisplayComparisonMonth(StatusType statusType)
    {
        M_SaveLoad.LoadHabitMonth(habitToDisplay.data.name, M_Date.singleton.today, out int monthCompletions, out float monthValue);
        M_SaveLoad.LoadHabitBestMonth(habitToDisplay.data.name, out DateTime bestMonthStartDate, out int bestMonthCompletions, out float bestMonthValue);

        if (statusType == StatusType.Completion)
        {
            DisplayStats(comparisonMonth.GetChild(0), M_Date.singleton.startOfCurrentMonth, M_Date.singleton.startOfCurrentMonth.AddMonths(1), monthCompletions, bestMonthCompletions, M_Date.MONTH_FORMAT);
            DisplayStats(comparisonMonth.GetChild(1), bestMonthStartDate, bestMonthStartDate.AddMonths(1), bestMonthCompletions, bestMonthCompletions, M_Date.MONTH_FORMAT);
        }
        else
        {
            DisplayStats(comparisonMonth.GetChild(0), M_Date.singleton.startOfCurrentMonth, M_Date.singleton.startOfCurrentMonth.AddMonths(1), monthValue, bestMonthValue, M_Date.MONTH_FORMAT);
            DisplayStats(comparisonMonth.GetChild(1), bestMonthStartDate, bestMonthStartDate.AddMonths(1), bestMonthValue, bestMonthValue, M_Date.MONTH_FORMAT);
        }
    
    }

    private void DisplayComparisonYear(StatusType statusType)
    {
        M_SaveLoad.LoadHabitYear(habitToDisplay.data.name, M_Date.singleton.today, out int yearCompletions, out float yearValue);
        M_SaveLoad.LoadHabitBestYear(habitToDisplay.data.name, out DateTime bestYearStartDate, out int bestYearCompletions, out float bestYearValue);

        if (statusType == StatusType.Completion)
        {
            DisplayStats(comparisonYear.GetChild(0), M_Date.singleton.startOfCurrentYear, M_Date.singleton.startOfCurrentYear.AddYears(1), yearCompletions, bestYearCompletions, M_Date.YEAR_FORMAT);
            DisplayStats(comparisonYear.GetChild(1), bestYearStartDate, bestYearStartDate.AddYears(1), bestYearCompletions, bestYearCompletions, M_Date.YEAR_FORMAT);
        }
        else
        {
            DisplayStats(comparisonYear.GetChild(0), M_Date.singleton.startOfCurrentYear, M_Date.singleton.startOfCurrentYear.AddYears(1), yearValue, bestYearValue, M_Date.YEAR_FORMAT);
            DisplayStats(comparisonYear.GetChild(1), bestYearStartDate, bestYearStartDate.AddYears(1), bestYearValue, bestYearValue, M_Date.YEAR_FORMAT);
        }
    }



    private void DisplayStats(Transform stats, DateTime startDate, DateTime endDate, float status,float maxStatus, string dateFormat)
    {
        stats.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = (maxStatus == 0) ? 1 : status / maxStatus;
        stats.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = status.ToString();

        stats.GetChild(1).GetComponent<TextMeshProUGUI>().text = startDate.ToString(dateFormat);
        stats.GetChild(2).GetComponent<TextMeshProUGUI>().text = endDate.ToString(dateFormat);
    }

    #endregion


    #region History
    [Header("\tComparison page")]
    [SerializeField] private TMP_Dropdown dropdownHistoryType;

    private void SetHistoryPage()
    {
    }

    private void DisplayHistoryPage()
    {

    }

    #endregion


    #region Calendar

    private DateTime calendarDate = new DateTime();

    [Header("\tCalendar")]
    [SerializeField] private TMP_Dropdown dropdownCalendarMonth;
    [SerializeField] private TMP_Dropdown dropdownCalendarYear;
    [SerializeField] private Button buttonPrevMonth;
    [SerializeField] private Button buttonNextMonth;


    private void SetCalendarPage()
    {
        SetDropdownMonth();
        SetDropdownYear();
        SetButtonPrevMonth();
        SetButtonNextMonth();
    }

    private void SetDropdownMonth()
        => dropdownCalendarMonth.onValueChanged.AddListener((int month) => ChangeMonth(month + 1));
    private void SetDropdownYear()
        => dropdownCalendarYear.onValueChanged.AddListener((int year) => ChangeYear(year));
    private void SetButtonPrevMonth()
        => buttonPrevMonth.onClick.AddListener(() => ChangeMonth(calendarDate.Month - 1));
    private void SetButtonNextMonth()
        => buttonNextMonth.onClick.AddListener(() => ChangeMonth(calendarDate.Month + 1));


    private void ChangeMonth(int newMonth)
    {
        calendarDate = new DateTime(calendarDate.Year, newMonth, calendarDate.Day);
        DisplayCalendarPage();
    }

    private void ChangeYear(int newYear)
    {
        calendarDate = new DateTime(newYear, calendarDate.Month, calendarDate.Day);
        DisplayCalendarPage();
    }


    private void DisplayCalendarPage()
    {
        DisplayCalendarDropdowns();
        DisplayCalendarButtons();
    }

    private void DisplayCalendarDropdowns()
    {
        dropdownCalendarMonth.value = calendarDate.Month  - 1;
        //dropdownCalendarYear.value = calendarDate.Year;
    }
    

    private void DisplayCalendarButtons()
    {
        DateTime creationDate = M_SaveLoad.LoadHabitCreationDate(habitToDisplay.data.name);

        if(calendarDate.Month > creationDate.Month && calendarDate.Year >= creationDate.Year)
            buttonPrevMonth.gameObject.SetActive(true);
        else
            buttonPrevMonth.gameObject.SetActive(false);

        if (calendarDate.Month < creationDate.Month && calendarDate.Year <= creationDate.Year)
            buttonNextMonth.gameObject.SetActive(true);
        else
            buttonNextMonth.gameObject.SetActive(false);
    }



    #endregion

}
