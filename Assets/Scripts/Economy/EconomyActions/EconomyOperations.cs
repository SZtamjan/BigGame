using System.Reflection;
using UnityEngine;

namespace Economy.EconomyActions
{
    public static class EconomyOperations
    {
        #region Purchase

        /// <summary>
        ///   <para>Returns true if Purchase was successful</para>
        ///   <para>Shows notification by default</para>
        /// </summary>
        /// <param name="unitCostStruct">Unit cost to make operations on</param>
        public static bool Purchase(ResourcesStruct unitCostStruct)
        {
            if (!CheckIfICanIAfford(unitCostStruct,false))
            {
                EconomyConditions.Instance.NotEnoughResources();
                return false;
            }
            
            SubtractStruct(unitCostStruct);
            return true;
        }
        
        /// <summary>
        ///   <para>Returns true if Purchase was successful</para>
        /// </summary>
        /// <param name="unitCostStruct">Unit cost to make operations on</param>
        /// <param name="notificationBoolean">Blean to display or not display a notification about insufficient resources</param>
        public static bool Purchase(ResourcesStruct unitCostStruct,bool notificationBoolean)
        {
            if (!CheckIfICanIAfford(unitCostStruct,false))
            {
                if(notificationBoolean) EconomyConditions.Instance.NotEnoughResources();
                return false;
            }
            
            SubtractStruct(unitCostStruct);
            return true;
        }

        #endregion

        #region CheckIfICanAfford

        /// <summary>
        ///   <para>Returns true if check was successful</para>
        ///   <para>Shows notification by default</para>
        /// </summary>
        /// <param name="unitCostStruct">Unit cost to calculate</param>
        public static bool CheckIfICanIAfford(ResourcesStruct unitCostStruct)
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
                    EconomyConditions.Instance.NotEnoughResources();
                    Debug.Log("Nie stać mnie" + unitCostStruct);
                    return false;
                }
            }
            
            return true;
        }
        
        /// <summary>
        ///   <para>Returns true if check was successful</para>
        /// </summary>
        /// <param name="unitCostStruct">Unit cost to calculate</param>
        /// <param name="showNotification">Boolean to display or not display a notification about insufficient resources</param>
        public static bool CheckIfICanIAfford(ResourcesStruct unitCostStruct, bool showNotification)
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
                    if(showNotification) EconomyConditions.Instance.NotEnoughResources();
                    Debug.Log("Nie stać mnie, potrzeba " + field.GetValue(unitCostStruct) + " aby to kupic");
                    return false;
                }
            }
            
            return true;
        }

        #endregion

        #region SubtractMoney

        public static void SubtractStruct(ResourcesStruct costStruct)
        {
            PropertyInfo[] fields = typeof(ResourcesStruct).GetProperties(BindingFlags.Instance |
                                                                          BindingFlags.NonPublic |
                                                                          BindingFlags.Public);
            
            foreach (var field in fields)
            {
                int costValue = (int)field.GetValue(costStruct);
                int economyResourcesValue = (int)field.GetValue(EconomyResources.Instance.Resources);

                int newValue = economyResourcesValue - costValue;

                field.SetValue(EconomyResources.Instance.Resources,newValue);
            }
        }

        #endregion
        
        #region AddMoney

        /// <summary>
        ///   <para>Adds resources</para>
        /// </summary>
        /// <param name="newResources">New resources to add</param>
        public static void AddResources(ResourcesStruct newResources)
        {
            PropertyInfo[] fields = typeof(ResourcesStruct).GetProperties(BindingFlags.Instance |
                                                                          BindingFlags.NonPublic |
                                                                          BindingFlags.Public);
            
            foreach (var field in fields)
            {
                int unitFieldValue = (int)field.GetValue(newResources);
                int economyResourcesValue = (int)field.GetValue(EconomyResources.Instance.Resources);

                int newValue = economyResourcesValue + unitFieldValue;

                field.SetValue(EconomyResources.Instance.Resources,newValue);
            }
        }

        #endregion
    }
}