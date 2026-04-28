using HarmonyLib;
using Vintagestory.Client.NoObf;

namespace NoSwitchToGeneralOnDeath;

[HarmonyPatch(typeof(HudDialogChat), "OnNewServerToClientChatLine")]
public class HudDialogChatOnNewServerToClientChatLinePatch {
    private static string lastMessageSent = null;

    private static void DoRuinedClutterNoSwitchOrChangeToCurrentTab(ref int groupId) {
        if (ModConfig.Loaded.MoveRuinedClutterMessagesToCurrentTab)
            groupId = Main.Game.currentGroupid;
        else if (ModConfig.Loaded.NoSwitchOnRuinedClutterMessages)
            Main.SaveCurrentChatTabForSwitch();
    }

    // repaired a little 17 + 1space
    // shattered 9 + 1space
    [HarmonyPrefix]
    public static void Prefix(ref int groupId, string message) {
        Main.API.Logger.Debug("BEFORE OnNewServerToClientChatLine");

        // Check if current message sent is last message for death repeats.
        if (lastMessageSent != null && message == lastMessageSent) {
            Main.SaveCurrentChatTabForSwitch();
        }
        lastMessageSent = message;

        // Return if message is less than 10 characters, as it isn't either of the checks
        if (message.Length < 10)
            return;

        // Ruined bed shattered 20 
        string checkForShatteredStr = message.Substring(message.Length - 10);
        if (checkForShatteredStr == " shattered") {
            DoRuinedClutterNoSwitchOrChangeToCurrentTab(ref groupId);
            return;
        }

        // If message is not shattered and is less than 18 characters, it's not repaired a little
        if (message.Length < 18)
            return;

        string checkForRepairedStr = message.Substring(message.Length - 18);
        if (checkForRepairedStr == " repaired a little") {
            DoRuinedClutterNoSwitchOrChangeToCurrentTab(ref groupId);
            return;
        }
    }

    [HarmonyPostfix]
    public static void Postfix(HudDialogChat __instance) {
        Main.SwitchToSavedChatTab(__instance);
    }
}
