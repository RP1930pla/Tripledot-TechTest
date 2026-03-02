using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BottomBarView : MonoBehaviour
{

    List<BottomBarButton> buttons;

    void Closed() 
    {
        Debug.Log("Event: Closed");
    }

    public bool ReturnFalseIfNoContent() 
    {
        if (buttons != null) 
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].buttonEnabled)
                {
                    return true;
                }
            }
            return false;
        }

        return false;
    }

    public void CheckForNoContent() 
    {
        if (ReturnFalseIfNoContent() == false)
        {
            Closed();
        }
    }

    private void Start()
    {
        CheckForNoContent();
    }

    public void ContentActivated() 
    {
        Debug.Log("Event: Content Activated");
    }

    public void Subscribe(BottomBarButton button)
    {
        if (buttons == null) 
        {
            buttons = new List<BottomBarButton>();
        }

        buttons.Add(button);
    }

    public void Unsubscribe(BottomBarButton button) 
    {
        if (buttons == null)
        {
            buttons = new List<BottomBarButton>();
        }

        buttons.Remove(button);
    }

    public void CloseOtherEnabledButtons() 
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].enabled) 
            {
                buttons[i].CloseButton();
            }
        }
    }

}
