using HarmonyLib;
using Vintagestory.Client.NoObf;

namespace NoSwitchToGeneralOnDeath;

[HarmonyPatch(typeof(HudDialogChat), "OnNewServerToClientChatLine")]
public class HudDialogChatOnNewServerToClientChatLinePatch {
    [HarmonyPostfix]
    public static void Postfix(HudDialogChat __instance) {
        if (NoSwitchToGeneralOnDeathModSystem.SwitchToChatGroupID == null)
            return;

        NoSwitchToGeneralOnDeathModSystem.SwitchToChatTab(__instance, (int)NoSwitchToGeneralOnDeathModSystem.SwitchToChatGroupID);

        NoSwitchToGeneralOnDeathModSystem.SwitchToChatGroupID = null;
    }
}
