using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class M_UI_UpdateHabit : MonoBehaviour
{

    public static M_UI_UpdateHabit singleton;
 
    private Habit habitToUpdate;
    private DateTime updateDay;

    [Header("Sprites")]
    [SerializeField] private Sprite toggleCross;
    [SerializeField] private Sprite toggleCheck;



    [Header("\nIn scene objects")]
    [SerializeField] private Button buttonUpdateData;

    [SerializeField] private Toggle toggleUpdate;
    [SerializeField] private TMP_InputField inputFieldUpdate;



    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textQuestion;

    [SerializeField] private TextMeshProUGUI textTarget;

    public void ChangeHabitToUpdate(Habit newHabitToUpdate, DateTime day)
    {
        habitToUpdate = newHabitToUpdate;
        updateDay = day;
        
        textName.text = habitToUpdate.data.name;
        textQuestion.text = habitToUpdate.data.question;


        bool activeType = habitToUpdate.data.type == HabitType.yesOrNo;

        
        toggleUpdate.gameObject.SetActive(activeType);
        toggleUpdate.isOn = M_SaveLoad.GetHabitInfo(newHabitToUpdate.data.name, updateDay) > 0;
        toggleUpdate.transform.GetChild(0).GetComponent<Image>().sprite = !toggleUpdate.isOn ? toggleCross : toggleCheck;


        inputFieldUpdate.gameObject.SetActive(!activeType);
        inputFieldUpdate.text = M_SaveLoad.GetHabitInfo(newHabitToUpdate.data.name, updateDay).ToString();
        inputFieldUpdate.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = "/" + habitToUpdate.data.targetAmount + habitToUpdate.data.unit;
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
        SetButtonUpdateData();
        SetToggleUpdate();
    }


    private void SetButtonUpdateData()
    {
        buttonUpdateData.onClick.AddListener(UpdateData);
        buttonUpdateData.onClick.AddListener(M_UI_Main.singleton.CloseUpdateHabitMenu);
        buttonUpdateData.onClick.AddListener(M_UI_SortHabits.singleton.Sort);
    }

    private void SetToggleUpdate()
    {
        toggleUpdate.onValueChanged.AddListener((val) => toggleUpdate.transform.GetChild(0).GetComponent<Image>().sprite = !val ? toggleCross : toggleCheck);
    }

    private void UpdateData()
    {
        float newValue;

        if (habitToUpdate.data.type == HabitType.yesOrNo)
            newValue = !toggleUpdate.isOn ? 0 : 1;
        else
            newValue = float.Parse(inputFieldUpdate.text);

        M_SaveLoad.UpdateHabitInfo(habitToUpdate.data.name, updateDay, newValue);
        habitToUpdate.UpdateData(newValue);
    }
}
