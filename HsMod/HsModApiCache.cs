using Assets;
using Hearthstone.Progression;
using Newtonsoft.Json;
using PegasusUtil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HsMod
{
    public static class HsModApiCache
    {
        private const int MaxXpChanges = 512;
        private static readonly object CacheLock = new object();
        private static readonly TimeSpan SnapshotInterval = TimeSpan.FromSeconds(1);
        private static readonly List<XpChangeInfo> XpChanges = new List<XpChangeInfo>();
        private static HsModState Snapshot = new HsModState();
        private static DateTime LastSnapshotUpdate = DateTime.MinValue;
        private static long NextSequence = 1;

        public static string GetStateJson(long afterSequence)
        {
            HsModState result;
            lock (CacheLock)
            {
                result = Snapshot.CloneWithoutChanges();
                result.XpChanges = XpChanges.Where(x => x.Sequence > afterSequence).ToList();
                result.NextSequence = XpChanges.Count == 0 ? afterSequence : XpChanges.Max(x => x.Sequence);
            }

            return JsonConvert.SerializeObject(result);
        }

        public static void RecordXpChanges(IEnumerable<RewardTrackXpChange> changes)
        {
            if (changes == null)
                return;

            List<XpChangeInfo> recorded = new List<XpChangeInfo>();
            foreach (RewardTrackXpChange change in changes)
            {
                if (change == null)
                    continue;

                int amount = CalculateXpChange(change);
                if (amount <= 0)
                    continue;

                recorded.Add(new XpChangeInfo
                {
                    Sequence = NextSequence++,
                    Time = DateTime.Now,
                    TrackType = GetTrackName(change.RewardTrackType),
                    SourceType = GetSourceName(change.RewardSourceType),
                    SourceId = change.RewardSourceId,
                    Amount = amount,
                    PrevLevel = change.PrevLevel,
                    PrevXp = change.PrevXp,
                    CurrLevel = change.CurrLevel,
                    CurrXp = change.CurrXp
                });
            }

            if (recorded.Count == 0)
                return;

            lock (CacheLock)
            {
                XpChanges.AddRange(recorded);
                if (XpChanges.Count > MaxXpChanges)
                    XpChanges.RemoveRange(0, XpChanges.Count - MaxXpChanges);
            }

            RefreshSnapshot(true);
        }

        public static void RefreshSnapshot(bool force)
        {
            DateTime now = DateTime.Now;
            if (!force && now - LastSnapshotUpdate < SnapshotInterval)
                return;

            HsModState snapshot = new HsModState
            {
                ServerTime = now,
                RewardTracks = new RewardTracksInfo
                {
                    Global = ReadTrack(Global.RewardTrackType.GLOBAL),
                    Battlegrounds = ReadTrack(Global.RewardTrackType.BATTLEGROUNDS),
                    Event = ReadEventTrack()
                },
                Ratings = ReadRatings()
            };

            lock (CacheLock)
            {
                Snapshot = snapshot;
                LastSnapshotUpdate = now;
            }
        }

        private static RewardTrackInfo ReadEventTrack()
        {
            try
            {
                Hearthstone.Progression.RewardTrack track = RewardTrackManager.Get()?.GetCurrentEventRewardTrack();
                return ReadTrack(track, "EVENT");
            }
            catch
            {
                return null;
            }
        }

        private static RewardTrackInfo ReadTrack(Global.RewardTrackType type)
        {
            try
            {
                Hearthstone.Progression.RewardTrack track = RewardTrackManager.Get()?.GetRewardTrack(type);
                return ReadTrack(track, type.ToString());
            }
            catch
            {
                return null;
            }
        }

        private static RewardTrackInfo ReadTrack(Hearthstone.Progression.RewardTrack track, string typeName)
        {
            if (track == null || track.TrackDataModel == null)
                return null;

            Hearthstone.DataModels.RewardTrackDataModel model = track.TrackDataModel;
            return new RewardTrackInfo
            {
                TrackType = typeName,
                RewardTrackId = model.RewardTrackId,
                Level = model.Level,
                Xp = model.Xp,
                XpNeeded = model.XpNeeded,
                XpProgress = model.XpProgress,
                TotalXp = model.TotalXp,
                LevelHardCap = model.LevelHardCap,
                IsMaxLevel = model.Level == model.LevelHardCap && model.Xp == 0,
                UpdatedAt = DateTime.Now
            };
        }

        private static RatingsInfo ReadRatings()
        {
            RatingsInfo info = new RatingsInfo();
            try
            {
                info.BattlegroundsRating = BaconLobbyMgr.Get()?.GetBattlegroundsActiveGameModeRating();
            }
            catch
            {
            }

            try
            {
                MedalInfoTranslator medalInfo = RankMgr.Get()?.GetLocalPlayerMedalInfo();
                if (medalInfo != null)
                {
                    info.ClassicRank = ToRankInfo(medalInfo.GetCurrentMedal(PegasusShared.FormatType.FT_CLASSIC));
                    info.StandardRank = ToRankInfo(medalInfo.GetCurrentMedal(PegasusShared.FormatType.FT_STANDARD));
                    info.WildRank = ToRankInfo(medalInfo.GetCurrentMedal(PegasusShared.FormatType.FT_WILD));
                    info.TwistRank = ToRankInfo(medalInfo.GetCurrentMedal(PegasusShared.FormatType.FT_TWIST));
                }
            }
            catch
            {
            }

            try
            {
                NetCache.NetCacheMercenariesPlayerInfo mercenariesPlayerInfo = NetCache.Get()?.GetNetObject<NetCache.NetCacheMercenariesPlayerInfo>();
                if (mercenariesPlayerInfo != null)
                {
                    info.MercenariesPvpRating = mercenariesPlayerInfo.PvpRating;
                    info.MercenariesPvpSeasonHighestRating = mercenariesPlayerInfo.PvpSeasonHighestRating;
                }
            }
            catch
            {
            }

            return info;
        }

        private static RankInfo ToRankInfo(TranslatedMedalInfo medal)
        {
            if (medal == null)
                return null;

            string rankName = Utils.RankIdxToString(medal.starLevel);
            return new RankInfo
            {
                RankName = rankName,
                StarLevel = medal.starLevel,
                EarnedStars = medal.earnedStars,
                LegendIndex = medal.legendIndex,
                SeasonWins = medal.seasonWins,
                SeasonGames = medal.seasonGames,
                Display = rankName == "传说" ? "传说" + medal.legendIndex : rankName + "-" + medal.earnedStars
            };
        }

        private static int CalculateXpChange(RewardTrackXpChange change)
        {
            if (change.CurrLevel == change.PrevLevel)
                return Math.Max(0, change.CurrXp - change.PrevXp);

            int prevTotal = CalculateTotalXp(change.RewardTrackType, change.PrevLevel, change.PrevXp);
            int currTotal = CalculateTotalXp(change.RewardTrackType, change.CurrLevel, change.CurrXp);
            return Math.Max(0, currTotal - prevTotal);
        }

        private static int CalculateTotalXp(int trackType, int level, int xp)
        {
            try
            {
                Hearthstone.Progression.RewardTrack track = RewardTrackManager.Get()?.GetRewardTrack((Global.RewardTrackType)trackType);
                RewardTrackDbfRecord asset = track?.RewardTrackAsset;
                if (asset == null || asset.Levels == null || level <= 1)
                    return xp;

                int total = xp;
                foreach (RewardTrackLevelDbfRecord record in asset.Levels.OrderBy(x => x.Level))
                {
                    if (record.Level >= level)
                        break;
                    total += Math.Max(0, record.XpNeeded);
                }
                return total;
            }
            catch
            {
                return xp;
            }
        }

        private static string GetTrackName(int trackType)
        {
            return ((Global.RewardTrackType)trackType).ToString();
        }

        private static string GetSourceName(int sourceType)
        {
            return ((RewardSourceType)sourceType).ToString();
        }
    }
}
