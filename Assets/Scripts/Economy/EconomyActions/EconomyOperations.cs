using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Economy.EconomyActions
{
    public class EconomyOperations : MonoBehaviour
    {
        /// <summary>
        ///   <para>Returns true if Purchase was successful</para>
        /// </summary>
        protected bool Purchase(ResourcesStruct unitCostStruct)
        {
            if (!CheckIfICanIAfford(unitCostStruct))
            {
                EconomyConditions.Instance.NotEnoughResources();
                return false;
            }
            
            CalculateMoney(unitCostStruct);
            return true;
        }

        protected bool CheckIfICanIAfford(ResourcesStruct unitCostStruct)
        {
            PropertyInfo[] fields = typeof(ResourcesStruct).GetProperties(BindingFlags.Instance |
                                                                          BindingFlags.NonPublic |
                                                                          BindingFlags.Public);
            
            foreach (var field in fields)
            {
                int unitFieldValue = (int)field.GetValue(unitCostStruct);
                int economyResourcesValue = (int)field.GetValue(EconomyResources.Instance.Resources);
                
                if (economyResourcesValue < unitFieldValue)
                {
                    Debug.Log("Nie stać mnie" + unitCostStruct);
                    return false;
                }
            }
            
            return true;
        }

        void CalculateMoney(ResourcesStruct unitCostStruct)
        {
            PropertyInfo[] fields = typeof(ResourcesStruct).GetProperties(BindingFlags.Instance |
                                                                          BindingFlags.NonPublic |
                                                                          BindingFlags.Public);
            
            foreach (var field in fields)
            {
                int unitFieldValue = (int)field.GetValue(unitCostStruct);
                int economyResourcesValue = (int)field.GetValue(EconomyResources.Instance.Resources);

                int newValue = economyResourcesValue - unitFieldValue;

                field.SetValue(EconomyResources.Instance.Resources,newValue);
            }
        }
    }
}