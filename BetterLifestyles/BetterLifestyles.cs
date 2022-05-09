using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using AIProject;
using AIProject.Definitions;
using System;
using System.Collections.Generic;

namespace BetterLifestyles
{
    [BepInPlugin(GUID, Name, Version)]
    [BepInProcess("AI-Syoujyo")]
    public partial class HardcoreMode : BaseUnityPlugin
    {
        const string GUID = "com.animal42069.betterlifestyles";
        const string Name = "Better Lifestyles";
        const string Version = "1.0.0";
        internal static ConfigEntry<float> MinimumLifestyleDesire { get; set; }

        public void Awake()
        {

            MinimumLifestyleDesire = Config.Bind("Settings", "Minimum Lifestyle Desire", 30f, new ConfigDescription("Lowest value a lifestyle desire can be.  Higher value means lifestyle desires will be performed more often", new AcceptableValueRange<float>(0f, 100f)));

            Harmony.CreateAndPatchAll(typeof(HardcoreMode));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(AgentActor), "SetDesire")]
        public static void AgentActor_SetDesire(AgentActor __instance, int key, float desireValue)
        {
            if (desireValue > MinimumLifestyleDesire.Value)
                return;

            var lifestyle = __instance.ChaControl.fileGameInfo.lifestyle;
            if (lifestyle < 0)
                return;

            Dictionary<int, float> desireTable = __instance.AgentData.DesireTable;
            if (desireTable == null)
                return;

            switch (lifestyle)
            {
                case 0:
                    if (key == Desire.GetDesireKey(Desire.Type.Eat) || key == Desire.GetDesireKey(Desire.Type.Want))
                        desireTable[key] = MinimumLifestyleDesire.Value;
                    break;
                case 1:
                    if (key == Desire.GetDesireKey(Desire.Type.Gift) || key == Desire.GetDesireKey(Desire.Type.Lonely))
                        desireTable[key] = MinimumLifestyleDesire.Value;
                    break;
                case 2:
                    if (key == Desire.GetDesireKey(Desire.Type.Cook) || key == Desire.GetDesireKey(Desire.Type.Hunt))
                        desireTable[key] = MinimumLifestyleDesire.Value;
                    break;
                case 3:
                    if (key == Desire.GetDesireKey(Desire.Type.Sleep) || key == Desire.GetDesireKey(Desire.Type.Break))
                        desireTable[key] = MinimumLifestyleDesire.Value;
                    break;
                case 4:
                    if (key == Desire.GetDesireKey(Desire.Type.Location) || key == Desire.GetDesireKey(Desire.Type.H))
                        desireTable[key] = MinimumLifestyleDesire.Value;
                    break;
                case 5:
                    if (key == Desire.GetDesireKey(Desire.Type.Game) || key == Desire.GetDesireKey(Desire.Type.Bath))
                        desireTable[key] = MinimumLifestyleDesire.Value;
                    break;
                default:
                    break;
            }
        }
    }
}
