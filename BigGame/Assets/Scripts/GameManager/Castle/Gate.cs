using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static PathClass;
[SelectionBase]
public class Gate : MonoBehaviour
{
    [SerializeField] private bool isPlayerSide = false;

    public List<Path> paths = new List<Path>();
    [SerializeField][Tooltip("dobrze dzia³a 1.3")] private float searchRadius = 0.8f;


    [SerializeField]
    private Gate _secondGastle;
    private bool _end = true;
    [Button]
    public void GeneratePath()
    {
        paths = new List<Path> { new Path { position = transform.position } };
        _secondGastle = NewPath().GetComponent<Gate>();

        paths.Add(new Path { position = _secondGastle.transform.position});

        _secondGastle.SetSecoundGate(this);
        _secondGastle.SetPath(paths);

    }

    public void SetSecoundGate(Gate gate)
    {
        _secondGastle = gate;
    }
    public void SetPath(List<Path> path)
    {
        paths = path;
    }
    public void SetIfplayerOrNot(bool isPlayer)
    {
        isPlayerSide = isPlayer;
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
            paths.Add(new Path { position = item.transform.position });
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


}
