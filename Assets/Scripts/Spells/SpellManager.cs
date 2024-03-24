using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;
    public List<SpellsScrptableObject> spele;
    public int element = 0;
    public bool playerUnits = true;
    public int power = 1;

    public Coroutine _SelectFile;

    [Button]
    private void TestShieldAll()
    {
        spele[element].SpellAction(null,playerUnits, power);;
    }


    private void Awake()
    {
        Instance = this;
    }


    public void SelectTile(Action<PathClass.Path> onComplete)
    {
        
        if (_SelectFile != null)
        {
            StopCoroutine(_SelectFile);
        }
        _SelectFile = StartCoroutine(SelectTileToFireSelector(onComplete));
    }
    private IEnumerator SelectTileToFireSelector(Action<PathClass.Path> onComplete)
    {
        PathClass.Path tile = null;
        
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        Gate thisGatePatch = GetPath(hit.collider.tag);
                        if (thisGatePatch == null)
                        {
                            break;
                        }
                        tile = GetTile(hit.transform.position, thisGatePatch);
                        onComplete.DynamicInvoke(tile);
                        
                    }
                    
                    break;
                }
                
            }
            yield return null;
        }
        
        _SelectFile = null;
       
        yield return null;
    }

    private Gate GetPath(string tag)
    {
        foreach (var item in CastlesController.Instance.playerCastle.gates)
        {
            if (item.CompareTag(tag))
            {
                return item;
            }
        }

        return null;
    }

    private PathClass.Path GetTile(Vector3 pos,Gate gate)
    {

        foreach (var item in gate.path)
        {
            if (item.position.z==pos.z && item.position.x==pos.x)
            {
                return item;
            }
        }
        return null;
    }


}


