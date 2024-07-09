using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour
{
    private static List<Button> _buttons = new ();
    public UnityEvent<OperatorOption> ClickEvent;
    public OperatorOption Option;

    private void Start()
    {
        _buttons.Add(GetComponent<Button>());
    }

    public static void SetInteractiveOnly([CanBeNull] Button btn)
    {
        foreach (var button in _buttons)
        {
            if (btn != button)
            {
                button.interactable = false;
            }
        }
    }

    public static void InteractiveAll()
    {
        foreach (var button in _buttons)
        {
            button.interactable = true;
        }
    }
    
    public void OnClick()
    {
        ClickEvent?.Invoke(Option);
    }
}
