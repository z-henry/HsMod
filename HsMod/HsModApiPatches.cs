using HarmonyLib;
using PegasusUtil;
using System;
using System.Collections.Generic;

namespace HsMod
{
    public class HsModApiPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Hearthstone.Progression.QuestXpRewardHandler), "Initialize")]
        public static void QuestXpRewardHandlerInitialize(List<RewardTrackXpChange> xpChanges, Action callback)
        {
            HsModApiCache.RecordXpChanges(xpChanges);
        }
    }
}
