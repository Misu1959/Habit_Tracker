using UnityEngine;
using UnityEngine.UI;

public class PageSwiper : MonoBehaviour
{
    private ScrollRect scrollRect => GetComponent<ScrollRect>();

    private int   totalPages        = 3;
    private float swipeSpeed        = 10f;
    private float swipeThreshold    = .1f;

    private float[] pagePositions;
    private bool isDragging = false;
    private Vector2 startDragPos;

    void Start()
    {
        pagePositions = new float[totalPages];
        for (int i = 0; i < totalPages; i++)
        {
            pagePositions[i] = (float)i / (totalPages - 1);
        }
    }

    void Update()
    {
        HandleInput();
        if (!isDragging)
        {
            SnapToNearestPage();
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startDragPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            float dragDistance = Input.mousePosition.x - startDragPos.x;
         
            if (Mathf.Abs(dragDistance / Screen.width) > swipeThreshold)
                MovePage(dragDistance);
        }
    }

    private void MovePage(float dragDistance)
    {
        float currentPos = scrollRect.horizontalNormalizedPosition;
        int closestPage = 0;
        float minDist = Mathf.Abs(currentPos - pagePositions[0]);

        for (int i = 1; i < pagePositions.Length; i++)
        {
            float dist = Mathf.Abs(currentPos - pagePositions[i]);
            if (dist < minDist)
            {
                minDist = dist;
                closestPage = i;
            }
        }

        if (dragDistance < 0 && closestPage < totalPages - 1)
            closestPage++;
        else if (dragDistance > 0 && closestPage > 0)
            closestPage--;

        targetPage = closestPage;
    }

    private int targetPage = 0;

    private void SnapToNearestPage()
    {
        float targetPos = pagePositions[targetPage];
        scrollRect.horizontalNormalizedPosition = Mathf.Lerp(
            scrollRect.horizontalNormalizedPosition,
            targetPos,
            Time.deltaTime * swipeSpeed
        );
    }
}
