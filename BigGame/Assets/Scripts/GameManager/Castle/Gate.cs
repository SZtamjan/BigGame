using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PathClass;
[SelectionBase]
public class Gate : MonoBehaviour
{
    [SerializeField] private bool isPlayerSide = false;

    public List<Path> paths = new List<Path>();
    public float searchRadius = 1f;
    private string _targetTag = "Path";

    [Button]
    public void GeneratePath()
    {
        paths.Add(new Path { position = transform.position });
        int loops = 0;
        while (loops<100)
        {
            Path newPath = NewHits(paths.Last().position);
            if (newPath == null)
            {
                break;
            }
            loops++;
            paths.Add(newPath);
        }
    }

    public Path NewHits(Vector3 pos)
    {
        List<Path> hits = new List<Path>();
        List<Path> hitsSort = new List<Path>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        Debug.Log(_targetTag);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(_targetTag))
            {
                hits.Add(new Path { position = collider.transform.position });
            }
        }
       
        

        if (hits.Count != 0)
        {
            return hits.Last();
        }
        else
        {
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Gate"))
                {
                    new Path { position = collider.transform.position };
                }
            }
        }
        return null;
    }
}
