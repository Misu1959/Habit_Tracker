using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SortedObject : MonoBehaviour
{
    private Canvas screen => GameObject.FindGameObjectWithTag("Screen").GetComponent<Canvas>();
    private RectTransform rectTransform => GetComponent<RectTransform>();


    private float layoutSize;

    private Vector2 initialPosition;



    public void StartSorting()
    {
        initialPosition = rectTransform.anchoredPosition;
        layoutSize = transform.parent.GetComponent<RectTransform>().rect.height;


        Color color = GetComponent<Image>().color;
        color.a = 1;
        GetComponent<Image>().color = color;

        M_UI_Main.singleton.TurnLayoutOff();
        M_UI_SortHabits.singleton.ChangeSortingData(SortingType.manually);
    }

    public void Sort(PointerEventData eventData)
    {
        SetPosition(eventData);
        ChangeOrder();
    }


    public void EndSorting()
    {
        Color color = GetComponent<Image>().color;
        color.a = 0;

        GetComponent<Image>().color = color;
        M_UI_Main.singleton.TurnLayoutOn();

        M_UI_SortHabits.singleton.Sort();

    }





    private void SetPosition(PointerEventData eventData)
    {
        float scaledDeltaY = eventData.delta.y / screen.scaleFactor;

        Vector2 newPos = rectTransform.anchoredPosition;

        newPos.y += scaledDeltaY;
        newPos.y = Mathf.Clamp(newPos.y, -layoutSize, 0);

        rectTransform.anchoredPosition = newPos;
    }

    private void ChangeOrder()
    {
        int index = M_Habits.singleton.habitList.IndexOf(GetComponent<Habit>());

        for (int i = 0; i < index; i++)
        {
            RectTransform habitRect = M_Habits.singleton.habitList[i].GetComponent<RectTransform>();

            if (rectTransform.position.y > habitRect.position.y)
            {

                Habit aux = M_Habits.singleton.habitList[index];
                M_Habits.singleton.habitList[index] = M_Habits.singleton.habitList[i];
                M_Habits.singleton.habitList[i] = aux;

                StartCoroutine(M_UI_Main.singleton.RefreshLayout());
                break;
            }
        }


        for (int i = M_Habits.singleton.habitList.Count - 1; i > index; i--)
        {
            RectTransform habitRect = M_Habits.singleton.habitList[i].GetComponent<RectTransform>();

            if (rectTransform.position.y < habitRect.position.y)
            {
                Habit aux = M_Habits.singleton.habitList[index];
                M_Habits.singleton.habitList[index] = M_Habits.singleton.habitList[i];
                M_Habits.singleton.habitList[i] = aux;

                StartCoroutine(M_UI_Main.singleton.RefreshLayout());
                break;
            }
        }
    }
}
