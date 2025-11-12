using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_SaveLoad : MonoBehaviour
{
    private const string NR_OF_HABITS = "nrOfHabits";


    private const string HABIT      = "habit";
    private const string KEY        = "key";

    private const string TYPE       = "type";
    private const string COLOR      = "color";
    private const string R      = "r";
    private const string G      = "g";
    private const string B      = "b";

    private const string NAME       = "name";
    private const string QUESTION   = "question";
    private const string UNIT       = "unit";
    private const string TARGET     = "target";

    private const string DAY        = "day";
    private const string MONTH      = "month";
    private const string YEAR       = "year";


    private const string CREATION_DATE  = "creation";
    
    private const string NR_OF_DATES    = "nrOfDates";
    private const string DATE           = "date";
    private const string DATE_FORMAT    = "dd mm yyyy";




    private const string SORT_TYPE  = "sortType";
    private const string SORT_WAY   = "sortWay";




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


    public static void LoadSortingData(out SortingType sortType,out SortingWay sortWay)
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

    public static void SaveNrOfHabits(int amount)
        => PlayerPrefs.SetInt(NR_OF_HABITS, amount);


    public static void SaveHabitData(Habit habitToSave)
    {
        HabitData data = habitToSave.data;

        SaveHabitType(data.name, data.type.ToString());
        SaveHabitColor(data.name, data.color);

        SaveHabitName(habitToSave);
        SaveHabitQuestion(data.name, data.question);
        
        SaveHabitUnit(data.name, data.unit);
        SaveHabitTarget(data.name, data.targetAmount);
        
        SaveHabitCreationDate(data.name, DateTime.Today);
    }



    private static void SaveHabitType(string name, string type)
        => PlayerPrefs.SetString(HABIT + name + TYPE, type);
    private static void SaveHabitColor(string name, Color color)
    {
        PlayerPrefs.SetFloat(HABIT + name + COLOR + R, color.r);
        PlayerPrefs.SetFloat(HABIT + name + COLOR + G, color.g);
        PlayerPrefs.SetFloat(HABIT + name + COLOR + B, color.b);
    }


    public static void SaveHabitName(Habit habit)
        => PlayerPrefs.SetString(HABIT + habit.transform.GetSiblingIndex() + NAME, habit.data.name);
    private static void SaveHabitQuestion(string name, string question)
        => PlayerPrefs.SetString(HABIT + name + QUESTION, question);


    private static void SaveHabitUnit(string name, string unit)
        => PlayerPrefs.SetString(HABIT + name + UNIT, unit);
    private static void SaveHabitTarget(string name, float target)
        => PlayerPrefs.SetFloat(HABIT + name + TARGET, target);


    private static void SaveHabitCreationDate(string name, DateTime creationDate)
        => PlayerPrefs.SetString(HABIT + name + CREATION_DATE, creationDate.ToString(DATE_FORMAT));




    public static void UpdateHabitColor(string name, Color newColor)
        => SaveHabitColor(name, newColor);

    public static void UpdateHabitInfo(string name, DateTime date, float amount)
    {
        PlayerPrefs.SetFloat(HABIT + name + date.ToString(DATE_FORMAT), amount);
        SaveHabitInfoDate(name, date);
    }


    private static void SaveHabitInfoDate(string name, DateTime infoDate)
    {
        int dateNumber;

        for (dateNumber = 0; dateNumber < LoadHabitNrOfDates(name); dateNumber++)
            if (infoDate == LoadHabitInfoDate(name, dateNumber))
                break;

        
        if(dateNumber == LoadHabitNrOfDates(name) + 1)
            SaveHabitNrOfDates(name, dateNumber);
        
        PlayerPrefs.SetString(HABIT + name + DATE + dateNumber, infoDate.ToString(DATE_FORMAT));
    }

    private static void SaveHabitNrOfDates(string name, int value)
        => PlayerPrefs.SetInt(HABIT + name + NR_OF_DATES, value);


    #endregion

    #region Load


    public static int LoadNrOfHabits()
        => PlayerPrefs.GetInt(NR_OF_HABITS);

    public static HabitData LoadHabitData(int key)
    {
        string habitName = LoadHabitName(key);


        HabitType type = LoadHabitType(habitName);
        Color32 color = LoadHabitColor(habitName);

        string question = LoadHabitQuestion(habitName);


        string unit = LoadHabitUnit(habitName);
        float target = LoadHabitTarget(habitName);

        return new HabitData(type, color, habitName, question, unit, target);
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
        => DateTime.Parse(PlayerPrefs.GetString(HABIT + name + CREATION_DATE));




    public static float GetHabitInfo(string name, DateTime date)
        => PlayerPrefs.GetFloat(HABIT + name + date.ToString(DATE_FORMAT));

    private static DateTime LoadHabitInfoDate(string name, int dateKey)
        => DateTime.Parse(PlayerPrefs.GetString(HABIT + name + DATE + dateKey));

    private static int LoadHabitNrOfDates(string name)
        => PlayerPrefs.GetInt(HABIT + name + NR_OF_DATES);
    #endregion

    #region Deletion

    public static void DeleteHabitData(string name)
    {

        DeleteHabitType(name);
        DeleteHabitColor(name);
        DeleteHabitName(name);
        DeleteHabitQuestion(name);
        DeleteHabitUnit(name);
        DeleteHabitTarget(name);


        DeleteCreationDate(name);

        DeleteHabitInfo(name);        // Delete data from all dates
        DeleteHabitInfoDates(name);   // Delete all dates
        DeleteHabitNrOfDates(name);   // Delete number of dates
    }


    private static void DeleteHabitType(string name)
        =>  PlayerPrefs.DeleteKey(HABIT + name + TYPE);
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
        for (int i = 0; i < LoadHabitNrOfDates(name); i++)
            PlayerPrefs.DeleteKey(HABIT + name + DATE + LoadHabitInfoDate(name, i));
    }

    private static void DeleteHabitInfoDates(string name)
    {
        for (int i = 0; i < LoadHabitNrOfDates(name); i++)
            PlayerPrefs.DeleteKey(HABIT + name + i);
    }
    private static void DeleteHabitNrOfDates(string name)
        => PlayerPrefs.DeleteKey(HABIT + name + NR_OF_DATES);
    #endregion

    #endregion
}
