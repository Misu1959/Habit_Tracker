using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

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
        DisplayData();
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

    private void DisplayData()
    {
        M_UI_ColorPicker.singleton.ChangeSelectedColor(habitToDisplay.data.color);
        M_UI_Main.singleton.panelDisplayHabit.GetComponent<PageColorizer>().Colorize(habitToDisplay.data.color);

        DisplayHabitName();
        DisplayHabitInfo();

        DisplayTypeDropdowns();


        DisplayComparisonPage();
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
        => dropdownComparisonType.onValueChanged.AddListener((int option) => RefreshComparisonPage((StatusType)option));


    private void DisplayComparisonPage()
    {
        dropdownComparisonType.value = 0;

        DisplayComparisonStreak(StatusType.Completion);
        DisplayComparisonDay(StatusType.Completion);
        DisplayComparisonWeek(StatusType.Completion);
        DisplayComparisonMonth(StatusType.Completion);
        DisplayComparisonYear(StatusType.Completion);
    }
    private void RefreshComparisonPage(StatusType statusType)
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

    Dictionary<string, int> monthsIndex = new Dictionary<string, int>()
    {
        { "January",0 },
        {"February",1 },
        {"March",2    },
        {"April",3    },
        {"May",4      },
        {"June",5     },
        {"July",6     },
        {"August",7   },
        {"September",8},
        {"October",9  },
        {"November",10},
        {"December",11}
    };


    private DateTime calendarDate = new DateTime();

    [Header("\tCalendar")]
    [SerializeField] private TMP_Dropdown dropdownCalendarMonth;
    [SerializeField] private TMP_Dropdown dropdownCalendarYear;
    [SerializeField] private Button buttonPrevMonth;
    [SerializeField] private Button buttonNextMonth;


    [SerializeField] private Transform calendar;
    [SerializeField] private TextMeshProUGUI textNoteStatus;
    [SerializeField] private TextMeshProUGUI textNote;

    int prevMonth;

    private void SetCalendarPage()
    {
        SetDropdownMonth();
        SetDropdownYear();
        SetButtonPrevMonth();
        SetButtonNextMonth();
    }

    private void SetDropdownYear()
        => dropdownCalendarYear.onValueChanged.AddListener(OnYearChanged);
    private void SetDropdownMonth()
        => dropdownCalendarMonth.onValueChanged.AddListener(OnMonthChanged);

    private void SetButtonPrevMonth()
        => buttonPrevMonth.onClick.AddListener(PreviousMonth);
    private void SetButtonNextMonth()
        => buttonNextMonth.onClick.AddListener(NextMonth);

    private void DisplayCalendarPage()
    {
        calendarDate = DateTime.Today;

        PopulateYearDropdown();
        PopulateMonthDropdown();

        RefreshCalendarPage();

        textNote.text = "";
        textNoteStatus.text = "";
    }


    private void RefreshCalendarPage()
    {
        DisplayCalendarButtonPrevMonth();
        DisplayCalendarButtonNextMonth();
        DisplayCalendar();
    }

    void PopulateYearDropdown()
    {
        dropdownCalendarYear.ClearOptions();
        List<string> options = new List<string>();

        
        int currentYear = DateTime.Now.Year;
        for (int y = habitToDisplay.data.creationDate.Year; y <= currentYear; y++)
            options.Add(y.ToString());

        dropdownCalendarYear.AddOptions(options);

        // Select current year
        int index = options.IndexOf(calendarDate.Year.ToString());
        dropdownCalendarYear.SetValueWithoutNotify(index);
        dropdownCalendarYear.RefreshShownValue();
    }

    void PopulateMonthDropdown()
    {
        dropdownCalendarMonth.ClearOptions();
        List<string> months = new List<string>()
        {
            "January","February","March","April","May","June",
            "July","August","September","October","November","December"
        };

        int selectedYear = int.Parse(dropdownCalendarYear.options[dropdownCalendarYear.value].text);
        int currentYear = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;

        if (selectedYear == habitToDisplay.data.creationDate.Year && selectedYear == currentYear)
            months = months.GetRange(habitToDisplay.data.creationDate.Month - 1, currentMonth - (habitToDisplay.data.creationDate.Month - 1));
        else if (selectedYear == habitToDisplay.data.creationDate.Year)
            months = months.GetRange(habitToDisplay.data.creationDate.Month - 1, 12 - (habitToDisplay.data.creationDate.Month - 1));
        else if (selectedYear == currentYear)
            months = months.GetRange(0, currentMonth);

        dropdownCalendarMonth.AddOptions(months);

        int selectedMonth = calendarDate.Month;
        int newIndex;

        if (selectedYear == habitToDisplay.data.creationDate.Year)
            newIndex = selectedMonth - habitToDisplay.data.creationDate.Month;
        else
            newIndex = selectedMonth - 1; 

        newIndex = Mathf.Clamp(newIndex, 0, months.Count - 1);

        dropdownCalendarMonth.SetValueWithoutNotify(newIndex);
        dropdownCalendarMonth.RefreshShownValue();
    }



    void OnYearChanged(int index)
    {
        int year = int.Parse(dropdownCalendarYear.options[index].text);

        calendarDate = new DateTime(year, calendarDate.Month, 1);

        PopulateMonthDropdown();
        UpdateDateFromDropdown();
    }

    void OnMonthChanged(int index)
    {
        UpdateDateFromDropdown();
    }

    void UpdateDateFromDropdown()
    {
        int year = int.Parse(dropdownCalendarYear.options[dropdownCalendarYear.value].text);
        int month = monthsIndex[dropdownCalendarMonth.options[dropdownCalendarMonth.value].text] + 1;

        calendarDate = new DateTime(year, month, 1);
     
        RefreshCalendarPage();
    }

    void PreviousMonth()
    {
        calendarDate = calendarDate.AddMonths(-1);
        SetDropdownsFromDate();
    }

    void NextMonth()
    {
        calendarDate = calendarDate.AddMonths(1);
        SetDropdownsFromDate();
    }



    void SetDropdownsFromDate()
    {
        int yearIndex = dropdownCalendarYear.options.FindIndex(o => o.text == calendarDate.Year.ToString());
        dropdownCalendarYear.SetValueWithoutNotify(yearIndex);

        PopulateMonthDropdown();


        int month = calendarDate.Month;
        if (calendarDate.Year == habitToDisplay.data.creationDate.Year)
            month = month - (12 - dropdownCalendarMonth.options.Count);

        dropdownCalendarMonth.SetValueWithoutNotify(month - 1);
        dropdownCalendarMonth.RefreshShownValue();

        RefreshCalendarPage();
    }



    private void DisplayCalendarButtonPrevMonth()
    {
        bool pass = M_Date.singleton.StartOfMonth(calendarDate) > M_Date.singleton.StartOfMonth(habitToDisplay.data.creationDate);
        buttonPrevMonth.gameObject.SetActive(pass);
    }
    private void DisplayCalendarButtonNextMonth()
    {
        bool pass = M_Date.singleton.StartOfMonth(calendarDate) < M_Date.singleton.startOfCurrentMonth;
        buttonNextMonth.gameObject.SetActive(pass);
    }




    private void DisplayCalendar()
    {
        DateTime prevCalendarPage = M_Date.singleton.StartOfMonth(calendarDate.AddMonths(-1));
        DateTime nextCalendarPage = M_Date.singleton.StartOfMonth(calendarDate.AddMonths(1));

        int daysInMonth = DateTime.DaysInMonth(calendarDate.Year, calendarDate.Month);
        int daysInPrevMonth = DateTime.DaysInMonth(prevCalendarPage.Year, prevCalendarPage.Month);


        int start       = (int)M_Date.singleton.StartOfMonth(calendarDate).DayOfWeek;
        int prevStart   = daysInPrevMonth - start + 1;

        int cellCounter = 0;

        for (int dayNum = 0; dayNum < start; dayNum++)
            SetDayButton(cellCounter++, new DateTime(prevCalendarPage.Year, prevCalendarPage.Month, prevStart + dayNum), true);

        for (int dayNum = 1; dayNum <= daysInMonth; dayNum++)
            SetDayButton(cellCounter++, new DateTime(calendarDate.Year, calendarDate.Month, dayNum), false);

        for (int dayNum = 1; dayNum <= 42 - (daysInMonth + start); dayNum++)
            SetDayButton(cellCounter++, new DateTime(nextCalendarPage.Year, nextCalendarPage.Month, dayNum), true);
    }
    private void DisplayNote(DateTime date, int completion, float value)
    {
        string completionStatus = completion == 1 ? " you achieved your goal." : " you didn't achieved your goal.";
        string valueStatus = habitToDisplay.data.type == HabitType.yesOrNo ? "" : value + " / " + habitToDisplay.data.targetAmount + habitToDisplay.data.unit;


        if (date < habitToDisplay.data.creationDate)
        {
            textNoteStatus.text = "You started this habit on " + habitToDisplay.data.creationDate.ToString("%d MMM yyyy");
            textNote.text = "";
        }
        else if(date > M_Date.singleton.today)
        {
            textNoteStatus.text = "I wish you will achieve your goal on " + date.ToString("%d MMM yyyy");
            textNote.text = "";
        }
        else
        {
            textNoteStatus.text = "In " + date.ToString("%d MMM yyyy") + completionStatus + "\n" + valueStatus;
            textNote.text = M_SaveLoad.LoadHabitNote(habitToDisplay.data.name, date);
        }
    }
    private void SetDayButton(int cell, DateTime date, bool isOtherMonth)
    {
        int column = cell % 7;
        int row = (cell / 7) % 7;

        Transform day = calendar.GetChild(row).GetChild(column);

        M_SaveLoad.LoadHabitDay(habitToDisplay.data.name, date, out int completion, out float value);


        Color dayColor = (completion == 0) ? new Color(232, 232, 232) : habitToDisplay.data.color;
        
        if(date < habitToDisplay.data.creationDate)
            dayColor.a = .25f;
        else if(date > M_Date.singleton.today)
            dayColor.a = .25f;
        else
            dayColor.a = isOtherMonth ? .5f : 1f;

        day.GetComponent<Image>().color = dayColor;
        
        day.GetComponent<Button>().onClick.RemoveAllListeners();
        day.GetComponent<Button>().onClick.AddListener(() => DisplayNote(date, completion, value));
    
        day.GetComponentInChildren<TextMeshProUGUI>().text = date.Day.ToString(); 
    }

    #endregion

}
