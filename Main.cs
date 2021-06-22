using ICities;
using CitiesHarmony.API;

namespace BusOnlyRoadEnabler
{
    public class Mod : IUserMod
    {
        public string Name => "Bus-Only Road Enabler";
        public string Description => "Makes bus-only roads possible.";

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled()
        {
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }

        public static string Identifier = "WQ.BORE/";
    }
}
