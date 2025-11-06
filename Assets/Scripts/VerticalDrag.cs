using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VerticalDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas screen => GameObject.FindGameObjectWithTag("Screen").GetComponent<Canvas>();

    
    private RectTransform rectTransform     => GetComponent<RectTransform>();

    private float layoutSize;

    private Vector2 initialPosition;



    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = rectTransform.anchoredPosition;
        layoutSize = transform.parent.GetComponent<RectTransform>().rect.height;

        M_UI_Main.singleton.TurnLayoutOff();
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetPosition(eventData);
        ChangeOrder();
    }

    public void OnEndDrag(PointerEventData eventData) => M_UI_Main.singleton.TurnLayoutOn();




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

                M_UI_Main.singleton.RefreshLayout();
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

                M_UI_Main.singleton.RefreshLayout();
                break;
            }
        }
    }

}
