using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class M_UI_ColorPicker : MonoBehaviour
{
    public static M_UI_ColorPicker singleton;


    [Header("\nIn scene objects")]
    [SerializeField] private Image imageHabitColor;

    [SerializeField] private Transform colorsList;



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
        foreach (Transform color in colorsList)
        {
            color.GetComponent<Button>().onClick.AddListener(() => ChangeSelectedColor(color.GetSiblingIndex()));
            color.GetComponent<Button>().onClick.AddListener(M_UI_Main.singleton.CloseColorPickerMenu);
        }

    }

    public void ChangeSelectedColor(int colorIndex)
    {
        Transform colorCheck = null;

        foreach (Transform color in colorsList)
            if (color.childCount > 0)
            {
                colorCheck = color.GetChild(0);
                break;
            }

        colorCheck.SetParent(colorsList.GetChild(colorIndex));
        colorCheck.localPosition = Vector2.zero;

        imageHabitColor.color = colorsList.GetChild(colorIndex).GetComponent<Image>().color;
    }
}
