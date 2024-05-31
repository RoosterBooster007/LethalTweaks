using HarmonyLib;
using LethalTweaks;

namespace LethalTools.Patches
{
    [HarmonyPatch(typeof(StunGrenadeItem))]
    internal class GrenadesPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void patchGrenades(ref bool ___hasExploded)
        {
            if ((bool) TweaksBase.unStun.BoxedValue)
            {
                ___hasExploded = true; // stops grenades from exploding
            }
        }
    }

    [HarmonyPatch(typeof(Turret))]
    internal class TurretsPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void patchTurrets(ref bool ___turretActive)
        {
            if ((bool)TweaksBase.disableTurrets.BoxedValue)
            {
                ___turretActive = false; // deactivates all turrets
            }
        }
    }

    [HarmonyPatch(typeof(Landmine))]
    internal class MinesPatch
    {
        [HarmonyPatch("Hit")]
        [HarmonyPrefix]
        private static void patchMines(ref bool __result)
        {
            if ((bool)TweaksBase.disableTurrets.BoxedValue)
            {
                __result = false; // deactivates all mine hits
            }
        }
    }
}
