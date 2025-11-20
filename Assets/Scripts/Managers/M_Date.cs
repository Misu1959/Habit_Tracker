using System;
using UnityEngine;

public class M_Date : MonoBehaviour
{
    public static M_Date singleton;


    public DateTime today => DateTime.Today;
    public DateTime startOfWeek 
    { 
        get
        {
            return today.AddDays(-(7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7);
        }
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
    }

}
