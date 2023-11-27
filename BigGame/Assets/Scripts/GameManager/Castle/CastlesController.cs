using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CastlesController : MonoBehaviour
{
    public static CastlesController Instance;
    public Castle playerCastle;
    public Castle enemyCastle;
    [SerializeField][Tag] private string[] tags;
    private int tagId = -1;
    private void Awake()
    {
        Instance = this;
        tagId = -1;
    }

    public string ReturnNextFreeTag()
    {
        tagId++;
        if (tags.Count()<=tagId)
        {
            tagId = 0;
        }
        return tags[tagId];
    }

    public void GatesInitialization()
    {
        enemyCastle.GetGates();
        enemyCastle.SetGates();
        playerCastle.GatesInitialization();
        
    }

}
