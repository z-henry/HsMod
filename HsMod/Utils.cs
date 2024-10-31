using System.Net;
            if (cardSaleResult.Action == Network.CardSaleResult.SaleResult.CARD_WAS_BOUGHT)
            {
                ;
            }
            if (cardSaleResult.Action != Network.CardSaleResult.SaleResult.CARD_WAS_SOLD)
                            //m_mercenaryAcknowledgements.Add(mercenaryAcknowledgeData);
            if (!isAutoRefundCardDisenchantEnable.Value)
            {
            }
                    CraftingManager.Get().TryGetCardSellValue(record.CardId, record.PremiumType, out int value);
                    totalSell += record.OwnedCount * value;
                    CraftingManager.Get().TryGetCardSellValue(record.CardId, TAG_PREMIUM.GOLDEN, out int goldenSellValue);
                    CraftingManager.Get().TryGetCardBuyValue(record.CardId, record.PremiumType, out int buyValue);
                    CraftingManager.Get().TryGetCardSellValue(record.CardId, TAG_PREMIUM.NORMAL, out int normalValue);
                    CraftingManager.Get().TryGetCardSellValue(record.CardId, TAG_PREMIUM.GOLDEN, out int goldenValue);

                    if ((sellValue == buyValue) && (normalBuyValue == normalSellValue) && (goldenBuyValue == goldenSellValue))
                    {
                        SignatureDisenchantCount = numSignatureCopiesInCollection,
                        DiamondDisenchantCount = numSignatureCopiesInCollection
                        totalSell += record.OwnedCount * sellValue;
                    value = -(normalValue * numNormalCopiesInCollection + goldenValue * numGoldenCopiesInCollection);
                    network.CraftingTransaction(m_pendingClientTransaction, value, numNormalCopiesInCollection, numGoldenCopiesInCollection, numSignatureCopiesInCollection, numDiamondCopiesInCollection);
                        MyLogger(LogLevel.Warning, $"尝试分解卡牌：{record.CardId}({record.Name.ToString()})，普通{numNormalCopiesInCollection}，金卡{numGoldenCopiesInCollection}。");
                    }
            webPageBackImg.Value = "";
        }


        //虚假开包数据范围
        public static List<int> GetCardsDbId()
        {
            List<int> cardsDbId = new List<int>();
            foreach (int dbid in GameDbf.GetIndex().GetCollectibleCardDbIds())
            {
                var entitydef = DefLoader.Get().GetEntityDef(dbid, false);
                if (entitydef != null)
                {
                    if (entitydef.GetRarity() != TAG_RARITY.FREE
                        && entitydef.GetRarity() != TAG_RARITY.INVALID)
                    {
                        if (entitydef.GetCardType() != TAG_CARDTYPE.HERO)
                            cardsDbId.Add(dbid);
                        else if (entitydef.GetCost() != 0)    // 忽略英雄皮肤
                            cardsDbId.Add(dbid);
                    }
                }
            }
            return cardsDbId;
        }

        //虚假结果，指定稀有度
        public static int GetRandomCardID(TAG_RARITY rarity)
        {
            int dbid;
            List<int> dbids = GetCardsDbId();
            while (true)
            {
                dbid = dbids[UnityEngine.Random.Range(0, dbids.Count)];
                if (DefLoader.Get().GetEntityDef(dbid, false).GetRarity() == rarity)
                {
                    break;
                }
            }
            return dbid;
        }


        public static NetCache.BoosterCard GenerateRandomACard(bool rarityRandom = false, bool premiumRandom = false, TAG_RARITY rarity = TAG_RARITY.LEGENDARY, TAG_PREMIUM premium = TAG_PREMIUM.GOLDEN)
        {
            if (!rarityRandom) rarity = (TAG_RARITY)fakeRandomRarity.Value;
            if (!premiumRandom) premium = fakeRandomPremium.Value;
            if (fakeBoosterDbId.Value.ToString().Substring(0, 7) == "GOLDEN_")
            {
                premiumRandom = false;
                premium = TAG_PREMIUM.GOLDEN;
            }
            List<int> dbids = GetCardsDbId();
            if (premiumRandom)
            {
                if (!isFakeAtypicalRandomPremium.Value)
                {
                    premium = (TAG_PREMIUM)UnityEngine.Random.Range(0, 2);
                }
                else premium = (TAG_PREMIUM)UnityEngine.Random.Range(0, Enum.GetValues(typeof(TAG_PREMIUM)).Length);
            }
            NetCache.BoosterCard card = new NetCache.BoosterCard();
            card.Def.Name = GameUtils.TranslateDbIdToCardId(rarityRandom ? dbids[UnityEngine.Random.Range(0, dbids.Count)] : GetRandomCardID(rarity));
            card.Def.Premium = premium;
            return card;
        }


        //虚假结果
        public static void GenerateRandomCard(bool rarityRandom = false, bool premiumRandom = false, TAG_RARITY rarity = TAG_RARITY.LEGENDARY, TAG_PREMIUM premium = TAG_PREMIUM.GOLDEN)
        {
            if (!rarityRandom) rarity = (TAG_RARITY)fakeRandomRarity.Value;
            if (!premiumRandom) premium = fakeRandomPremium.Value;
            if (fakeBoosterDbId.Value.ToString().Substring(0, 7) == "GOLDEN_")
            {
                premiumRandom = false;
                premium = TAG_PREMIUM.GOLDEN;
            }
            List<int> dbids = GetCardsDbId();
            for (int i = 1; i <= 5; i++)
            {
                if (premiumRandom)
                {
                    if (!isFakeAtypicalRandomPremium.Value)
                    {
                        premium = (TAG_PREMIUM)UnityEngine.Random.Range(0, 2);
                    }
                    else premium = (TAG_PREMIUM)UnityEngine.Random.Range(0, Enum.GetValues(typeof(TAG_PREMIUM)).Length);
                }
                switch (i)
                {
                    case 1:
                        fakeCardID1.Value = rarityRandom ? dbids[UnityEngine.Random.Range(0, dbids.Count)] : GetRandomCardID(rarity);
                        fakeCardPremium1.Value = premium;
                        break;
                    case 2:
                        fakeCardID2.Value = rarityRandom ? dbids[UnityEngine.Random.Range(0, dbids.Count)] : GetRandomCardID(rarity);
                        fakeCardPremium2.Value = premium;
                        break;
                    case 3:
                        fakeCardID3.Value = rarityRandom ? dbids[UnityEngine.Random.Range(0, dbids.Count)] : GetRandomCardID(rarity);
                        fakeCardPremium3.Value = premium;
                        break;
                    case 4:
                        fakeCardID4.Value = rarityRandom ? dbids[UnityEngine.Random.Range(0, dbids.Count)] : GetRandomCardID(rarity);
                        fakeCardPremium4.Value = premium;
                        break;
                    case 5:
                        fakeCardID5.Value = rarityRandom ? dbids[UnityEngine.Random.Range(0, dbids.Count)] : GetRandomCardID(rarity);
                        fakeCardPremium5.Value = premium;
                        break;
                }
            }
        }

        public static void BuyAdventure(BuyAdventureTemplate adventure)
        {
            if (adventure == BuyAdventureTemplate.DoNothing)
            {
                return;
            }
            if (SceneMgr.Get().GetMode() == SceneMgr.Mode.STARTUP || SceneMgr.Get().GetMode() == SceneMgr.Mode.LOGIN)
            {
                UIStatus.Get().AddInfo("未初始化！");
                return;
            }
            if (SceneMgr.Get().GetMode() == SceneMgr.Mode.GAMEPLAY)
            {
                UIStatus.Get().AddInfo("不能在游戏内购买！");
                return;
            }
            try
            {
                int wingID = -1;
                ProductType productType = ProductType.PRODUCT_TYPE_UNKNOWN;
                switch (adventure)
                {
                    case BuyAdventureTemplate.BuyKara:
                        productType = ProductType.PRODUCT_TYPE_WING;
                        wingID = 16;
                        break;
                    case BuyAdventureTemplate.BuyNAX:
                        productType = ProductType.PRODUCT_TYPE_NAXX;
                        wingID = 1;
                        break;
                    case BuyAdventureTemplate.BuyBRM:
                        productType = ProductType.PRODUCT_TYPE_BRM;
                        wingID = 6;
                        break;
                    case BuyAdventureTemplate.BuyLOE:
                        productType = ProductType.PRODUCT_TYPE_LOE;
                        wingID = 11;
                        break;
                    default:
                        return;
                }

                if (adventure == BuyAdventureTemplate.BuyKara)
                {
                    for (int i = 16; i <= 20; i++)
                    {
                        wingID = i;
                        if (StoreManager.GetStaticProductItemOwnershipStatus(productType, wingID, out string _) != ItemOwnershipStatus.OWNED)
                        {
                            break;
                        }
                    }
                    if (wingID == 20)
                    {
                        wingID = 15;
                    }
                }

                if (StoreManager.GetStaticProductItemOwnershipStatus(productType, wingID, out string failReason) == ItemOwnershipStatus.OWNED)
                {
                    Utils.MyLogger(LogLevel.Warning, $"{adventure}：冒险已拥有！");
                    UIStatus.Get().AddInfo("所选冒险已拥有！");
                }
                else
                {
                    StoreManager.Get().StartAdventureTransaction(productType, wingID, null, null, global::ShopType.ADVENTURE_STORE, 1, false, null, 0);

                    //ProductDataModel productByPmtId = StoreManager.Get().Catalog.GetProductByPmtId(ProductId.CreateFromValidated((long)0));     // 购买经典卡包
                    //PriceDataModel priceDataModel = productByPmtId.Prices.FirstOrDefault((PriceDataModel p) => p.Currency == CurrencyType.GOLD);
                    //Shop.Get().AttemptToPurchaseProduct(productByPmtId, priceDataModel, 1);
                }

            }
            catch (Exception ex)
            {
                Utils.MyLogger(LogLevel.Warning, ex);
            }
        }

        public static List<int> CacheCoin = new List<int>();
        public static List<int> CacheCoinCard = new List<int>();
        public static List<int> CacheGameBoard = new List<int>();
        public static List<int> CacheBgsBoard = new List<int>();
        public static List<int> CacheCardBack = new List<int>();
        public static List<int> CacheBgsFinisher = new List<int>();
        public static Dictionary<int, Assets.CardHero.HeroType> CacheHeroes = new Dictionary<int, Assets.CardHero.HeroType>();
        public static string CacheLastOpponentFullName;
        public static string CacheRawHeroCardId;
        public static Blizzard.GameService.SDK.Client.Integration.BnetAccountId CacheLastOpponentAccountID;
        public static List<MercenarySkin> CacheMercenarySkin = new List<MercenarySkin>();

        public static class CacheInfo
        {

            public static void UpdateCoin()
            {
                CacheCoin.Clear();
                foreach (var record in GameDbf.CosmeticCoin.GetRecords())
                {
                    if (record != null)
                    {
                        CacheCoin.Add(record.CardId);
                    }
                }
            }
            public static void UpdateCoinCard()
            {
                CacheCoinCard.Clear();
                foreach (var record in GameDbf.CosmeticCoin.GetRecords())
                {
                    if (record != null)
                    {
                        CacheCoinCard.Add(record.CardId);
                    }
                }
            }

            public static void UpdateGameBoard()
            {
                CacheGameBoard.Clear();
                foreach (var record in GameDbf.Board.GetRecords())
                {
                    if (record != null)
                    {
                        CacheGameBoard.Add(record.ID);
                    }
                }
            }
            public static void UpdateBgsBoard()
            {
                CacheBgsBoard.Clear();
                foreach (var record in GameDbf.BattlegroundsBoardSkin.GetRecords())
                {
                    if (record != null)
                    {
                        CacheBgsBoard.Add(record.ID);
                    }
                }
            }
            public static void UpdateHeroes()
            {
                CacheHeroes.Clear();
                foreach (var record in GameDbf.CardHero.GetRecords())
                {
                    if (record != null)
                    {
                        CacheHeroes.Add(record.CardId, record.HeroType);
                    }
                }
            }
            public static void UpdateCardBack()
            {
                CacheCardBack.Clear();
                foreach (var record in GameDbf.CardBack.GetRecords())
                {
                    if (record != null)
                    {
                        CacheCardBack.Add(record.ID);
                    }
                }
            }
            public static void UpdateBgsFinisher()
            {
                CacheBgsFinisher.Clear();
                foreach (var record in GameDbf.BattlegroundsFinisher.GetRecords())
                {
                    if (record != null)
                    {
                        CacheBgsFinisher.Add(record.ID);
                    }
                }
            }

            public static void UpdateMercenarySkin()
            {
                CacheMercenarySkin.Clear();
                foreach (var merc in GameDbf.LettuceMercenary.GetRecords())
                {

                    if (merc != null && merc.MercenaryArtVariations.Count > 0)
                    {
                        MercenarySkin mercSkin = new MercenarySkin
                        {
                            Name = merc.MercenaryArtVariations.First().CardRecord.Name.GetString(),
                            Id = new List<int>(),
                            hasDiamond = false
                        };
                        foreach (var art in merc.MercenaryArtVariations.OrderBy(x => x.ID).ToList())
                        {
                            if (art != null)
                            {
                                mercSkin.Id.Add(art.CardId);
                                if (art.DefaultVariation)
                                {
                                    mercSkin.Default = art.CardId;
                                }
                                foreach (var premiums in art.MercenaryArtVariationPremiums)
                                {
                                    if (premiums != null && premiums.Premium == Assets.MercenaryArtVariationPremium.MercenariesPremium.PREMIUM_DIAMOND)
                                    {
                                        mercSkin.hasDiamond = true;
                                        mercSkin.Diamond = art.CardId;
                                    }
                                }
                            }
                        }
                        CacheMercenarySkin.Add(mercSkin);
                    }
                }
            }

        }

        public static void UpdateHeroPowerMapping()
        {
            HeroesPowerMapping.Clear();
            //HeroesPowerMapping.Add("CS2_083b_H1", "CS2_102_H1"); 求解存在问题，如，玛维的技能无法正确列出。
            foreach (var hero in HeroesMapping)
            {
                string rawHeroPower = GameUtils.GetHeroPowerCardIdFromHero(hero.Key);
                string aimHeroPower = GameUtils.GetHeroPowerCardIdFromHero(hero.Value);

                if (rawHeroPower != string.Empty && aimHeroPower != string.Empty && (!HeroesPowerMapping.ContainsKey(rawHeroPower)))
                {
                    HeroesPowerMapping.Add(rawHeroPower, aimHeroPower);
                    //MyLogger(LogLevel.Debug, "HeroPowerMapping: " + rawHeroPower + " -> " + aimHeroPower);
                }
            }
        }


        public static bool IsMercenaryFullyUpgraded(LettuceMercenary merc)
        {
            if (merc == null || !merc.m_owned || !merc.IsMaxLevel())
            {
                return false;
            }
            else
            {
                foreach (var ability in merc.m_abilityList)
                {
                    if (ability.GetNextUpgradeCost() <= 0)
                    {
                        continue;
                    }
                    else return false;
                }
                foreach (var equipment in merc.m_equipmentList)
                {
                    if (equipment.GetNextUpgradeCost() <= 0)
                    {
                        continue;
                    }
                    else return false;
                }
                return true;
            }
        }

        public static long CalcMercenaryCoinNeed(LettuceMercenary merc)
        {
            long coinNeed = 0;
            if (!merc.m_owned)
            {
                coinNeed = merc.GetCraftingCost() - merc.m_currencyAmount;
                return (coinNeed > 0) ? coinNeed : 4096;
            }

            foreach (var ability in merc.m_abilityList)
            {
                if (ability.GetNextUpgradeCost() <= 0)
                {
                    continue;
                }
                int tier = ability.GetNextTier() - 1;
                switch (tier)
                {
                    case 1:
                        coinNeed += 50 + 125 + 150 + 150;
                        break;
                    case 2:
                        coinNeed += 125 + 150 + 150;
                        break;
                    case 3:
                        coinNeed += 150 + 150; ;
                        break;
                    case 4:
                        coinNeed += 150;
                        break;
                    case 5:
                        coinNeed += 0;
                        break;
                }
            }
            foreach (var equipment in merc.m_equipmentList)
            {
                if (equipment.GetNextUpgradeCost() <= 0)
                {
                    continue;
                }
                int tier = equipment.GetNextTier() - 1;
                switch (tier)
                {
                    case 1:
                        coinNeed += 100 + 150 + 175;
                        break;
                    case 2:
                        coinNeed += 150 + 175;
                        break;
                    case 3:
                        coinNeed += 175; ;
                        break;
                    case 4:
                        coinNeed += 0;
                        break;
                }
            }
            coinNeed -= merc.m_currencyAmount;
            return (coinNeed > 0) ? coinNeed : 8192;
        }

        public static class CheckInfo
        {
            public static bool IsCoin()
            {
                if (CacheCoin.Count == 0) CacheInfo.UpdateCoin();
                if (CacheCoin.Contains(skinCoin.Value)) return true;
                else return false;
            }
            public static bool IsCoin(string cardId)
            {
                int dbId = GameUtils.TranslateCardIdToDbId(cardId);
                if (CacheCoinCard.Count == 0) CacheInfo.UpdateCoinCard();
                if (CacheCoinCard.Contains(dbId)) return true;
                else return false;
            }
            public static bool IsBoard()
            {
                if (CacheGameBoard.Count == 0) CacheInfo.UpdateGameBoard();
                if (CacheGameBoard.Contains(skinBoard.Value)) return true;
                else return false;
            }
            public static bool IsBgsBoard()
            {
                if (CacheBgsBoard.Count == 0) CacheInfo.UpdateBgsBoard();
                if (CacheBgsBoard.Contains(skinBgsBoard.Value)) return true;
                else return false;
            }
            public static bool IsHero(int DbID, out Assets.CardHero.HeroType heroType)
            {
                if (CacheHeroes.Count == 0) CacheInfo.UpdateHeroes();
                if (CacheHeroes.ContainsKey(DbID))
                {
                    heroType = CacheHeroes[DbID];
                    return true;
                }
                if (DefLoader.Get()?.GetEntityDef(DbID)?.GetCardType() == TAG_CARDTYPE.HERO)
                {
                    heroType = Assets.CardHero.HeroType.UNKNOWN;
                    return true;
                }
                else
                {
                    heroType = Assets.CardHero.HeroType.UNKNOWN;
                    return false;
                }
            }
            public static bool IsCardBack()
            {
                if (CacheCardBack.Count == 0) CacheInfo.UpdateCardBack();
                if (CacheCardBack.Contains(skinCardBack.Value)) return true;
                else return false;
            }
            public static bool IsBgsFinisher()
            {
                if (CacheBgsFinisher.Count == 0) CacheInfo.UpdateBgsFinisher();
                if (CacheBgsFinisher.Contains(skinBgsFinisher.Value)) return true;
                else return false;
            }

            public static bool IsHero(string cardID, out Assets.CardHero.HeroType heroType)
            {
                if (CacheHeroes.Count == 0) CacheInfo.UpdateHeroes();
                int dbid = GameUtils.TranslateCardIdToDbId(cardID);
                if (CacheHeroes.ContainsKey(dbid))
                {
                    heroType = CacheHeroes[dbid];
                    return true;
                }
                else
                {
                    heroType = Assets.CardHero.HeroType.UNKNOWN;
                    return false;
                }
            }

            public static bool IsMercenarySkin(string cardID, out MercenarySkin skin)
            {
                if (CacheMercenarySkin.Count == 0) CacheInfo.UpdateMercenarySkin();
                int dbid = GameUtils.TranslateCardIdToDbId(cardID);

                foreach (var mercSkin in CacheMercenarySkin)
                {
                    if (mercSkin.Id.Contains(dbid))
                    {
                        skin = mercSkin;
                        return true;
                    }
                }
                skin = new MercenarySkin();
                return false;
            }

        }

        public static void EnableBepInExLogs()
        {
            var coreConfigProp = typeof(BepInEx.Configuration.ConfigFile).GetProperty("CoreConfig", BindingFlags.Static | BindingFlags.NonPublic);
            if (coreConfigProp == null) throw new ArgumentNullException(nameof(coreConfigProp));
            var coreConfig = (BepInEx.Configuration.ConfigFile)coreConfigProp.GetValue(null, null);

            //var bepinMeta = new BepInEx.BepInPlugin("BepInEx", "BepInEx", typeof(BepInEx.Bootstrap.Chainloader).Assembly.GetName().Version.ToString());
            //var bepinexConfig = new BepInEx.Configuration.ConfigFile(Path.Combine(BepInEx.Paths.ConfigPath, "BepInEx.cfg"), true, bepinMeta);
            BepInEx.Configuration.ConfigEntry<bool> configUnityLogListening;
            BepInEx.Configuration.ConfigEntry<bool> configWriteUnityLog;
            BepInEx.Configuration.ConfigEntry<bool> configAppendLog;
            BepInEx.Configuration.ConfigEntry<bool> configEnabled;
            BepInEx.Configuration.ConfigEntry<LogLevel> configLogLevels;
            BepInEx.Configuration.ConfigEntry<LogLevel> configUnityLogLevels;
            if (coreConfig.TryGetEntry("Logging", "UnityLogListening", out configUnityLogListening))
            {
                configUnityLogListening.Value = true;
            }
            else
            {
                MyLogger(LogLevel.Error, Path.Combine(BepInEx.Paths.ConfigPath, "BepInEx.cfg"));
                MyLogger(LogLevel.Error, "Logging.UnityLogListening not found");
            }
            if (coreConfig.TryGetEntry("Logging.Disk", "WriteUnityLog", out configWriteUnityLog))
            {
                configWriteUnityLog.Value = true;
            }
            else
            {
                MyLogger(LogLevel.Error, "Logging.Disk.WriteUnityLog not found");
            }
            if (coreConfig.TryGetEntry("Logging.Disk", "AppendLog", out configAppendLog))
            {
                configAppendLog.Value = false;
            }
            else
            {
                MyLogger(LogLevel.Error, "Logging.Disk.AppendLog not found");
            }
            if (coreConfig.TryGetEntry("Logging.Disk", "Enabled", out configEnabled))
            {
                configEnabled.Value = true;
            }
            else
            {
                MyLogger(LogLevel.Error, "Logging.DiskEnabled  not found");
            }
            if (coreConfig.TryGetEntry("Logging.Disk", "LogLevels", out configLogLevels))
            {
                configLogLevels.Value = LogLevel.All;
            }
            else
            {
                MyLogger(LogLevel.Warning, "Logging.Disk.LogLevels not found");
            }
            if (coreConfig.TryGetEntry("Logging.Unity", "LogLevels", out configUnityLogLevels))
            {
                configUnityLogLevels.Value = LogLevel.All;
            }
            else
            {
                MyLogger(LogLevel.Warning, "Logging.Unity.LogLevels not found");
            }
        }

        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir))
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d);
                    else
                        DeleteFolder(d);
                }
                Directory.Delete(dir, true);
            }
        }

        public static class LeakInfo
        {
            public static void Mercenaries(string savePath = @"BepInEx/HsMercenaries.log")
            {
                //List<LettuceTeam> teams = CollectionManager.Get().GetTeams();
                //System.IO.File.WriteAllText(savePath, DateTime.Now.ToLocalTime().ToString() + "\t获取到您的队伍如下：\n");
                //foreach (LettuceTeam team in teams)
                //{
                //    System.IO.File.AppendAllText(savePath, team.Name + "\n");
                //}
                System.IO.File.WriteAllText(savePath, DateTime.Now.ToLocalTime().ToString() + "\t获取到关卡信息如下：\n");
                System.IO.File.AppendAllText(savePath, "[ID]\t[Heroic?]\t[Bounty]\t[BossName]\n");
                foreach (var record in GameDbf.LettuceBounty.GetRecords())     // 生成关卡名称
                {
                    string saveString;
                    if (record != null)
                    {
                        saveString = record.ID.ToString() + (record.Heroic ? " Heroic " : " ") + record.BountySetRecord.Name.GetString() + " " + GameDbf.Card.GetRecord(record.FinalBossCardId).Name.GetString();
                        System.IO.File.AppendAllText(savePath, saveString + "\n");
                    }
                }
            }
            public static void MyCards(string savePath = @"BepInEx/HsRefundCards.log")
            {
                System.IO.File.AppendAllText(savePath, DateTime.Now.ToLocalTime().ToString() + "\t获取到全额分解卡牌情况如下：\n");
                System.IO.File.AppendAllText(savePath, "[Name]\t[PremiumType]\t[Rarity]\t[CardId]\t[CardDbId]\t[OwnedCount]\n");
                //Filter<CollectibleCard> filter3 = new Filter<CollectibleCard>((CollectibleCard card) => card.IsRefundable);
                foreach (var record in CollectionManager.Get().GetOwnedCards())
                {
                    string saveString;
                    if (record != null && record.IsCraftable && record.IsRefundable && (record.OwnedCount > 0))
                    {
                        // GameUtils.TranslateDbIdToCardId(record.CardId, false);
                        saveString = $"{record.Name}\t{record.PremiumType}\t{record.Rarity}\t{record.CardId}\t{record.CardDbId}\t{record.OwnedCount}\t";
                        System.IO.File.AppendAllText(savePath, saveString + "\n");
                    }
                }
            }
            public static void Skins(string savePath = "BepInEx/HsSkins.log")
            {
                System.IO.File.WriteAllText(savePath, DateTime.Now.ToLocalTime().ToString() + "\t获取到硬币皮肤如下：\n");
                System.IO.File.AppendAllText(savePath, "[CARD_ID]\t[Name]\n");
                foreach (var record in GameDbf.CosmeticCoin.GetRecords())
                {
                    string saveString;
                    if (record != null)
                    {
                        saveString = $"{record.CardId}\t{record.Name.GetString()}";
                        System.IO.File.AppendAllText(savePath, saveString + "\n");
                    }
                }
                System.IO.File.AppendAllText(savePath, DateTime.Now.ToLocalTime().ToString() + "\t获取到卡背信息如下：\n");
                System.IO.File.AppendAllText(savePath, "[ID]\t[Name]\n");
                foreach (var record in GameDbf.CardBack.GetRecords())
                {
                    string saveString;
                    if (record != null)
                    {
                        // GameUtils.TranslateDbIdToCardId(record.CardId, false);
                        saveString = $"{record.ID}\t{record.Name.GetString()}";
                        System.IO.File.AppendAllText(savePath, saveString + "\n");
                    }
                }

                System.IO.File.AppendAllText(savePath, DateTime.Now.ToLocalTime().ToString() + "\t获取到游戏面板信息如下：\n");
                System.IO.File.AppendAllText(savePath, "[ID]\t[NOTE_DESC]\n");
                foreach (var record in GameDbf.Board.GetRecords())
                {
                    string saveString;
                    if (record != null)
                    {
                        // GameUtils.TranslateDbIdToCardId(record.CardId, false);
                        saveString = $"{record.ID}\t{record.GetVar("NOTE_DESC")}";
                        System.IO.File.AppendAllText(savePath, saveString + "\n");
                    }
                }

                System.IO.File.AppendAllText(savePath, DateTime.Now.ToLocalTime().ToString() + "\t获取到酒馆战斗面板如下：\n");
                System.IO.File.AppendAllText(savePath, "[ID]\t[CollectionShortName]\t[CollectionName]\n");
                foreach (var record in GameDbf.BattlegroundsBoardSkin.GetRecords())
                {
                    string saveString;
                    if (record != null)
                    {
                        // GameUtils.TranslateDbIdToCardId(record.CardId, false);
                        saveString = $"{record.ID}\t{record.CollectionShortName.GetString()}\t{record.CollectionName.GetString()}";
                        System.IO.File.AppendAllText(savePath, saveString + "\n");
                    }
                }

                System.IO.File.AppendAllText(savePath, DateTime.Now.ToLocalTime().ToString() + "\t获取到酒馆终结特效如下：\n");
                System.IO.File.AppendAllText(savePath, "[ID]\t[CollectionShortName]\t[CollectionName]\n");
                foreach (var record in GameDbf.BattlegroundsFinisher.GetRecords())
                {
                    string saveString;
                    if (record != null)
                    {
                        // GameUtils.TranslateDbIdToCardId(record.CardId, false);
                        saveString = $"{record.ID}\t{record.CollectionShortName.GetString()}\t{record.CollectionName.GetString()}";
                        System.IO.File.AppendAllText(savePath, saveString + "\n");
                    }
                }

                System.IO.File.AppendAllText(savePath, DateTime.Now.ToLocalTime().ToString() + "\t获取到英雄皮肤（包括酒馆）如下：\n");
                System.IO.File.AppendAllText(savePath, "[CARD_ID]\t[Name]\t[HeroType]\n");
                foreach (var record in GameDbf.CardHero.GetRecords())
                {
                    string saveString;
                    if (record != null)
                    {
                        string name = GameDbf.Card.GetRecord(record.CardId).Name.GetString();
                        // GameUtils.TranslateDbIdToCardId(record.CardId, false);
                        saveString = $"{record.CardId}\t{name}\t{record.HeroType}\t";
                        System.IO.File.AppendAllText(savePath, saveString + "\n");
                    }
                }
            }
        }
    }
}
