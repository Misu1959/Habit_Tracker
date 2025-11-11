using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Action action { private get; set; }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerEnter != this.gameObject)
            return;
        
        action?.Invoke();
    }

}