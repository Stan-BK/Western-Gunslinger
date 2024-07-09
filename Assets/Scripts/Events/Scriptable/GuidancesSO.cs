using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/GuidanceListSO")]
public class GuidancesSO : ScriptableObject
{
    public List<Guidance> Guidances = new List<Guidance>();
}

