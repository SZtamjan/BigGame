using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Economy.EconomyActions
{
    public abstract class EconomyOperations : MonoBehaviour
    {
        /// <summary>
        ///   <para>Returns true if Purchase was successful</para>
        /// </summary>
        protected bool Purchase(ResourcesStruct unitCostStruct)
        {
            if (CanIAfford(unitCostStruct))
            {
                CalculateMoney();
                return true;
            }

            return false;
        }

        bool CanIAfford(ResourcesStruct unitCostStruct)
        {
            FieldInfo[] fields = typeof(ResourcesStruct).GetFields(BindingFlags.Instance |
                                                                   BindingFlags.NonPublic |
                                                                   BindingFlags.Public);
            
            foreach (var field in fields)
            {
                //object unitFieldValue = field.GetValue(unitCostStruct);
                //object economyResourcesValue = field.GetValue(EconomyResources.Instance.Resources);
                int unitFieldValue = (int)field.GetValue(unitCostStruct);
                int economyResourcesValue = (int)field.GetValue(EconomyResources.Instance.Resources);
                
                Debug.Log("xd" + unitFieldValue);
                Debug.Log("lol" + economyResourcesValue);

                if (economyResourcesValue < unitFieldValue)
                {
                    Debug.Log("Nie stać mnie" + unitCostStruct);
                }
                
            }
            
            return true;
        }

        void CalculateMoney()
        {
            
        }
    
    }
}