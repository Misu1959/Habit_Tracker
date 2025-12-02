using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VerticalDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{

    private enum DragType
    {
        none,
        pressed,
        sort,
        scroll
    };
    private ScrollRect scrollRect => M_UI_Main.singleton.panelHabits.GetComponentInParent<ScrollRect>();

    private PointerEventData currentEventData;

    private DragType dragType;


    [SerializeField] private SortedObject sortedObject;

    private float holdTime;
    [SerializeField] private float minHoldTime;
    [SerializeField] private float maxHoldTime;


    void Update() => CheckSortingTimer();


    public void OnPointerDown(PointerEventData eventData)
    {
        currentEventData = eventData;


        holdTime = 0f;
        dragType = DragType.pressed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(dragType == DragType.sort)
            sortedObject.EndSorting();

        Invoke(nameof(Fct), .01f);
    }

    void Fct()
    {
        sortedObject.GetComponent<Habit>().TurnButtonsOnOff(true);
        dragType = DragType.none;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        sortedObject.GetComponent<Habit>().TurnButtonsOnOff(false);

        switch (dragType)
        {
            case DragType.pressed:
                dragType = DragType.scroll;
                scrollRect.OnBeginDrag(eventData); 
                break;
            case DragType.scroll:
                dragType = DragType.scroll;
                scrollRect.OnBeginDrag(eventData); 
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        switch (dragType)
        {
            case DragType.scroll:
                scrollRect.OnDrag(eventData);
                break;
            case DragType.sort:
                sortedObject.Sort(eventData);
                break;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        sortedObject.GetComponent<Habit>().TurnButtonsOnOff(true);

        switch (dragType)
        {
            case DragType.scroll:
                dragType = DragType.none;
                scrollRect.OnEndDrag(eventData);
                break;
            case DragType.sort:
                dragType = DragType.none;
                sortedObject.EndSorting();
                break;
        }
    }

    private void CheckSortingTimer()
    {
        if (dragType != DragType.pressed)
            return;

        holdTime = Mathf.Clamp(holdTime + Time.deltaTime, 0, maxHoldTime);


        if (holdTime == maxHoldTime)
        {
            sortedObject.GetComponent<Habit>().TurnButtonsOnOff(false);

            dragType = DragType.sort;

            sortedObject.StartSorting();
            scrollRect.OnEndDrag(currentEventData);
        }
    }
}