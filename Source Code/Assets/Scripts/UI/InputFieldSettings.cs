using UnityEngine;
using TMPro;
using System;


[Serializable]
public enum InputFieldType
{
    alphabet,
    digits,
    alphabetDigits,
    alphabetSigns,
    alphabetDigitsSigns
}

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldSettings : MonoBehaviour
{

    const string ALPHABET   = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
    const string DIGITS     = "1234567890.";
    const string SIGNS      = ".!?";


    private TMP_InputField inputField => GetComponent<TMP_InputField>();


    [SerializeField] private InputFieldType type;
    [SerializeField] private int charLimit;

    public Action onValueChangeCheck = null;
    public Action onValueEndCheck;


    private string ValidCharacters()
    {
        switch (type)
        {
            case InputFieldType.alphabet:
                return ALPHABET;
            case InputFieldType.digits: 
                return DIGITS;
            case InputFieldType.alphabetDigits:
                return ALPHABET + DIGITS;
            case InputFieldType.alphabetSigns:
                return ALPHABET + SIGNS;
            case InputFieldType.alphabetDigitsSigns:
                return ALPHABET + DIGITS + SIGNS;
        }

        return "";
    }


    private void Start() => Setup();

    private void Setup()
    {
        inputField.onValueChanged.AddListener((text) =>
        {
            inputField.characterLimit = !text.Contains('.') ? charLimit : charLimit + 3;
            onValueChangeCheck?.Invoke();
        });

        inputField.characterLimit = charLimit;


        if (type == InputFieldType.digits)
        {
            inputField.onValidateInput = (string text, int charIndex, char addedChar) => ValidateDigitCharacter(text, ValidCharacters(), addedChar);

            inputField.onEndEdit.AddListener((string text) =>
            {

                if (text.EndsWith("."))
                    text = text.TrimEnd('.');

                inputField.text = text;
            });
        }
        else
            inputField.onValidateInput = (string text, int charIndex, char addedChar) => ValidateCharacter(text, ValidCharacters(), addedChar);
    }


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
        inputField.characterLimit = !text.Contains('.') ? charLimit : charLimit + 3;


        char invalidChar = '\0';


        if (validCharacters.IndexOf(characterToAdd) == -1)
            return invalidChar;
        else if (text.Length == 1 && text[0] == '0' && characterToAdd != '.')
            return invalidChar;
        else if (text.Contains(characterToAdd) && characterToAdd == '.')
            return invalidChar;
        else if (text.Contains('.') && (text.Length - text.IndexOf('.')) > 2)
            return invalidChar;

        return characterToAdd;
    }
}
