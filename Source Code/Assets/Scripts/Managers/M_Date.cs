using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;





public class M_Date : MonoBehaviour
{
    public static string DAY_FORMAT = "%d MMM yyyy";
    public static string MONTH_FORMAT = "MMM yyyy";
    public static string YEAR_FORMAT = "yyyy";

    public static M_Date singleton;


    public DateTime today => DateTime.Today;
    public DateTime startOfCurrentWeek => StartOfWeek(today);
    public DateTime startOfCurrentMonth => StartOfMonth(today);
    public DateTime startOfCurrentYear => StartOfYear(today);


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
    }


    public DateTime StartOfWeek(DateTime day)
        => day.AddDays(-(7 + (day.DayOfWeek - DayOfWeek.Monday)) % 7);

    public DateTime StartOfMonth(DateTime day)
        => new DateTime(day.Year, day.Month, 1);


    public DateTime StartOfYear(DateTime day)
        => new DateTime(day.Year, 1, 1);


    public IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
    {
        for (DateTime day = from.Date; day <= to.Date; day = day.AddDays(1))
            yield return day;
    }

    public IEnumerable<DateTime> EachWeek(DateTime from, DateTime to)
    {
        for (DateTime date = from.Date; date <= to.Date; date = date.AddDays(7))
            yield return date;
    }

    public IEnumerable<DateTime> EachMonth(DateTime from, DateTime to)
    {
        for (DateTime date = new DateTime(from.Year, from.Month, 1); date <= to; date = date.AddMonths(1))
            yield return date;
    }

    public IEnumerable<DateTime> EachYear(DateTime from, DateTime to)
    {
        for (DateTime date = new DateTime(from.Year, 1, 1); date <= to; date = date.AddYears(1))
            yield return date;
    }

    public bool ChechForNewDay(DateTime day, DateTime dateToCheck) => day != dateToCheck;
    public bool ChechForNewWeek(DateTime day, DateTime dateToCheck) => StartOfWeek(day) != StartOfWeek(dateToCheck);
    public bool ChechForNewMonth(DateTime day, DateTime dateToCheck) => StartOfMonth(day) != StartOfMonth(dateToCheck);
    public bool ChechForNewYear(DateTime day, DateTime dateToCheck) => StartOfYear(day) != StartOfYear(dateToCheck);


    public DateTime StringToDate(string str) => DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) ? date : DateTime.Today;

}
