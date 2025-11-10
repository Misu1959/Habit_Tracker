using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class M_UI_UpdateHabit : MonoBehaviour
{

    public static M_UI_UpdateHabit singleton;


    [SerializeField] private Button buttonUpdateData;

    [SerializeField] private Toggle toggleUpdate;
    [SerializeField] private TMP_InputField inputFieldUpdate;



    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textQuestion;

    [SerializeField] private TextMeshProUGUI textTarget;


    private Habit habitToUpdate;
    public void ChangeHabitToUpdate(Habit newHabitToUpdate)
    {
        habitToUpdate = newHabitToUpdate;
        
        textName.text = habitToUpdate.data.name;
        textQuestion.text = habitToUpdate.data.question;


        bool activeType = habitToUpdate.data.type == HabitType.yesOrNo;

        toggleUpdate.gameObject.SetActive(activeType);
        inputFieldUpdate.gameObject.SetActive(!activeType);

        toggleUpdate.isOn = habitToUpdate.data.currentAmount > 0;
        inputFieldUpdate.text = habitToUpdate.data.currentAmount + " / " + habitToUpdate.data.targetAmount; 
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
    }

    private void SetButtonUpdateData()
    {
        buttonUpdateData.onClick.AddListener(() => Debug.Log("Saved"));
        buttonUpdateData.onClick.AddListener(M_UI_Main.singleton.CloseUpdateHabitMenu);
    }
}
