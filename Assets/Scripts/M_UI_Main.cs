using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M_UI_Main : MonoBehaviour
{
    public static M_UI_Main singleton;

    [field:SerializeField] public GameObject panelHabits { get; private set; }
    [SerializeField] private GameObject panelCreateHabit;

    [SerializeField] private Button buttonAddHabbit;

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
        SetButtonAddHabit();
    }


    #region Setup
    private void SetButtonAddHabit()
    {
        buttonAddHabbit.onClick.AddListener(OpenCreateHabitMenu);
        buttonAddHabbit.onClick.AddListener(M_UI_CreateHabit.singleton.ResetInputs);
    }

    #endregion


    #region Methods


    private void OpenCreateHabitMenu() => panelCreateHabit.SetActive(true);
    public void CloseCreateHabitMenu() => panelCreateHabit.SetActive(false);


    #endregion
}
