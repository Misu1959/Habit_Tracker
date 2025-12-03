using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class M_UI_DisplayHabit : MonoBehaviour
{
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

        DisplayComparisonPage();
        DisplayHistoryPage();
        DisplayCalendarPage();
    }

    private void DisplayHabitName() => textName.text = habitToDisplay.data.name;

    private void DisplayHabitInfo()
    {
        M_SaveLoad.LoadHabitStatsTotal(habitToDisplay.data.name, out int totalCompletions, out float totalValue);
        M_SaveLoad.LoadHabitStreak(habitToDisplay.data.name, out DateTime startDate, out DateTime endDate, out int streakCompletion, out float streakValue);



        textStreak.text = streakCompletion.ToString();

        textTotalCompletions.text = totalCompletions.ToString();
        textTotalValue.text = totalValue.ToString();

        textTotalValue.transform.parent.gameObject.SetActive(habitToDisplay.data.type == HabitType.measurable);
    }

    #endregion



    #region Stats Comparison 

    [Header("\tComparison page")]
    
    [SerializeField] private Transform comparisonStreak;
    [SerializeField] private Transform comparisonDay;
    [SerializeField] private Transform comparisonWeek;
    [SerializeField] private Transform comparisonMonth;
    [SerializeField] private Transform comparisonYear;

    private void DisplayComparisonPage()
    {
        DisplayComparisonStreak();
        DisplayComparisonDay();
        DisplayComparisonWeek();
        DisplayComparisonMonth();
        DisplayComparisonYear();
    }

    private void DisplayComparisonStreak()
    {
        M_SaveLoad.LoadHabitStreak(habitToDisplay.data.name, out DateTime streakStartDate, out DateTime streakEndDate, out int streakCompletions, out float streakValue);
        M_SaveLoad.LoadHabitBestStreak(habitToDisplay.data.name, out DateTime bestStreakStartDate, out DateTime bestStreakEndDate, out int bestStreakCompletions, out float bestStreakValue);



        comparisonStreak.GetChild(0).GetComponentInChildren<HabitStats>().DisplayStats(habitToDisplay.data.type,streakCompletions, streakValue, M_Date.DAY_FORMAT, streakStartDate, streakEndDate);
        comparisonStreak.GetChild(1).GetComponentInChildren<HabitStats>().DisplayStats(habitToDisplay.data.type, bestStreakCompletions, bestStreakValue, M_Date.DAY_FORMAT, bestStreakStartDate, bestStreakEndDate);
    }
    private void DisplayComparisonDay()
    {
        M_SaveLoad.LoadHabitDay(habitToDisplay.data.name, M_Date.singleton.today, out int dayCompletion, out float dayValue);
        M_SaveLoad.LoadHabitBestDay(habitToDisplay.data.name, out DateTime bestDay, out int bestDayCompletion, out float bestDayValue);

        comparisonDay.GetChild(0).GetComponentInChildren<HabitStats>().DisplayStats(habitToDisplay.data.type, dayCompletion, dayValue, M_Date.DAY_FORMAT, M_Date.singleton.today);
        comparisonDay.GetChild(1).GetComponentInChildren<HabitStats>().DisplayStats(habitToDisplay.data.type, bestDayCompletion, bestDayValue, M_Date.DAY_FORMAT, bestDay);
    }
    private void DisplayComparisonWeek()
    {
        M_SaveLoad.LoadHabitWeek(habitToDisplay.data.name, M_Date.singleton.today, out int weekCompletions, out float weekValue);
        M_SaveLoad.LoadHabitBestWeek(habitToDisplay.data.name, out DateTime bestWeekStartDate, out int bestWeekCompletions, out float bestWeekValue);

        comparisonWeek.GetChild(0).GetComponentInChildren<HabitStats>().DisplayStats(habitToDisplay.data.type, weekCompletions, weekValue, M_Date.DAY_FORMAT, M_Date.singleton.startOfCurrentWeek, M_Date.singleton.startOfCurrentWeek.AddDays(7));
        comparisonWeek.GetChild(1).GetComponentInChildren<HabitStats>().DisplayStats(habitToDisplay.data.type, bestWeekCompletions, bestWeekValue, M_Date.DAY_FORMAT, bestWeekStartDate, bestWeekStartDate.AddDays(7));
    }
    private void DisplayComparisonMonth()
    {
        M_SaveLoad.LoadHabitMonth(habitToDisplay.data.name, M_Date.singleton.today, out int monthCompletions, out float monthValue);
        M_SaveLoad.LoadHabitBestMonth(habitToDisplay.data.name, out DateTime bestMonthStartDate, out int bestMonthCompletions, out float bestMonthValue);

        comparisonMonth.GetChild(0).GetComponentInChildren<HabitStats>().DisplayStats(habitToDisplay.data.type, monthCompletions, monthValue, M_Date.MONTH_FORMAT, M_Date.singleton.startOfCurrentMonth);
        comparisonMonth.GetChild(1).GetComponentInChildren<HabitStats>().DisplayStats(habitToDisplay.data.type, bestMonthCompletions, bestMonthValue, M_Date.MONTH_FORMAT, bestMonthStartDate);
    }

    private void DisplayComparisonYear()
    {
        M_SaveLoad.LoadHabitYear(habitToDisplay.data.name, M_Date.singleton.today, out int yearCompletions, out float yearValue);
        M_SaveLoad.LoadHabitBestYear(habitToDisplay.data.name, out DateTime bestYearStartDate, out int bestYearCompletions, out float bestYearValue);


        comparisonYear.GetChild(0).GetComponentInChildren<HabitStats>().DisplayStats(habitToDisplay.data.type, yearCompletions, yearValue, M_Date.YEAR_FORMAT, M_Date.singleton.startOfCurrentYear);
        comparisonYear.GetChild(1).GetComponentInChildren<HabitStats>().DisplayStats(habitToDisplay.data.type, bestYearCompletions, bestYearValue, M_Date.YEAR_FORMAT, bestYearStartDate);
    }

    #endregion


    #region History
    [Header("\tComparison page")]
    [SerializeField] private ScrollRect scrollRectHistory;
    [SerializeField] private TMP_Dropdown dropdownHistoryTimePeriod;


    private List<RectTransform> pool = new List<RectTransform>();

    private float itemHeight = 125f; // height + buffer


    private int dataCount;
    private float previousScroll;
    private int topDataIndex = 0;



    private void SetHistoryPage()
    {
        SetDropdownHistoryTimePeriod();
        SetHistoryScrollRect();
    }

    private void SetDropdownHistoryTimePeriod()
        => dropdownHistoryTimePeriod.onValueChanged.AddListener((int option) => SetDataCount(CalculateNumberOfStats(option)));
    private void SetHistoryScrollRect()
    {
        scrollRectHistory.onValueChanged.AddListener(_ => RecycleIfNeeded());


        foreach (RectTransform item in scrollRectHistory.content)
            pool.Add(item);

        UpdateContentHeight();
    }


    private void DisplayHistoryPage()
    {
        dropdownHistoryTimePeriod.SetValueWithoutNotify(0);
        dropdownHistoryTimePeriod.onValueChanged.Invoke(0);
    }


    public void SetDataCount(int newDataCount)
    {
        dataCount = newDataCount;

        UpdateContentHeight();
        RefreshPool();
    }
    
    
    private void RefreshPool()
    {
        topDataIndex = 0;

        for (int i = 0; i < pool.Count; i++)
        {
            int dataIndex = topDataIndex + i;
            if (dataIndex >= dataCount)
                pool[i].gameObject.SetActive(false);
            else
            {
                pool[i].gameObject.SetActive(true);
                SetItemPosition(pool[i], dataIndex); UpdateItem(pool[i], dataIndex);
            }
        }
    }
    private void RecycleIfNeeded()
    {
        if (pool.Count == 0)
            return;

        float scrollY = scrollRectHistory.content.anchoredPosition.y;
        float viewportHeight = scrollRectHistory.viewport.rect.height;

        if (scrollY > previousScroll)
        {
            RectTransform first = pool[0];

            float topItemPos = topDataIndex * itemHeight;
            if (topItemPos + itemHeight < scrollY - itemHeight)
            {
                pool.RemoveAt(0);
                pool.Add(first);

                topDataIndex++;

                int newDataIndex = topDataIndex + pool.Count - 1;
                if (newDataIndex < dataCount)
                {
                    SetItemPosition(first, newDataIndex);
                    UpdateItem(first, newDataIndex);
                }
            }
        }
        else
        {
            RectTransform last = pool[pool.Count - 1];
            int lastDataIndex = topDataIndex + pool.Count - 1;

            float lastItemPos = lastDataIndex * itemHeight;
            if (lastItemPos > scrollY + viewportHeight + itemHeight)
            {
                pool.RemoveAt(pool.Count - 1);
                pool.Insert(0, last);

                topDataIndex--;

                int newDataIndex = topDataIndex;
                if (newDataIndex >= 0)
                {
                    SetItemPosition(last, newDataIndex);
                    UpdateItem(last, newDataIndex);
                }
            }
        }

        previousScroll = scrollY;
    }
    private void UpdateContentHeight()
        => scrollRectHistory.content.sizeDelta = new Vector2(scrollRectHistory.content.sizeDelta.x, dataCount * itemHeight);



    private void SetItemPosition(RectTransform item, int dataIndex)
        => item.anchoredPosition = new Vector2(0, -dataIndex * itemHeight);
    private void UpdateItem(RectTransform item, int dataIndex)
    {
        int val = dataCount - dataIndex - 1;

        switch (dropdownHistoryTimePeriod.value)
        {
            case 0: // Daily
                DateTime dayDate = habitToDisplay.data.creationDate.AddDays(val);

                M_SaveLoad.LoadHabitDay(habitToDisplay.data.name, dayDate, out int dayCompletion, out float dayValue);
                item.GetComponent<HabitStats>().DisplayStats(habitToDisplay.data.type, dayCompletion, dayValue, M_Date.DAY_FORMAT, dayDate);
                break;
            case 1: // Weekly
                DateTime weekDate = habitToDisplay.data.creationDate.AddDays(7*val);

                M_SaveLoad.LoadHabitWeek(habitToDisplay.data.name, weekDate, out int weekCompletion, out float weekValue);
                item.GetComponent<HabitStats>().DisplayStats(habitToDisplay.data.type, weekCompletion, weekValue, M_Date.DAY_FORMAT, M_Date.singleton.StartOfWeek(weekDate), M_Date.singleton.StartOfWeek(weekDate).AddDays(7));
                break;
            case 2: // Monthly
                DateTime monthDate = habitToDisplay.data.creationDate.AddMonths(val);

                M_SaveLoad.LoadHabitMonth(habitToDisplay.data.name, monthDate, out int monthCompletions, out float monthValue);
                item.GetComponent<HabitStats>().DisplayStats(habitToDisplay.data.type, monthCompletions, monthValue, M_Date.MONTH_FORMAT, monthDate);
                break;
            case 3: // Yearly
                DateTime yearDate = habitToDisplay.data.creationDate.AddYears(val);

                M_SaveLoad.LoadHabitYear(habitToDisplay.data.name, yearDate, out int yearCompletions, out float yearValue);
                item.GetComponent<HabitStats>().DisplayStats(habitToDisplay.data.type, yearCompletions, yearValue, M_Date.YEAR_FORMAT, yearDate);
                break;
        }

    }


    private int CalculateNumberOfStats(int dropdownOption)
    {
        var timeDiff = GetDifference(habitToDisplay.data.creationDate, M_Date.singleton.today);

        switch (dropdownOption)
        {
            case 0: // Daily
                return timeDiff.days;
            case 1: // Weekly
                return timeDiff.weeks;
            case 2: // Monthly
                return timeDiff.months;
            case 3: // Yearly
                return timeDiff.years;
        }

        return 0;
    }
    private (int years, int months, int weeks, int days) GetDifference(DateTime start, DateTime end)
    {
        int totalDays = (end - start).Days;

        int years = end.Year - start.Year;
        int months = end.Month - start.Month;
        int days = end.Day - start.Day;

        bool addHalfMonth = false;
        bool addHalfYear = false;

        if (days < 0)
        {
            end = end.AddMonths(-1);
            days += DateTime.DaysInMonth(end.Year, end.Month);
            months--;

            addHalfMonth = true;
        }

        if (months < 0)
        {
            months += 12;
            years--;

            addHalfYear = true;
        }

        if (addHalfMonth)
            months++;

        if(addHalfYear)
            years++;

        return (years + 1, 12 * years + months + 1, totalDays / 7 + 1, totalDays + 1);
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
    int prevMonth;


    private DateTime calendarDate = new DateTime();

    [Header("\tCalendar")]
    [SerializeField] private TMP_Dropdown dropdownCalendarMonth;
    [SerializeField] private TMP_Dropdown dropdownCalendarYear;
    [SerializeField] private Button buttonPrevMonth;
    [SerializeField] private Button buttonNextMonth;


    [SerializeField] private Transform calendar;
    [SerializeField] private Image imageToday;
    [SerializeField] private TextMeshProUGUI textNoteStatus;
    [SerializeField] private TextMeshProUGUI textNote;


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




    public void DisplayCalendar()
    {
        imageToday.gameObject.SetActive(false);

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
        string completionStatus = completion == 1 ? " you achieved your goal." : " you didn't achieve your goal.";
        string valueStatus = habitToDisplay.data.type == HabitType.yesOrNo ? "" : value + "/" + habitToDisplay.data.targetAmount +" "+ habitToDisplay.data.unit;


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


        Color dayColor = (completion == 0) ? Color.white : habitToDisplay.data.color;

        if (date < habitToDisplay.data.creationDate)
            dayColor.a = .25f;
        else if (date > M_Date.singleton.today)
            dayColor.a = isOtherMonth ? .33f : .66f;
        else if (date <= M_Date.singleton.today)
            dayColor.a = isOtherMonth ? .33f : 1;

        day.GetComponent<Image>().color = dayColor;
        
        day.GetComponent<Button>().onClick.RemoveAllListeners();
        day.GetComponent<Button>().onClick.AddListener(() => DisplayNote(date, completion, value));
    
        day.GetComponentInChildren<TextMeshProUGUI>().text = date.Day.ToString();

        if (date == M_Date.singleton.today)
        {
            imageToday.gameObject.SetActive(true);


            imageToday.transform.SetParent(day);
            imageToday.transform.SetAsFirstSibling();
            imageToday.transform.localPosition = Vector2.zero;
            
            
            imageToday.color = new Color(0, 0, 0, .5f);

        }
    }

    #endregion

}
