using System;
using System.Collections.Generic;

namespace HsMod
{
    public class HsModState
    {
        public DateTime ServerTime { get; set; }
        public long NextSequence { get; set; }
        public RewardTracksInfo RewardTracks { get; set; }
        public RatingsInfo Ratings { get; set; }
        public List<XpChangeInfo> XpChanges { get; set; }

        public HsModState CloneWithoutChanges()
        {
            return new HsModState
            {
                ServerTime = ServerTime,
                NextSequence = NextSequence,
                RewardTracks = RewardTracks,
                Ratings = Ratings,
                XpChanges = new List<XpChangeInfo>()
            };
        }
    }

    public class RewardTracksInfo
    {
        public RewardTrackInfo Global { get; set; }
        public RewardTrackInfo Event { get; set; }
        public RewardTrackInfo Battlegrounds { get; set; }
    }

    public class RewardTrackInfo
    {
        public string TrackType { get; set; }
        public int RewardTrackId { get; set; }
        public int Level { get; set; }
        public int Xp { get; set; }
        public int XpNeeded { get; set; }
        public string XpProgress { get; set; }
        public int TotalXp { get; set; }
        public int LevelHardCap { get; set; }
        public bool IsMaxLevel { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class RatingsInfo
    {
        public int? BattlegroundsRating { get; set; }
        public RankInfo ClassicRank { get; set; }
        public RankInfo StandardRank { get; set; }
        public RankInfo WildRank { get; set; }
        public RankInfo TwistRank { get; set; }
        public int? MercenariesPvpRating { get; set; }
        public int? MercenariesPvpSeasonHighestRating { get; set; }
    }

    public class RankInfo
    {
        public string RankName { get; set; }
        public int StarLevel { get; set; }
        public int EarnedStars { get; set; }
        public int LegendIndex { get; set; }
        public int SeasonWins { get; set; }
        public int SeasonGames { get; set; }
        public string Display { get; set; }
    }

    public class XpChangeInfo
    {
        public long Sequence { get; set; }
        public DateTime Time { get; set; }
        public string TrackType { get; set; }
        public string SourceType { get; set; }
        public int SourceId { get; set; }
        public int Amount { get; set; }
        public int PrevLevel { get; set; }
        public int PrevXp { get; set; }
        public int CurrLevel { get; set; }
        public int CurrXp { get; set; }
    }
}
