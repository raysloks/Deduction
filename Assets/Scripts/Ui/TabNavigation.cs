using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TabNavigation : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject gameObject = EventSystem.current.currentSelectedGameObject;
            if (gameObject != null)
            {
                Selectable selectable = gameObject.GetComponent<Selectable>();
                if (selectable != null)
                {
                    bool shift = Input.GetKey(KeyCode.LeftShift);
                    Selectable target = shift ? selectable.FindSelectableOnUp() : selectable.FindSelectableOnDown();
                    if (target == null)
                    {
                        Selectable next = selectable;
                        if (shift)
                            while ((next = next.FindSelectableOnDown()) != null)
                                target = next;
                        else
                            while ((next = next.FindSelectableOnUp()) != null)
                                target = next;
                    }
                    if (target == null)
                    {
                        target = shift ? selectable.FindSelectableOnLeft() : selectable.FindSelectableOnRight();
                        if (target == null)
                        {
                            Selectable next = selectable;
                            if (shift)
                                while ((next = next.FindSelectableOnRight()) != null)
                                    target = next;
                            else
                                while ((next = next.FindSelectableOnLeft()) != null)
                                    target = next;
                        }
                    }
                    if (target != null)
                    {
                        target.Select();
                        ScrollToItem(target.GetComponent<RectTransform>());
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            GameObject gameObject = EventSystem.current.currentSelectedGameObject;
            if (gameObject != null)
            {
                Selectable selectable = gameObject.GetComponent<Selectable>();
                if (selectable is InputField)
                {
                    Selectable target = selectable.FindSelectableOnDown();
                    if (target != null)
                    {
                        target.Select();
                        ScrollToItem(target.GetComponent<RectTransform>());
                        if (target is Button button)
                            button.OnSubmit(new BaseEventData(EventSystem.current));
                    }
                    else
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                    }
                }
            }
        }
    }

    private void ScrollToItem(RectTransform item)
    {
        if (item == null)
            return;

        ScrollRect scrollRect = item.GetComponentInParent<ScrollRect>();
        if (scrollRect == null)
            return;
        RectTransform content = scrollRect.content;

        Vector3[] corners = new Vector3[4];
        item.GetWorldCorners(corners);
        Vector2 itemPositionMin = scrollRect.transform.InverseTransformPoint(corners[0]);
        Vector2 itemPositionMax = scrollRect.transform.InverseTransformPoint(corners[2]);
        Rect rect = scrollRect.GetComponent<RectTransform>().rect;

        if (itemPositionMax.y > rect.height / 2f)
            content.localPosition = new Vector2(content.localPosition.x, content.localPosition.y - itemPositionMax.y + rect.height / 2f);
        if (itemPositionMin.y < -rect.height / 2f)
            content.localPosition = new Vector2(content.localPosition.x, content.localPosition.y - itemPositionMin.y - rect.height / 2f);
    }
}
