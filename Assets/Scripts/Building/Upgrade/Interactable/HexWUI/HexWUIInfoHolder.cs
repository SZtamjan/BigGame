using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HexWUIInfoHolder : MonoBehaviour
{
    [SerializeField] private GameObject hexWUIRoratable;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI food;
    [SerializeField] private TextMeshProUGUI wood;
    [SerializeField] private TextMeshProUGUI stone;
    
    public GameObject HexWuiRoratable => hexWUIRoratable;

    public TextMeshProUGUI Gold => gold;

    public TextMeshProUGUI Food => food;

    public TextMeshProUGUI Wood => wood;

    public TextMeshProUGUI Stone => stone;
}
