using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;
    

    [Tooltip("tu ma byæ ustawione UI"),SerializeField]
    private LayerMask UiLayer;
    [Tag,Tooltip("tu ma myæ ustawiony tag: CARD"), SerializeField]
    private String CardTag;
    [Tooltip("tu ma byæ ustawiona Camera UI"), SerializeField]
    private Camera UiCam;


    //testing variables

    public List<SpellsScrptableObject> spele;
    public int element = 0;
    public bool playerUnits = true;
    public int power = 1;

    // end of testing variables

    public Coroutine _SelectFile;

    [Button]
    private void TestShieldAll()
    {
        spele[element].SpellAction(null, playerUnits, power); ;
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

    public void SelectCard(Action<GameObject> onComplete)
    {
        if (_SelectFile != null)
        {
            StopCoroutine(_SelectFile);
        }
        _SelectFile = StartCoroutine(SelectCardOnhand(onComplete));
    }

    private IEnumerator SelectCardOnhand(Action<GameObject> onComplete)
    {

        GameObject card = null;

        while (true)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                //Ray2D ray = UiCam.ScreenPointToRay(Input.mousePosition);
                //Ray2D ray = UiCam.ScreenToWorldPoint(Input.mousePosition);
                Ray ray = UiCam.ScreenPointToRay(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction,500f);

                if (hit)
                {
                    
                    card = hit.collider.gameObject;
                    if (card.CompareTag(CardTag))
                    {
                        onComplete.DynamicInvoke(card);
                        break;
                    }
                    else
                    {
                        onComplete.DynamicInvoke(null);
                        break;
                    }
                    
                }

                onComplete.DynamicInvoke(card);
                break;
            }



            yield return null;
        }
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
                            onComplete.DynamicInvoke(tile);
                            // tu wy³aczenie koloru
                            break;
                        }
                        tile = GetTile(hit.transform.position, thisGatePatch);
                        onComplete.DynamicInvoke(tile);

                    }
                    onComplete.DynamicInvoke(tile);
                    break;
                }
                onComplete.DynamicInvoke(tile);
                break;
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

    private PathClass.Path GetTile(Vector3 pos, Gate gate)
    {

        foreach (var item in gate.path)
        {
            if (item.position.z == pos.z && item.position.x == pos.x)
            {
                return item;
            }
        }
        return null;
    }


}


