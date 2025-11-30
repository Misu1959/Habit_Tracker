using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class M_SaveLoad : MonoBehaviour
{
    private const string NR_OF_HABITS = "nrOfHabits";

    private const string HABIT = "habit";
    private const string KEY = "key";

    private const string TYPE = "type";
    private const string COLOR = "color";
    private const string R = "r";
    private const string G = "g";
    private const string B = "b";

    private const string NAME = "name";
    private const string QUESTION = "question";
    private const string UNIT = "unit";
    private const string TARGET = "target";
    private const string NOTE = "note";


    private const string CREATION_DATE = "creation";
    
    private const string TOTAL = "toatal";
    private const string BEST = "best";


    private const string DATE = "date";
    private const string DAY = "day";
    private const string WEEK = "week";
    private const string MONTH = "month";
    private const string YEAR = "year";

    private const string COMPLETIONS = "completions";
    private const string VALUE = "value";

    private const string STREAK = "streak";




    private const string START = "start";
    private const string END = "end";







    private const string SORT_TYPE = "sortType";
    private const string SORT_WAY = "sortWay";




    #region Sorting data

    public static void SaveSortingData(SortingType sortType, SortingWay sortWay)
    {
        SaveSortingType(sortType);
        SaveSortingWay(sortWay);
    }


    private static void SaveSortingType(SortingType sortType)
        => PlayerPrefs.SetString(SORT_TYPE, sortType.ToString());
    private static void SaveSortingWay(SortingWay sortWay)
        => PlayerPrefs.SetString(SORT_WAY, sortWay.ToString());


    public static void LoadSortingData(out SortingType sortType, out SortingWay sortWay)
    {
        sortType = LoadSortingType();
        sortWay = LoadSortingWay();
    }


    private static SortingType LoadSortingType()
    {
        if (Enum.TryParse(typeof(SortingType), PlayerPrefs.GetString(SORT_TYPE), out object output) == false)
            return SortingType.manually;

        return (SortingType)output;
    }
    private static SortingWay LoadSortingWay()
    {
        if (Enum.TryParse(typeof(SortingWay), PlayerPrefs.GetString(SORT_WAY), out object output) == false)
            return SortingWay.both;

        return (SortingWay)output;
    }

    #endregion



    #region Habits data


    #region Save

    public static void SaveNrOfHabits()
    {
        PlayerPrefs.SetInt(NR_OF_HABITS, M_Habits.singleton.habitList.Count);

        foreach (Habit habit in M_Habits.singleton.habitList)
            SaveHabitName(habit);
    }

    public static void SaveHabitData(Habit habitToSave)
    {
        HabitData data = habitToSave.data;

        SaveHabitType(data.name, data.type.ToString());
        SaveHabitColor(data.name, data.color);

        SaveHabitName(habitToSave);
        SaveHabitQuestion(data.name, data.question);

        SaveHabitUnit(data.name, data.unit);
        SaveHabitTarget(data.name, data.targetAmount);

        SaveHabitCreationDate(data.name, data.creationDate);

    }



    private static void SaveHabitType(string name, string type)
        => PlayerPrefs.SetString(HABIT + name + TYPE, type);
    private static void SaveHabitColor(string name, Color color)
    {
        PlayerPrefs.SetFloat(HABIT + name + COLOR + R, color.r);
        PlayerPrefs.SetFloat(HABIT + name + COLOR + G, color.g);
        PlayerPrefs.SetFloat(HABIT + name + COLOR + B, color.b);
    }


    private static void SaveHabitName(Habit habit)
        => PlayerPrefs.SetString(HABIT + habit.transform.GetSiblingIndex() + NAME, habit.data.name);
    private static void SaveHabitQuestion(string name, string question)
        => PlayerPrefs.SetString(HABIT + name + QUESTION, question);


    private static void SaveHabitUnit(string name, string unit)
        => PlayerPrefs.SetString(HABIT + name + UNIT, unit);
    private static void SaveHabitTarget(string name, float target)
        => PlayerPrefs.SetFloat(HABIT + name + TARGET, target);


    private static void SaveHabitCreationDate(string name, DateTime creationDate)
        => PlayerPrefs.SetString(HABIT + name + CREATION_DATE, creationDate.ToString());




    public static void UpdateHabitColor(string name, Color newColor)
        => SaveHabitColor(name, newColor);

    public static void UpdateHabit(string name, DateTime date, float amount, string note)
    {

        SaveHabitDay(name, date, amount);
        SaveHabitWeek(name, date);
        SaveHabitMonth(name,date);
        SaveHabitYear(name, date);
        SaveHabitStreak(name, date);

        SaveHabitStatsTotal(name, date);

        SaveHabitNote(name, date, note);
    }


    private static void SaveHabitNote(string name, DateTime date, string note)
        => PlayerPrefs.SetString(HABIT + name + DAY + date.ToString() + NOTE, note);

    private static void SaveHabitDay(string name, DateTime date, float value)
    {
        if (PlayerPrefs.GetString(HABIT + name + DAY + DATE) != M_Date.singleton.today.ToString())
        {
            SaveHabitBestDay(name, DateTime.Parse(PlayerPrefs.GetString(HABIT + name + DAY + DATE, M_Date.singleton.today.ToString()), CultureInfo.InvariantCulture)); // Check if yesterday is best day
            PlayerPrefs.SetString(HABIT + name + DAY + DATE, M_Date.singleton.today.ToString());
        }

        PlayerPrefs.SetInt(HABIT + name + DAY + date.ToString() + COMPLETIONS, value < LoadHabitTarget(name) ? 0 : 1);
        PlayerPrefs.SetFloat(HABIT + name + DAY + date.ToString() + VALUE, value);
    }
    private static void SaveHabitWeek(string name, DateTime date)
    {
        if(PlayerPrefs.GetString(HABIT + name + WEEK + DATE) != M_Date.singleton.StartOfWeek(date).ToString())
        {
            SaveHabitBestWeek(name, DateTime.Parse(PlayerPrefs.GetString(HABIT + name + WEEK + DATE, M_Date.singleton.startOfCurrentWeek.ToString()), CultureInfo.InvariantCulture)); // Check if last week is best week


            PlayerPrefs.SetString(HABIT + name + WEEK + DATE, M_Date.singleton.StartOfWeek(date).ToString());

            PlayerPrefs.SetInt(HABIT + name + WEEK + COMPLETIONS, 0);
            PlayerPrefs.SetFloat(HABIT + name + WEEK + VALUE, 0);
        }

        LoadHabitDay(name, date, out int dayCompletion, out float dayValue);

        int weekCompletions = PlayerPrefs.GetInt(HABIT + name + WEEK + COMPLETIONS);
        float weekValue     = PlayerPrefs.GetFloat(HABIT + name + WEEK + VALUE);



        PlayerPrefs.SetInt(HABIT + name + WEEK + M_Date.singleton.StartOfWeek(date).ToString()+ COMPLETIONS,  weekCompletions + dayCompletion);
        PlayerPrefs.SetFloat(HABIT + name + WEEK + M_Date.singleton.StartOfWeek(date).ToString() + VALUE, weekValue + dayValue);
    }
    private static void SaveHabitMonth(string name, DateTime date)
    {
        if (PlayerPrefs.GetString(HABIT + name + MONTH + DATE) != M_Date.singleton.StartOfMonth(date).ToString())
        {
            SaveHabitBestMonth(name, DateTime.Parse(PlayerPrefs.GetString(HABIT + name + MONTH + DATE, M_Date.singleton.startOfCurrentMonth.ToString()), CultureInfo.InvariantCulture)); // Check if last month is best month

            PlayerPrefs.SetString(HABIT + name + MONTH + DATE, M_Date.singleton.StartOfMonth(date).ToString());

            PlayerPrefs.SetInt(HABIT + name + MONTH + COMPLETIONS, 0);
            PlayerPrefs.SetFloat(HABIT + name + MONTH + VALUE, 0);
        }

        LoadHabitDay(name, date, out int dayCompletion, out float dayValue);

        int monthCompletions = PlayerPrefs.GetInt(HABIT + name + MONTH + COMPLETIONS);
        float monthValue = PlayerPrefs.GetFloat(HABIT + name + MONTH + VALUE);


        PlayerPrefs.SetInt(HABIT + name + MONTH + M_Date.singleton.StartOfMonth(date).ToString() + COMPLETIONS, monthCompletions + dayCompletion);
        PlayerPrefs.SetFloat(HABIT + name + MONTH + M_Date.singleton.StartOfMonth(date).ToString() + VALUE, monthValue + dayValue);
    }
    private static void SaveHabitYear(string name, DateTime date)
    {
        if (PlayerPrefs.GetString(HABIT + name + YEAR + DATE) !=  M_Date.singleton.StartOfYear(date).ToString())
        {
            SaveHabitBestYear(name, DateTime.Parse(PlayerPrefs.GetString(HABIT + name + YEAR + DATE, M_Date.singleton.startOfCurrentYear.ToString()), CultureInfo.InvariantCulture)); // Check if last year is best year

            PlayerPrefs.SetString(HABIT + name + YEAR + DATE, M_Date.singleton.StartOfYear(date).ToString());

            PlayerPrefs.SetInt(HABIT + name + YEAR + COMPLETIONS, 0);
            PlayerPrefs.SetFloat(HABIT + name + YEAR + VALUE, 0);
        }
        
        LoadHabitDay(name, date, out int dayCompletion, out float dayValue);

        int yearCompletions = PlayerPrefs.GetInt(HABIT + name + YEAR + COMPLETIONS);
        float yearValue = PlayerPrefs.GetFloat(HABIT + name + YEAR + VALUE);


        PlayerPrefs.SetInt(HABIT + name + YEAR + M_Date.singleton.StartOfYear(date).ToString() + COMPLETIONS, yearCompletions + dayCompletion);
        PlayerPrefs.SetFloat(HABIT + name + YEAR + M_Date.singleton.StartOfYear(date).ToString() + VALUE, yearValue + dayValue);
    }
    private static void SaveHabitStreak(string name, DateTime date)
    {
        if (PlayerPrefs.GetString(HABIT + name + STREAK + DATE) != date.ToString())
        {
            SaveHabitBestStreak(name); // Everyday  check if prevStreak is best streak

            PlayerPrefs.SetString(HABIT + name + STREAK + DATE, date.ToString());

            PlayerPrefs.SetInt(HABIT + name + STREAK + START + COMPLETIONS, PlayerPrefs.GetInt(HABIT + name + STREAK + COMPLETIONS));
            PlayerPrefs.SetFloat(HABIT + name + STREAK + START + VALUE, PlayerPrefs.GetFloat(HABIT + name + STREAK + VALUE));

            if (PlayerPrefs.GetInt(HABIT + name + STREAK + END) == 0)
            {
                PlayerPrefs.SetString(HABIT + name + STREAK + START + DATE, date.ToString());
                PlayerPrefs.SetString(HABIT + name + STREAK + END + DATE, date.ToString());
            }
        }

        LoadHabitDay(name, date, out int dayCompletion, out float dayValue);

        int streakCompletions = PlayerPrefs.GetInt(HABIT + name + STREAK + START + COMPLETIONS);
        float streakValue = PlayerPrefs.GetFloat(HABIT + name + STREAK + START + VALUE);

        PlayerPrefs.SetInt(HABIT + name + STREAK + COMPLETIONS, streakCompletions + dayCompletion);
        PlayerPrefs.SetFloat(HABIT + name + STREAK + VALUE, streakValue + dayValue);

        PlayerPrefs.SetInt(HABIT + name + STREAK + END, dayCompletion);
    
        if(dayCompletion == 1)
            PlayerPrefs.SetString(HABIT + name + STREAK + END + DATE, date.ToString());

    }




    private static void SaveHabitBestDay(string name, DateTime date)
    {
        LoadHabitDay(name, date, out int dayCompletion, out float dayValue);
        LoadHabitBestDay(name, out DateTime bestWeekDate, out int bestDayCompletion, out float bestDayValue);

        if (dayCompletion <= bestDayCompletion)
            return;

        PlayerPrefs.SetString(HABIT + name + BEST + DAY + DATE, date.ToString());

        PlayerPrefs.SetInt(HABIT + name + BEST + DAY + COMPLETIONS, dayCompletion);
        PlayerPrefs.SetFloat(HABIT + name + BEST + DAY + VALUE, dayValue);
    }
    private static void SaveHabitBestWeek(string name, DateTime date)
    {
        LoadHabitWeek(name, date, out int weekCompletions, out float weekValue);
        LoadHabitBestWeek(name, out DateTime bestWeekDate, out int bestWeekCompletions, out float bestWeekValue);

        if (weekCompletions <= bestWeekCompletions)
            return;

        PlayerPrefs.SetString(HABIT + name + BEST + WEEK + DATE, M_Date.singleton.StartOfWeek(date).ToString());

        PlayerPrefs.SetInt(HABIT + name + BEST + WEEK + COMPLETIONS, weekCompletions);
        PlayerPrefs.SetFloat(HABIT + name + BEST + WEEK + VALUE, weekValue);
    }
    private static void SaveHabitBestMonth(string name, DateTime date)
    {
        LoadHabitMonth(name, date, out int monthCompletions, out float monthValue);
        LoadHabitBestMonth(name, out DateTime bestMonthDate, out int bestMonthCompletions, out float bestMonthValue);

        if (monthCompletions <= bestMonthCompletions)
            return;

        PlayerPrefs.SetString(HABIT + name + BEST + MONTH + DATE, M_Date.singleton.StartOfMonth(date).ToString());

        PlayerPrefs.SetInt(HABIT + name + BEST + MONTH + COMPLETIONS, monthCompletions);
        PlayerPrefs.SetFloat(HABIT + name + BEST + MONTH + VALUE, monthValue);
    }
    private static void SaveHabitBestYear(string name, DateTime date)
    {
        LoadHabitYear(name, date, out int yearCompletions, out float yearValue);
        LoadHabitBestYear(name, out DateTime bestYearDate, out int bestYearCompletions, out float bestYearValue);

        if (yearCompletions <= bestYearCompletions)
            return;

        PlayerPrefs.SetString(HABIT + name + BEST + YEAR + DATE, M_Date.singleton.StartOfYear(date).ToString());

        PlayerPrefs.SetInt(HABIT + name + BEST + YEAR + COMPLETIONS, yearCompletions);
        PlayerPrefs.SetFloat(HABIT + name + BEST + YEAR + VALUE, yearValue);
    }
    private static void SaveHabitBestStreak(string name)
    {
        LoadHabitStreak(name, out DateTime streakStartDate, out DateTime streakEndDate, out int streakCompletions, out float streakValue);
        LoadHabitBestStreak(name, out DateTime bestStreakStartDate, out DateTime bestStreakEndDate, out int bestStreakCompletions, out float bestStreakValue);

        if (streakCompletions <= bestStreakCompletions)
            return;

        PlayerPrefs.SetString(HABIT + name + BEST + STREAK + START + DATE, streakStartDate.ToString());
        PlayerPrefs.SetString(HABIT + name + BEST + STREAK + END + DATE, streakEndDate.ToString());


        PlayerPrefs.SetInt(HABIT + name + BEST + STREAK + COMPLETIONS, streakCompletions);
        PlayerPrefs.SetFloat(HABIT + name + BEST + STREAK + VALUE, streakValue);
    }


    private static void SaveHabitStatsTotal(string name, DateTime date)
    {
        if (PlayerPrefs.GetString(HABIT + name + TOTAL + DATE) != date.ToString())
        {
            PlayerPrefs.SetString(HABIT + name + TOTAL + DATE, date.ToString());

            PlayerPrefs.SetInt(HABIT + name + TOTAL + START + COMPLETIONS, PlayerPrefs.GetInt(HABIT + name + TOTAL + COMPLETIONS));
            PlayerPrefs.SetFloat(HABIT + name + TOTAL + START + VALUE, PlayerPrefs.GetFloat(HABIT + name + TOTAL + VALUE));
        }

        LoadHabitDay(name, date, out int dayCompletion, out float dayValue);

        int totalCompletions = PlayerPrefs.GetInt(HABIT + name + TOTAL + START + COMPLETIONS);
        float totalValue = PlayerPrefs.GetFloat(HABIT + name + TOTAL + START + VALUE);


        PlayerPrefs.SetInt(HABIT + name + TOTAL + COMPLETIONS, totalCompletions + dayCompletion);
        PlayerPrefs.SetFloat(HABIT + name + TOTAL + VALUE, totalValue + dayValue);
    }

    #endregion

    #region Load


    public static int LoadNrOfHabits()
        => PlayerPrefs.GetInt(NR_OF_HABITS);

    public static HabitData LoadHabitData(int key)
    {
        string habitName = LoadHabitName(key);

        DateTime creationDate = LoadHabitCreationDate(habitName);

        HabitType type = LoadHabitType(habitName);
        Color32 color = LoadHabitColor(habitName);

        string question = LoadHabitQuestion(habitName);


        string unit = LoadHabitUnit(habitName);
        float target = LoadHabitTarget(habitName);


        return new HabitData(creationDate,type, color, habitName, question, unit, target);
    }



    private static HabitType LoadHabitType(string name)
        => (HabitType)Enum.Parse(typeof(HabitType), PlayerPrefs.GetString(HABIT + name + TYPE));

    private static Color32 LoadHabitColor(string name)
    {
        float r = PlayerPrefs.GetFloat(HABIT + name + COLOR + R);
        float g = PlayerPrefs.GetFloat(HABIT + name + COLOR + G);
        float b = PlayerPrefs.GetFloat(HABIT + name + COLOR + B);


        return new Color(r, g, b, 1);
    }


    private static string LoadHabitName(int key)
        => PlayerPrefs.GetString(HABIT + key + NAME);

    private static string LoadHabitQuestion(string name)
        => PlayerPrefs.GetString(HABIT + name + QUESTION);


    private static string LoadHabitUnit(string name)
        => PlayerPrefs.GetString(HABIT + name + UNIT);
    private static float LoadHabitTarget(string name)
        => PlayerPrefs.GetFloat(HABIT + name + TARGET);


    private static DateTime LoadHabitCreationDate(string name)
        => DateTime.Parse(PlayerPrefs.GetString(HABIT + name + CREATION_DATE), CultureInfo.InvariantCulture);




    public static string LoadHabitNote(string name, DateTime date)
        => PlayerPrefs.GetString(HABIT + name + DAY + date.ToString() + NOTE);

    public static void LoadHabitDay(string name, DateTime date, out int completions, out float value)
    {
        completions = PlayerPrefs.GetInt(HABIT + name + DAY + date.ToString() + COMPLETIONS);
        value = PlayerPrefs.GetFloat(HABIT + name + DAY + date.ToString() + VALUE);
    }
    public static void LoadHabitWeek(string name, DateTime date, out int completions, out float value)
    {
        completions = PlayerPrefs.GetInt(HABIT + name + WEEK + M_Date.singleton.StartOfWeek(date).ToString() + COMPLETIONS);
        value = PlayerPrefs.GetFloat(HABIT + name + WEEK + M_Date.singleton.StartOfWeek(date).ToString() + VALUE);
    }
    public static void LoadHabitMonth(string name, DateTime date, out int completions, out float value)
    {
        completions = PlayerPrefs.GetInt(HABIT + name + MONTH + M_Date.singleton.StartOfMonth(date).ToString() + COMPLETIONS);
        value = PlayerPrefs.GetFloat(HABIT + name + MONTH + M_Date.singleton.StartOfMonth(date).ToString() + VALUE);
    }
    public static void LoadHabitYear(string name, DateTime date, out int completions, out float value)
    {
        completions = PlayerPrefs.GetInt(HABIT + name + YEAR + M_Date.singleton.StartOfYear(date).ToString() + COMPLETIONS);
        value = PlayerPrefs.GetFloat(HABIT + name + YEAR + M_Date.singleton.StartOfYear(date).ToString() + VALUE);
    }
    public static void LoadHabitStreak(string name, out DateTime startDate, out DateTime endDate, out int completions, out float value)
    {
        string dateS = PlayerPrefs.GetString(HABIT + name + STREAK + START + DATE, M_Date.singleton.today.ToString());
        string dateE = PlayerPrefs.GetString(HABIT + name + STREAK + END + DATE, M_Date.singleton.today.ToString());


        startDate = DateTime.Parse(dateS, CultureInfo.InvariantCulture);
        endDate = DateTime.Parse(dateE, CultureInfo.InvariantCulture);


        completions = PlayerPrefs.GetInt(HABIT + name + STREAK + COMPLETIONS);
        value = PlayerPrefs.GetFloat(HABIT + name + STREAK + VALUE);
    }


    public static void LoadHabitBestDay(string name, out DateTime bestDate, out int completions, out float value)
    {
        string date = PlayerPrefs.GetString(HABIT + name + BEST + DAY + DATE, M_Date.singleton.today.ToString());
        bestDate = DateTime.Parse(date, CultureInfo.InvariantCulture);


        completions = PlayerPrefs.GetInt(HABIT + name + BEST + DAY + COMPLETIONS);
        value = PlayerPrefs.GetFloat(HABIT + name + BEST + DAY + VALUE);
    }
    public static void LoadHabitBestWeek(string name, out DateTime startDate, out int completions, out float value)
    {
        string date = PlayerPrefs.GetString(HABIT + name + BEST + WEEK + DATE, M_Date.singleton.startOfCurrentWeek.ToString());
        startDate = DateTime.Parse(date, CultureInfo.InvariantCulture);

        completions = PlayerPrefs.GetInt(HABIT + name + BEST + WEEK + COMPLETIONS);
        value = PlayerPrefs.GetFloat(HABIT + name + BEST + WEEK + VALUE);
    }
    public static void LoadHabitBestMonth(string name, out DateTime startDate, out int completions, out float value)
    {
        string date = PlayerPrefs.GetString(HABIT + name + BEST + MONTH + DATE, M_Date.singleton.startOfCurrentMonth.ToString());
        startDate = DateTime.Parse(date, CultureInfo.InvariantCulture);

        completions = PlayerPrefs.GetInt(HABIT + name + BEST + MONTH + COMPLETIONS);
        value = PlayerPrefs.GetFloat(HABIT + name + BEST + MONTH + VALUE);
    }
    public static void LoadHabitBestYear(string name, out DateTime startDate, out int completions, out float value)
    {
        string date = PlayerPrefs.GetString(HABIT + name + BEST + YEAR + DATE, M_Date.singleton.startOfCurrentYear.ToString());
        startDate = DateTime.Parse(date, CultureInfo.InvariantCulture);

        completions = PlayerPrefs.GetInt(HABIT + name + BEST + YEAR + COMPLETIONS);
        value = PlayerPrefs.GetFloat(HABIT + name + BEST + YEAR + VALUE);
    }
    public static void LoadHabitBestStreak(string name, out DateTime startDate, out DateTime endDate, out int completions, out float value)
    {
        string dateS = PlayerPrefs.GetString(HABIT + name + BEST + STREAK + START + DATE, M_Date.singleton.today.ToString());
        string dateE = PlayerPrefs.GetString(HABIT + name + BEST + STREAK + END + DATE, M_Date.singleton.today.ToString());


        startDate = DateTime.Parse(dateS, CultureInfo.InvariantCulture);
        endDate = DateTime.Parse(dateE, CultureInfo.InvariantCulture);

        completions = PlayerPrefs.GetInt(HABIT + name + BEST + STREAK + COMPLETIONS);
        value = PlayerPrefs.GetFloat(HABIT + name + BEST + STREAK + VALUE);
    }



    public static void LoadHabitStatsTotal(string name, out int totalCompletions, out float totalValue)
    {
        totalCompletions = PlayerPrefs.GetInt(HABIT + name + TOTAL + COMPLETIONS, 0);
        totalValue = PlayerPrefs.GetFloat(HABIT + name + TOTAL + VALUE, 0);
    }



    #endregion

    #region Deletion

    public static void DeleteHabitData(string name)
    {
        DeleteHabitInfo(name);

        DeleteHabitType(name);
        DeleteHabitColor(name);
        DeleteHabitName(name);
        DeleteHabitQuestion(name);
        DeleteHabitUnit(name);
        DeleteHabitTarget(name);
        DeleteCreationDate(name);
    }


    private static void DeleteHabitType(string name)
        => PlayerPrefs.DeleteKey(HABIT + name + TYPE);
    private static void DeleteHabitColor(string name)
    {
        PlayerPrefs.DeleteKey(HABIT + name + COLOR + R);
        PlayerPrefs.DeleteKey(HABIT + name + COLOR + G);
        PlayerPrefs.DeleteKey(HABIT + name + COLOR + B);
    }


    private static void DeleteHabitName(string name)
        => PlayerPrefs.DeleteKey(HABIT + name + NAME);
    private static void DeleteHabitQuestion(string name)
        => PlayerPrefs.DeleteKey(HABIT + name + QUESTION);


    private static void DeleteHabitUnit(string name)
        => PlayerPrefs.DeleteKey(HABIT + name + UNIT);
    private static void DeleteHabitTarget(string name)
        => PlayerPrefs.DeleteKey(HABIT + name + TARGET);


    private static void DeleteCreationDate(string name)
        => PlayerPrefs.DeleteKey(HABIT + name + CREATION_DATE);




    private static void DeleteHabitInfo(string name)
    {
        DeleteHabitDays(name);
        DeleteHabitWeeks(name);
        DeleteHabitMonths(name);
        DeleteHabitYears(name);
        DeleteHabitStreak(name);


        DeleteHabitBestDay(name);
        DeleteHabitBestWeek(name);
        DeleteHabitBestMonth(name);
        DeleteHabitBestYear(name);
        DeleteHabitBestStreak(name);

        DeleteHabitStatsTotal(name);
    }



    public static void DeleteHabitDays(string name)
    {
        foreach (DateTime day in M_Date.singleton.EachDay(LoadHabitCreationDate(name), M_Date.singleton.today))
        {
            PlayerPrefs.DeleteKey(HABIT + name + DAY + day.ToString() + COMPLETIONS);
            PlayerPrefs.DeleteKey(HABIT + name + DAY + day.ToString() + VALUE);

            PlayerPrefs.DeleteKey(HABIT + name + DAY + day.ToString() + NOTE);
        }
    }
    public static void DeleteHabitWeeks(string name)
    {
        foreach (DateTime day in M_Date.singleton.EachWeek(LoadHabitCreationDate(name), M_Date.singleton.today))
        {
            PlayerPrefs.DeleteKey(HABIT + name + WEEK + M_Date.singleton.StartOfWeek(day).ToString() + COMPLETIONS);
            PlayerPrefs.DeleteKey(HABIT + name + WEEK + M_Date.singleton.StartOfWeek(day).ToString() + VALUE);
        }
    }
    public static void DeleteHabitMonths(string name)
    {
        foreach (DateTime day in M_Date.singleton.EachMonth(LoadHabitCreationDate(name), M_Date.singleton.today))
        {
            PlayerPrefs.DeleteKey(HABIT + name + MONTH + day.ToString() + COMPLETIONS);
            PlayerPrefs.DeleteKey(HABIT + name + MONTH + day.ToString() + VALUE);
        }
    }
    public static void DeleteHabitYears(string name)
    {
        foreach (DateTime day in M_Date.singleton.EachYear(LoadHabitCreationDate(name), M_Date.singleton.today))
        {
            PlayerPrefs.DeleteKey(HABIT + name + YEAR + day.ToString() + COMPLETIONS);
            PlayerPrefs.DeleteKey(HABIT + name + YEAR + day.ToString() + VALUE);
        }
    }
    public static void DeleteHabitStreak(string name)
    {
        PlayerPrefs.DeleteKey(HABIT + name + STREAK + START + DATE);
        PlayerPrefs.DeleteKey(HABIT + name + STREAK + END + DATE);

        PlayerPrefs.DeleteKey(HABIT + name + STREAK + COMPLETIONS);
        PlayerPrefs.DeleteKey(HABIT + name + STREAK + VALUE);
    }



    public static void DeleteHabitBestDay(string name)
    {
        PlayerPrefs.DeleteKey(HABIT + name + BEST + DAY + DATE);

        PlayerPrefs.DeleteKey(HABIT + name + BEST + DAY + COMPLETIONS);
        PlayerPrefs.DeleteKey(HABIT + name + BEST + DAY + VALUE);
    }
    public static void DeleteHabitBestWeek(string name)
    {
        PlayerPrefs.DeleteKey(HABIT + name + BEST + WEEK + DATE);

        PlayerPrefs.DeleteKey(HABIT + name + BEST + WEEK + COMPLETIONS);
        PlayerPrefs.DeleteKey(HABIT + name + BEST + WEEK + VALUE);
    }
    public static void DeleteHabitBestMonth(string name)
    {
        PlayerPrefs.DeleteKey(HABIT + name + BEST + MONTH + DATE);

        PlayerPrefs.DeleteKey(HABIT + name + BEST + MONTH + COMPLETIONS);
        PlayerPrefs.DeleteKey(HABIT + name + BEST + MONTH + VALUE);
    }
    public static void DeleteHabitBestYear(string name)
    {
        PlayerPrefs.DeleteKey(HABIT + name + BEST + YEAR + DATE);

        PlayerPrefs.DeleteKey(HABIT + name + BEST + YEAR + COMPLETIONS);
        PlayerPrefs.DeleteKey(HABIT + name + BEST + YEAR + VALUE);
    }
    public static void DeleteHabitBestStreak(string name)
    {
        PlayerPrefs.DeleteKey(HABIT + name + BEST + STREAK + START + DATE);
        PlayerPrefs.DeleteKey(HABIT + name + BEST + STREAK + END + DATE);

        PlayerPrefs.DeleteKey(HABIT + name + BEST + STREAK + COMPLETIONS);
        PlayerPrefs.DeleteKey(HABIT + name + BEST + STREAK + VALUE);
    }


    public static void DeleteHabitStatsTotal(string name)
    {
        PlayerPrefs.DeleteKey(HABIT + name + TOTAL + COMPLETIONS);
        PlayerPrefs.DeleteKey(HABIT + name + TOTAL + VALUE);
    }
    #endregion

    #endregion
}
