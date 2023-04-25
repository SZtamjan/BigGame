using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ProTips", menuName = "ProTips")]

public class ProtipText : ScriptableObject
{
    [Header("Tips")]
    public List<string> proTips = new List<string>();
}
