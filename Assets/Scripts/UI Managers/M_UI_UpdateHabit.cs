using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class M_UI_UpdateHabit : MonoBehaviour
{

    public static M_UI_UpdateHabit singleton;


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


    private Habit habitToUpdate;
    public void ChangeHabitToUpdate(Habit newHabitToUpdate)
    {
        habitToUpdate = newHabitToUpdate;
        
        textName.text = habitToUpdate.data.name;
        textQuestion.text = habitToUpdate.data.question;


        bool activeType = habitToUpdate.data.type == HabitType.yesOrNo;

        toggleUpdate.gameObject.SetActive(activeType);
        toggleUpdate.isOn = habitToUpdate.data.currentAmount > 0;

        inputFieldUpdate.gameObject.SetActive(!activeType);
        inputFieldUpdate.text = habitToUpdate.data.currentAmount.ToString();
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
        buttonUpdateData.onClick.AddListener(() => Debug.Log("Saved"));
        buttonUpdateData.onClick.AddListener(M_UI_Main.singleton.CloseUpdateHabitMenu);
    }

    private void SetToggleUpdate()
    {
        toggleUpdate.onValueChanged.AddListener((val) => toggleUpdate.GetComponent<Image>().sprite = !val ? toggleCross : toggleCheck);
    }

}
