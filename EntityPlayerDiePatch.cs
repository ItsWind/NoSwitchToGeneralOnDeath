using HarmonyLib;
using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace NoSwitchToGeneralOnDeath;

/*
[HarmonyPatch(typeof(EntityPlayer), nameof(EntityPlayer.Die))]
public class EntityPlayerDiePatch {
    [HarmonyPrefix]
    public static bool Prefix() {
        ClientMain game = (ClientMain)NoSwitchToGeneralOnDeathModSystem.API.World;
        NoSwitchToGeneralOnDeathModSystem.SwitchToChatGroupID = game.currentGroupid;

        return true;
    }
}
*/