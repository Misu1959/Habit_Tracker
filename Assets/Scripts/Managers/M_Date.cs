using System;
using UnityEngine;

public class M_Date : MonoBehaviour
{
    public static M_Date singleton;


    public DateTime today => DateTime.Today;
    public DateTime startOfWeek => today.AddDays(-(int)today.DayOfWeek + 1);


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
