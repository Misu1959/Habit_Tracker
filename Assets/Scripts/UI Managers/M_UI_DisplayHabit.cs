using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class M_UI_DisplayHabit : MonoBehaviour
{

    public static M_UI_DisplayHabit singleton;

    private Habit habitToDisplay;


    [Header("In scene objects")]
    [SerializeField] private TextMeshProUGUI textName;

    [SerializeField] private Button buttonGoBack;
    [SerializeField] private Button buttonRecolorHabit;
    [SerializeField] private Button buttonDeleteHabit;


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
    }



    #region Setup

    private void SetButtonGoBack()
    {
        buttonGoBack.onClick.AddListener(M_UI_Main.singleton.CloseDisplayHabitMenu);
        buttonGoBack.onClick.AddListener(SetScrollRectPosition);
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

        DisplayHabitName();
    }

    private void DisplayHabitName() => textName.text = habitToDisplay.data.name;


    private void SetScrollRectPosition() => scrollRectPages.horizontalNormalizedPosition = 0;

    #endregion



    #region Stats Comparison 


    #endregion


    #region History


    #endregion


    #region Calendar

    [Header("Calendar")]
    [SerializeField] private Button buttonMonth;
    [SerializeField] private Button buttonYear;
    [SerializeField] private Dropdown dropDownValue;


    #endregion

}
