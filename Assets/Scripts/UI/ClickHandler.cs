using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickHandler : MonoBehaviour
{
    public UnityEvent<OperatorOption> ClickEvent;
    public OperatorOption Option;

    public void OnClick()
    {
        ClickEvent?.Invoke(Option);
    }
}
