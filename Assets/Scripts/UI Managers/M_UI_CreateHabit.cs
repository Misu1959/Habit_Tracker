using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M_UI_CreateHabit : MonoBehaviour
{

    public static M_UI_CreateHabit singleton;
    
    [Header("Sprites")]
    [SerializeField] private Sprite toggleOff;
    [SerializeField] private Sprite toggleOn;

    
    [Header("\nIn scene objects")]
    [SerializeField] private Button buttonCreateHabit;
    [SerializeField] private Button buttonCancel;


    [SerializeField] private Button buttonChangeColor;

    [SerializeField] private TMP_InputField inputHabitName;
    [SerializeField] private TMP_InputField inputHabitQuestion;

    [SerializeField] private TMP_InputField inputHabitUnit;
    [SerializeField] private TMP_InputField inputHabitAmount;


    [SerializeField] private Toggle toggleHabitType;



    #region Habit inputs

    private Color habitColor => buttonChangeColor.transform.GetChild(0).GetComponent<Image>().color;
    private HabitType habitType => !toggleHabitType.isOn ? HabitType.yesOrNo : HabitType.measurable; 

    private string habitName => inputHabitName.text;
    private string habitQuestion => inputHabitQuestion.text;
    private string habitUnit => inputHabitUnit.text;

    private float habitAmount => !float.TryParse(inputHabitAmount.text, out float value) ? 1 : value;


    #endregion

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
        SetButtonCancel();
        SetButtonCreateHabit();


        SetButtonChangeColor();
        SetToggleType();
        SetInputsLimits();
    }
    
    
    #region Setup

    private void SetButtonCancel() => buttonCancel.onClick.AddListener(M_UI_Main.singleton.CloseCreateHabitMenu);
    private void SetButtonCreateHabit()
    {
        buttonCreateHabit.onClick.AddListener(M_UI_Main.singleton.CloseCreateHabitMenu);
        buttonCreateHabit.onClick.AddListener(() => M_Habits.singleton.CreateHabit(new HabitData(M_Date.singleton.today, habitType, habitColor, habitName, habitQuestion, habitUnit, habitAmount), false));
    }



    private void SetButtonChangeColor() => buttonChangeColor.onClick.AddListener(M_UI_Main.singleton.OpenColorPickerMenu);
    private void SetToggleType()
    {
        GameObject unitAmount_Group = inputHabitUnit.transform.parent.gameObject;

        toggleHabitType.onValueChanged.AddListener((val) => toggleHabitType.transform.GetChild(0).GetComponent<Image>().sprite = !val ? toggleOff : toggleOn);

        toggleHabitType.onValueChanged.AddListener((val) => unitAmount_Group.SetActive(val));
        toggleHabitType.onValueChanged.AddListener((val) => CheckAllMandatoryInputs());
    }
    
    
    
    private void SetInputsLimits()
    {
        inputHabitName.GetComponent<InputFieldSettings>().onValueChangeCheck   += CheckAllMandatoryInputs;
        inputHabitUnit.GetComponent<InputFieldSettings>().onValueChangeCheck   += CheckAllMandatoryInputs;
        inputHabitAmount.GetComponent<InputFieldSettings>().onValueChangeCheck += CheckAllMandatoryInputs;

    }

    #endregion


    #region Methods

    public void ResetInputs()
    {
        inputHabitName.text = string.Empty;
        inputHabitQuestion.text = string.Empty;
        
        inputHabitUnit.text = string.Empty;
        inputHabitAmount.text = string.Empty;

        toggleHabitType.isOn = false;


        M_UI_ColorPicker.singleton.ChangeSelectedColor(0);
        M_UI_Main.singleton.panelCreateHabit.GetComponent<PageColorizer>().Colorize(habitColor);


        CheckAllMandatoryInputs();
    }

    private void CheckAllMandatoryInputs()
    {
        bool allMandatoryInputsMet = true;

        inputHabitName.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        inputHabitUnit.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        inputHabitAmount.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";


        if (inputHabitName.text.Length == 0)
        {
            allMandatoryInputsMet = false;

            inputHabitName.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Mandatory field!";
        }

        if(!CheckForNameDuplicate())
        {
            allMandatoryInputsMet = false;
            inputHabitName.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Name already used!";
        }

        if (toggleHabitType.isOn)
        {
            if (inputHabitUnit.text.Length == 0)
            {
                allMandatoryInputsMet = false;
                inputHabitUnit.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Mandatory field!";
            }

            if (inputHabitAmount.text == "" || float.TryParse(inputHabitAmount.text, out float value) && value == 0)
            {
                allMandatoryInputsMet = false;
                inputHabitAmount.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Mandatory field!";
            }
        }

        buttonCreateHabit.interactable = allMandatoryInputsMet;
    }

    private bool CheckForNameDuplicate()
    {
        foreach (Habit habit in M_Habits.singleton.habitList)
            if (habit.data.name == inputHabitName.text)
                return false;

        return true;
    }


    #endregion
}
