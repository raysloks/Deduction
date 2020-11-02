using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TabNavigation : MonoBehaviour
{
    void Update()
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
                    if (target != null)
                        target.Select();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            GameObject gameObject = EventSystem.current.currentSelectedGameObject;
            if (gameObject != null)
            {
                Selectable selectable = gameObject.GetComponent<Selectable>();
                if (selectable != null)
                {
                    Selectable target = selectable.FindSelectableOnDown();
                    if (target != null)
                        target.OnPointerDown(new PointerEventData(EventSystem.current));
                }
            }
        }
    }
}
