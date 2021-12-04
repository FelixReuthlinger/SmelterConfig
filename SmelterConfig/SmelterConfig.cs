using System;
using BepInEx;
using BepInEx.Configuration;
using Jotunn.Managers;

namespace SmelterConfig
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class SmelterConfigPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "org.bepinex.plugins.smelter.config";
        public const string PluginName = "SmelterConfig";
        public const string PluginVersion = "0.1.0";

        public static ConfigEntry<int> SmelterMaxOreInputCapacity;
        public static ConfigEntry<int> SmelterMaxFuelInputCapacity;
        public static ConfigEntry<int> SmelterFuelUsedPerProduct;
        public static ConfigEntry<int> SmelterSecondsPerProduct;

        private void Awake()
        {
            string sectionName = "SmelterConfig";

            string SmelterMaxOreInputCapacityDescription = "How much ore you can put at once into the smelter.";
            SmelterMaxOreInputCapacity = Config.Bind(sectionName, "Maximum Ore Input Capacity", 10,
                SmelterMaxOreInputCapacityDescription);

            string SmelterMaxFuelInputCapacityDescription =
                "How much fuel (coal) you can put at once into the smelter.";
            SmelterMaxFuelInputCapacity = Config.Bind(sectionName, "Maximum Fuel Input Capacity", 10,
                SmelterMaxFuelInputCapacityDescription);
            
            string SmelterFuelPerProductDescription =
                "How many fuel items are consumed for one output product.";
            SmelterFuelUsedPerProduct = Config.Bind(sectionName, "Fuel Per Product Consumption", 4,
                SmelterFuelPerProductDescription);
            
            string SmelterSecondsPerProductDescription =
                "How many seconds it will take to create one output product.";
            SmelterSecondsPerProduct = Config.Bind(sectionName, "Seconds Per Product Duration", 10,
                SmelterSecondsPerProductDescription);

            ItemManager.OnItemsRegistered += ConfigureSmelter;
        }

        private void ConfigureSmelter()
        {
            try
            {
                Smelter smelterPrefab = PrefabManager.Cache.GetPrefab<Smelter>("smelter");
                smelterPrefab.m_maxOre = SmelterMaxOreInputCapacity.Value;
                smelterPrefab.m_maxFuel = SmelterMaxFuelInputCapacity.Value;
                smelterPrefab.m_fuelPerProduct = SmelterFuelUsedPerProduct.Value;
                smelterPrefab.m_secPerProduct = SmelterSecondsPerProduct.Value;
            }
            catch (Exception ex)
            {
                Jotunn.Logger.LogError($"Error while configuring smelter: {ex.Message}");
            }
            finally
            {
                ItemManager.OnItemsRegistered -= ConfigureSmelter;
            }
        }
    }
}