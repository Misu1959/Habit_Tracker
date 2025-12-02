using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizedScrollRect : MonoBehaviour
{
    private ScrollRect scrollRect => GetComponent<ScrollRect>();
    private RectTransform content => scrollRect.content;
    
    [SerializeField]private float itemSize = 100f;
    private bool isDragging = false;


    public bool vertical = true;

    void LateUpdate()
    {
        if (isDragging)
            return;

        float current = vertical ? content.anchoredPosition.y : -content.anchoredPosition.x;
        float snapped = Mathf.Round(current / itemSize) * itemSize;

        if (vertical)
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, snapped);
        else
            content.anchoredPosition = new Vector2(-snapped, content.anchoredPosition.y);
    }

    public void OnBeginDrag() => isDragging = true;
    public void OnEndDrag() => StartCoroutine(StopDragging());

    IEnumerator StopDragging()
    {
        yield return new WaitForEndOfFrame();
        isDragging = false;
    }
}
