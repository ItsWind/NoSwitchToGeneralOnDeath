using Vintagestory.API.Client;
using Vintagestory.API.Common;
using HarmonyLib;
using Vintagestory.Client.NoObf;
using Vintagestory.API.Datastructures;

namespace NoSwitchToGeneralOnDeath;

public class NoSwitchToGeneralOnDeathModSystem : ModSystem {
    Harmony harmony = null;
    public static ICoreClientAPI API = null;
    public static int? SwitchToChatGroupID = null;

    public static void SwitchToChatTab(HudDialogChat chatBox, int groupIDToSwitchTo) {
        ClientMain game = (ClientMain)API.World;

        game.currentGroupid = groupIDToSwitchTo;

        Traverse chatBoxTrav = Traverse.Create(chatBox);

        //int tabIndex = chatBox.tabIndexByGroupId(game.currentGroupid);
        int tabIndex = (int)chatBoxTrav.Method("tabIndexByGroupId", game.currentGroupid).GetValue();

        GuiElementHorizontalTabs chatTabs = chatBox.Composers["chat"].GetHorizontalTabs("tabs");

        chatTabs.TabHasAlarm[tabIndex] = false;
        if (!game.ChatHistoryByPlayerGroup.ContainsKey(game.currentGroupid)) {
            game.ChatHistoryByPlayerGroup[game.currentGroupid] = new LimitedList<string>(30);
        }

        //chatBox.UpdateText();
        chatBoxTrav.Method("UpdateText").GetValue();

        chatTabs.activeElement = tabIndex;
    }

    public override void StartClientSide(ICoreClientAPI api) {
        API = api;

        harmony = new Harmony(Mod.Info.ModID);
        harmony.PatchAll();
    }

    public override void Dispose() {
        harmony?.UnpatchAll(Mod.Info.ModID);
    }
}
