using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public static class SavePaths
{
    public static string levelPath = Application.persistentDataPath + "/level.xd";
    public static string tipPath = Application.persistentDataPath + "/tip.pog";
}
