using System;
using BepInEx;
using BepInEx.Configuration;
using Jotunn.Managers;

namespace SmelterConfig
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    internal class SmelterConfigPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "org.bepinex.plugins.smelter.config";
        public const string PluginName = "SmelterConfig";
        public const string PluginVersion = "0.1.0";

        private static ConfigEntry<int> _smelterMaxOreInputCapacity;
        private static ConfigEntry<int> _smelterMaxFuelInputCapacity;
        private static ConfigEntry<int> _smelterFuelUsedPerProduct;
        private static ConfigEntry<int> _smelterSecondsPerProduct;

        private void Awake()
        {
            const string smelterMaxOreInputCapacityDescription = "How much ore you can put at once into the smelter.";
            _smelterMaxOreInputCapacity = Config.Bind(PluginName, "Maximum Ore Input Capacity", 10,
                smelterMaxOreInputCapacityDescription);

            const string smelterMaxFuelInputCapacityDescription =
                "How much fuel (coal) you can put at once into the smelter.";
            _smelterMaxFuelInputCapacity = Config.Bind(PluginName, "Maximum Fuel Input Capacity", 20,
                smelterMaxFuelInputCapacityDescription);

            const string smelterFuelPerProductDescription = "How many fuel items are consumed for one output product.";
            _smelterFuelUsedPerProduct = Config.Bind(PluginName, "Fuel Per Product Consumption", 2,
                smelterFuelPerProductDescription);

            const string smelterSecondsPerProductDescription =
                "How many seconds it will take to create one output product.";
            _smelterSecondsPerProduct = Config.Bind(PluginName, "Seconds Per Product Duration", 30,
                smelterSecondsPerProductDescription);

            ItemManager.OnItemsRegistered += ConfigureSmelter;
        }

        private static void ConfigureSmelter()
        {
            try
            {
                var smelterPrefab = PrefabManager.Cache.GetPrefab<Smelter>("smelter");
                smelterPrefab.m_maxOre = _smelterMaxOreInputCapacity.Value;
                smelterPrefab.m_maxFuel = _smelterMaxFuelInputCapacity.Value;
                smelterPrefab.m_fuelPerProduct = _smelterFuelUsedPerProduct.Value;
                smelterPrefab.m_secPerProduct = _smelterSecondsPerProduct.Value;
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