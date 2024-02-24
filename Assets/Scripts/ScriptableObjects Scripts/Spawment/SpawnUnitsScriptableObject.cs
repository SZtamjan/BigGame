using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Spawment/Units")]
public class SpawnUnitsScriptableObject : ScriptableObject
{
    [Tooltip("Co siê ma szance zespawniæ")]
    [SerializeField] private List<UnitScriptableObjects> Units;

    [SerializeField] public SelectMetchod Method;


    [ShowIf("Method", SelectMetchod.RandomWithNull)]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float nullPropability;
    public UnitScriptableObjects SelectUnit()
    {
        if (Units.Count == 0)
        {
            return null;
        }
        switch (Method)
        {
            case SelectMetchod.RandomNotNull:
                return Units[Random.Range(0, Units.Count)];
                break;
            case SelectMetchod.RandomWithNull:
                if (Random.Range(0f, 1f) > nullPropability)
                {
                    return Units[Random.Range(0, Units.Count)];
                }
                else
                {
                    return null;
                }
                break;
            default:
                return null;
                break;

        }
    }



    [SerializeField]
    public enum SelectMetchod
    {
        RandomNotNull,
        RandomWithNull
    }
}
