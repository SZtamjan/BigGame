using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastlesController : MonoBehaviour
{
    public static CastlesController Instance;
    public Castle playerCastle;
    public Castle enemyCastle;
    private void Awake()
    {
        Instance = this;
    }
}
