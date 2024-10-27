using AxGrid;
using AxGrid.Base;
using AxGrid.Path;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviourExt
{
    public float initialSpeed = 10f;
    public float targetSpeed = 50f;
    public float accelerationTime = 3f;
    public float decelerationTime = 1f;
    public ScrollRect scrollRect;
    public VerticalLayoutGroup verticalLayoutGroup;

    private RectTransform content;
    private float outOfBoundsThreshold;
    private float contentHeight;
    private bool isScrolling;
    private float currentSpeed;
    private float elapsedTime;
    private float initialPosition;

    [OnStart]
    public void Init()
    {
        content = scrollRect.content;
        outOfBoundsThreshold = scrollRect.content.GetChild(0).GetComponent<RectTransform>().rect.height * 1.5f +
                               verticalLayoutGroup.spacing * 3;
        scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        contentHeight = CalculateContentHeight();
        isScrolling = false;
        currentSpeed = initialSpeed;
        elapsedTime = 0f;
        initialPosition = content.anchoredPosition.y;

        Settings.Model.EventManager.AddAction("StartScrolling", StartScrolling);
        Settings.Model.EventManager.AddAction("StopScrolling", StopScrolling);
    }

    [OnUpdate]
    public void UpdateScroll()
    {
        if (isScrolling)
        {
            elapsedTime += Time.deltaTime;
            currentSpeed = Mathf.Lerp(initialSpeed, targetSpeed, elapsedTime / accelerationTime);

            AutoScrollDown();
            HandleVerticalScroll();
        }
    }

    private void AutoScrollDown()
    {
        content.anchoredPosition += currentSpeed * Time.deltaTime * Vector2.down;
    }

    private void HandleVerticalScroll()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            var item = content.GetChild(i);

            if (ReachedThreshold(item))
            {
                Vector2 newPos = item.localPosition;
                newPos.y += contentHeight + outOfBoundsThreshold - verticalLayoutGroup.spacing * 2;
                item.localPosition = newPos;
            }
        }
    }

    private bool ReachedThreshold(Transform item)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(item.position);
    
        return viewportPosition.y < -0.5f; 
    }



    private float CalculateContentHeight()
    {
        float totalHeight = 0f;
        for (int i = 0; i < content.childCount; i++)
        {
            totalHeight += content.GetChild(i).GetComponent<RectTransform>().rect.height;
        }

        return totalHeight;
    }

    private void StartScrolling()
    {
        isScrolling = true;
        elapsedTime = 0f;
    }


    private void StopScrolling()
    {
        Path.EasingCircEaseOut(decelerationTime, currentSpeed, 0f, value =>
            {
                currentSpeed = value;
                AutoScrollDown();
            })
            .Action(() =>
            {
                isScrolling = false;
                SnapToClosestItem();
                Settings.Model.EventManager.Invoke("OnStopScrollingAnimationComplete");
            });
    }

    private void SnapToClosestItem()
    {
        float offset = scrollRect.content.GetChild(0).GetComponent<RectTransform>().rect.height +
                       verticalLayoutGroup.spacing;
        float currentPositionY = content.anchoredPosition.y;
        float n = Mathf.Round((currentPositionY - initialPosition) / offset);
        float overshootPositionY = initialPosition + offset * (n + 0.2f);
        float targetPositionY = initialPosition + offset * n;

        Path.EasingBounceEaseOut(0.2f, currentPositionY, overshootPositionY,
                value => { content.anchoredPosition = new Vector2(content.anchoredPosition.x, value); })
            .Action(() =>
            {
                Path.EasingBounceEaseOut(0.5f, overshootPositionY, targetPositionY,
                    value => { content.anchoredPosition = new Vector2(content.anchoredPosition.x, value); });
            });
    }


    [OnDestroy]
    public void Destroy()
    {
        Settings.Model.EventManager.RemoveAction("StartScrolling", StartScrolling);
        Settings.Model.EventManager.RemoveAction("StopScrolling", StopScrolling);
    }
}