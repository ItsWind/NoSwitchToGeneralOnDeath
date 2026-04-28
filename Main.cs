using Vintagestory.API.Client;
using Vintagestory.API.Common;
using HarmonyLib;
using Vintagestory.Client.NoObf;
using Vintagestory.API.Datastructures;

namespace NoSwitchToGeneralOnDeath;

public class Main : ModSystem {
    Harmony harmony = null;
    public static ICoreClientAPI API = null;
    public static ClientMain Game = null;
    private static int? SwitchToChatGroupID = null;

    private static void SwitchToChatTab(HudDialogChat chatBox, int groupIDToSwitchTo) {
        Game.currentGroupid = groupIDToSwitchTo;

        Traverse chatBoxTrav = Traverse.Create(chatBox);

        //int tabIndex = chatBox.tabIndexByGroupId(game.currentGroupid);
        int tabIndex = (int)chatBoxTrav.Method("tabIndexByGroupId", Game.currentGroupid).GetValue<object>();

        GuiElementHorizontalTabs chatTabs = chatBox.Composers["chat"].GetHorizontalTabs("tabs");

        chatTabs.TabHasAlarm[tabIndex] = false;
        if (!Game.ChatHistoryByPlayerGroup.ContainsKey(Game.currentGroupid)) {
            Game.ChatHistoryByPlayerGroup[Game.currentGroupid] = new LimitedList<string>(30);
        }

        //chatBox.UpdateText();
        chatBoxTrav.Method("UpdateText").GetValue<object>();

        chatTabs.activeElement = tabIndex;
    }

    public static void SwitchToSavedChatTab(HudDialogChat chatBox) {
        if (SwitchToChatGroupID == null)
            return;

        SwitchToChatTab(chatBox, (int)SwitchToChatGroupID);

        SwitchToChatGroupID = null;
    }

    public static void SaveCurrentChatTabForSwitch() {
        SwitchToChatGroupID = Game.currentGroupid;
    }

    private void LoadModConfig() {
        string fileName = "NoSwitchToGeneralOnDeath.json";

        try {
            ModConfig loaded;
            if ((loaded = API.LoadModConfig<ModConfig>(fileName)) == null)
                API.StoreModConfig<ModConfig>(ModConfig.Loaded, fileName);
            else
                ModConfig.Loaded = loaded;
        }
        catch {
            API.StoreModConfig<ModConfig>(ModConfig.Loaded, fileName);
        }
    }

    public void OnPlayerDies(IClientPlayer player) {
        SaveCurrentChatTabForSwitch();
    }

    public override void StartClientSide(ICoreClientAPI api) {
        API = api;
        Game = (ClientMain)API.World;

        LoadModConfig();

        api.Event.PlayerDeath += OnPlayerDies;

        harmony = new Harmony(Mod.Info.ModID);
        harmony.PatchAll();
    }

    public override void Dispose() {
        API.Event.PlayerDeath -= OnPlayerDies;

        harmony?.UnpatchAll(Mod.Info.ModID);
    }
}
