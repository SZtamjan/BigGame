using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static PathClass;
[SelectionBase]
public class Gate : MonoBehaviour
{
    [SerializeField] private bool isPlayerSide = false;

    public List<Path> path = new List<Path>();
    [SerializeField][Tooltip("dobrze dzia³a 0.8")] private float searchRadius = 0.8f;

    [Tag] public string newTag;
    [SerializeField] private Gate _secondGastle;
    [SerializeField] private Castle _MyCastle;

    // tutaj coœ dodaje ziom

    [Button]
    public void GeneratePath()
    {
        newTag = CastlesController.Instance.ReturnNextFreeTag();
        gameObject.tag = newTag;
        path = new List<Path> { new Path { position = transform.position } };
        _secondGastle = NewPath().GetComponent<Gate>();
        _secondGastle.tag = newTag;
        path.Add(new Path { position = _secondGastle.transform.position });

        _secondGastle.SetSecoundGate(this);
        _secondGastle.SetPath(path);

    }


    #region path creation
   
    public void SetPath(List<Path> path)
    {
        this.path = path;
    }
    public void SetIfplayerOrNot(bool isPlayer)
    {
        isPlayerSide = isPlayer;
    }

    public void SetSecoundGate(Gate gate)
    {
        _secondGastle = gate;
    }

    public void SetMyCastle(Castle castle)
    {
        _MyCastle = castle;
    }

    public GameObject NewPath()
    {
        GameObject toReturn = null;

        List<GameObject> ControlList = new List<GameObject>();

        List<GameObject> hits = GetHits(transform.position, "Path");
        hits = RemoveDuplicatesFromList(hits, ControlList);
        ControlList.AddRange(hits);
        int loops = 0;
        while (loops < 1000)
        {
            hits = GetHits(ControlList.Last().transform.position, "Path");
            if (hits.Count == 0)
            {
                break;
            }
            hits = RemoveDuplicatesFromList(hits, ControlList);
            ControlList.AddRange(hits);


            loops++;
        }


        foreach (var item in ControlList)
        {
            item.tag = newTag;
            path.Add(new Path { position = item.transform.position });
        }
        
        toReturn = GetHits(ControlList.Last().transform.position, "Gate").Last();


        return toReturn;

    }

    List<GameObject> GetHits(Vector3 position, string tag)
    {
        List<GameObject> hits = new List<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(position, searchRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(tag))
            {
                hits.Add(collider.gameObject);
            }
        }

        return hits;
    }

    List<GameObject> RemoveDuplicatesFromList(List<GameObject> sourceList, List<GameObject> referenceList)
    {
        List<GameObject> duplicates = new List<GameObject>();

        foreach (GameObject item in sourceList)
        {
            if (referenceList.Contains(item))
            {
                duplicates.Add(item);
            }
        }

        foreach (GameObject duplicate in duplicates)
        {
            sourceList.Remove(duplicate);
        }

        return sourceList;
    }

    #endregion
}
