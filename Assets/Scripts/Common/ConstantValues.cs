using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class ConstantValues {
    public static readonly string GOOLGE_PLAY_STORE_LINK = "https://play.google.com/store/apps/details?id=com.devpixel.necromancerdefense";
    public const int REVIVAL_DIA_AMOUNT = 300;
    public static readonly Color activateColor = new Color(1, 1, 1);
    public static readonly Color inActivateColor = new Color(0f, 0f, 0f);
    public static readonly int TARGET_FRAME = 60; // 1회
    public static readonly int ONE_BILLION = 1000000000; // 1시간
    public static readonly int MAX_ACHIEVEMENT_MONSTER_COUNT = 800000000; // 1시간
    
    #region LogMessage
    public static readonly string NOT_ENOUGH_SP_COUNT = "스킬 포인트가 부족합니다.";
    

    #endregion

    public const string SCENE_TAG = "GameScene";
    public const int MAX_ARCHER_COUNT_LEVEL = 15;
    #region TAG
    public const string MOB_MELEE_ATTACK_TAG = "MobMeleeAttack";
    public const string SKELETON_WARRIOR_NAME = "SkeletonWarrior_01_Basis";
    public const string MOB_TAG = "Mob";
    public const string BOMB_GOBLIN_TAG = "Bomb_Goblin";
    public const string SUMMON_TAG = "Summon";
    public const string ALLIES_TAG = "Allies";
    public const string SUMMON_MELEE_TAG = "SummonMelee";
    public const string CATAPULT_TAG = "CatapultMob";
    public const string MOB_CHEST_TAG = "Mob_Chest";
    public const string NECROMANCER_TAG = "Necromancer";
    public const string BOSS_TAG = "BossMob";
    public const string DUNGEON_BOSS_TAG = "DungeonBossMob";
    public const string ARCHER_TAG = "Archer";
    public const string HERO_TAG = "Hero";
    public const string WALL_TAG = "Wall";
    public const string GOLD_GOBLINE = "GoldGoblin";
    public const string RAID_NAME = "RaidDragon";
    public const string RAID_NAME_Clone = "RaidDragon(Clone)";
    public const string AWAKENING_BOSS_NAME = "AwakeningBossPrefab(Clone)";
    public const string CHEST_NORMAL_CLONE = "chest_normal(Clone)";
    public const string CHEST_UNIQUE_CLONE = "chest_unique(Clone)";
    #endregion

    #region IAP_PRODUCT_ID
    public const string DIA_PRODUCT_01 = "com.necrodefense.product_dia_01";
    public const string DIA_PRODUCT_02 = "com.necrodefense.product_dia_02";
    public const string DIA_PRODUCT_03 = "com.necrodefense.product_dia_03";
    public const string DIA_PRODUCT_04 = "com.necrodefense.product_dia_04";
    public const string DIA_PRODUCT_05 = "com.necrodefense.product_dia_05";
    public const string DIA_PRODUCT_06 = "com.necrodefense.product_dia_06";
    public const string PASS_ATTENDANCE = "com.necrodefense.pass_attendance"; // 출석 패스
    public const string PASS_STAGE_01 = "com.necrodefense.pass_stage_01"; // 스테이지 패스 01
    public const string PASS_STAGE_02 = "com.necrodefense.pass_stage_02"; // 스테이지 패스 02
    public const string PASS_STAGE_03 = "com.necrodefense.pass_stage_03"; // 스테이지 패스 03
    public const string PASS_STAGE_04 = "com.necrodefense.pass_stage_04"; // 스테이지 패스 04
    public const string PASS_STAGE_05 = "com.necrodefense.pass_stage_05"; // 스테이지 패스 05
    public const string ADS_REMOVE_PACKAGE = "com.necrodefense.ads_remove_package"; // 광고 제거 패키지 >> 우편함으로 다이아
    public const string FAST_GROWTH_PACKAGE = "com.necrodefense.package_fast_growth"; // 급속 성장 패키지 >> 우편함
    public const string DAILY_DUNGEON_TICKET_PACKAGE = "com.necrodefense.package_daily_dungeon_ticket"; // 던전 입장권 패키지 (데일리) >> 우편함
    public const string DAILY_GROWTH_PACKAGE = "com.necrodefense.package_daily_growth_package"; // 일일 성장 지원 패키지 >> 우편함
    public const string OPEN_CONGRA_PACKAGE = "com.necrodefense.package_open"; // 오픈 기념 패키지 >> 우편함
    public const string ADVANCED_GROWTH_PACKAGE = "com.necrodefense.advanced_growth_package"; // 급속 성장 패키지 >> 우편함
    public const string SKILL_ENCHANT_STONE_PACKAGE = "com.necrodefense.skill_enchant_package"; // 던전 입장권 패키지 (데일리) >> 우편함
    public const string GOLD_PACKAGE = "com.necrodefense.gold_package"; // 던전 입장권 패키지 (데일리) >> 우편함
    public const string BLOOD_OF_ALTAR_PACKAGE = "com.necrodefense.blood_of_altar_package"; // [1.1.3] 피의 제단
    public const string CHRISTMAS_PACKAGE = "com.necrodefense.christmas_package"; // [1.1.5] 크리스마스 특별 패키지
    public const string PACKAGE_DIA = "com.necrodefense.package_dia";
    public const string PACKAGE_SPECIAL = "com.necrodefense.package_special";
    public const string DAILY_SPECIAL_PACKAGE = "com.necrodefense.daily_special_package";
    public const string RAID_PACKAGE = "com.necrodefense.raid_package";
    public const string MONTHLY_CORPS_PACKAGE = "com.necrodefense.monthly_corps_package";
    public const string MONTHLY_RAID_PACKAGE = "com.necrodefense.monthly_raid_package";
    public const string MONTHLY_GROWTH_PACKAGE = "com.necrodefense.monthly_growth_package";
    public const string COSTUME_01 = "com.necrodefense.costume_01"; // 어둠의 수호자 코스튬 (costume_01) >> 즉시 적용
    public const string COSTUME_02 = "com.necrodefense.costume_02"; // 영혼 강탈자 코스튬 (costume_02) >> 즉시 적용
    public const string COSTUME_03 = "com.necrodefense.costume_03"; // 피의 군주 코스튬 (costume_03) >> 즉시 적용
    public const string COSTUME_04 = "com.necrodefense.costume_04"; // 피의 군주 코스튬 (costume_03) >> 즉시 적용

    #endregion

    #region WARNING_MESSAGE
    public static readonly string CONTENT_ENTRANCE_WARNING = "던전 입장 시 현재 진행되고 있는 WAVE가 종료됩니다. 정말로 진행하시겠습니까?";
    public static readonly string CONTENT_EXIT_WARNING = "정말로 포기하시겠습니까?";
    
    #endregion
}
