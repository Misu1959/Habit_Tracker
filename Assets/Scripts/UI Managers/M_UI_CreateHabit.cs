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
        buttonCreateHabit.onClick.AddListener(() => M_Habits.singleton.CreateHabit(new HabitData(habitType, habitColor, habitName, habitQuestion, habitUnit, habitAmount)));
    }



    private void SetButtonChangeColor() => buttonChangeColor.onClick.AddListener(M_UI_Main.singleton.OpenColorPickerMenu);
    private void SetToggleType()
    {
        GameObject unitAmount_Group = inputHabitUnit.transform.parent.parent.gameObject;

        toggleHabitType.onValueChanged.AddListener((val) => ChangeToogleIcon(val));

        toggleHabitType.onValueChanged.AddListener((val) => unitAmount_Group.SetActive(val));
        toggleHabitType.onValueChanged.AddListener((val) => CheckAllMandatoryInputs());
    }
    
    
    
    private void SetInputsLimits()
    {
        string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ .!?";
        string digits = "1234567890.";


        SetInputField(inputHabitName, 10, alphabet);
        SetInputField(inputHabitQuestion, 20, alphabet + digits);

        SetInputField(inputHabitUnit, 10, alphabet);
        SetDigitInputField(inputHabitAmount, 5, digits);

    }

    private void SetInputField(TMP_InputField input, int characterAmount, string validCharacters)
    {
        input.characterLimit = characterAmount;

        input.onValidateInput = (string text, int charIndex, char addedChar) => ValidateCharacter(text, validCharacters, addedChar);

        input.onValueChanged.AddListener((text) => CheckAllMandatoryInputs());
    }

    private void SetDigitInputField(TMP_InputField input, int characterAmount, string validCharacters)
    {

        input.characterLimit = characterAmount;

        input.onValidateInput = (string text, int charIndex, char addedChar) => ValidateDigitCharacter(text, validCharacters, addedChar);

        input.onValueChanged.AddListener((text) => CheckAllMandatoryInputs());


        input.onEndEdit.AddListener((string text) =>
        {
            if (text.EndsWith("."))
                text = text.TrimEnd('.');

            input.text = text;
        });
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

        CheckAllMandatoryInputs();
    }


    private void ChangeToogleIcon(bool val) => toggleHabitType.GetComponent<Image>().sprite = !val ? toggleOff : toggleOn; 


    private char ValidateCharacter(string text, string validCharacters, char characterToAdd)
    {
        string signs = ".!?";

        if (validCharacters.IndexOf(characterToAdd) != -1)
            return characterToAdd;
        else if (signs.Contains(characterToAdd) && text.Length > 0)
            return characterToAdd;

        return '\0';
    }

    private char ValidateDigitCharacter(string text, string validCharacters, char characterToAdd)
    {
        char invalidChar = '\0';


        if (validCharacters.IndexOf(characterToAdd) == -1)
            return invalidChar;
        else if (text.Length == 1 && text[0] == '0' && characterToAdd != '.')
            return invalidChar;
        else if (text.Contains(characterToAdd) && characterToAdd == '.')
            return invalidChar;


        return characterToAdd;
    }

    private void CheckAllMandatoryInputs()
    {
        bool allMandatoryInputsMet = true;


        if (inputHabitName.text.Length == 0)
            allMandatoryInputsMet = false;


        if (toggleHabitType.isOn)
        {
            if (inputHabitUnit.text.Length == 0 || inputHabitAmount.text.Length == 0)
                allMandatoryInputsMet = false;

            if (float.TryParse(inputHabitAmount.text, out float value) && value == 0)
                allMandatoryInputsMet = false;
        }
        Color createHabitButtonColor = allMandatoryInputsMet ? Color.white : Color.gray;

        buttonCreateHabit.interactable = allMandatoryInputsMet;


        buttonCreateHabit.GetComponent<Image>().color = createHabitButtonColor;
        buttonCreateHabit.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = createHabitButtonColor;
    }

    #endregion
}
