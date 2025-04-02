using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
public static class Actions 
{
    public static UnityAction UserSaveRequestStart;
    public static UnityAction IsTodaysFirstUser;
    public static UnityAction OnChangedUnitDie;
    public static UnityAction<float> OnHPChange;
    public static UnityAction OnChangedPowerLevel;
    public static UnityAction OnGoldChanged;
    public static UnityAction OnWaveDoneGetDia;
    public static UnityAction OnBlackStoneChanged;
    public static UnityAction OnDiaChanged;
    public static UnityAction OnDisplayingUIDataChanged;
    public static UnityAction<bool> LoadAllDataDone;
    public static UnityAction NotificationChanged;
    public static UnityAction UserPassChanged;
    public static UnityAction UserNotificationChanged;
    public static UnityAction BloodOfAltarChanged;
    public static UnityAction UserPurchasedProductsChanged;
    public static UnityAction<string> OnChangedUserPurchased;
    public static UnityAction OnChangedWaveRanking; 
    public static UnityAction OnChangedRaidRanking;
    public static UnityAction OnChangedAchievement;
    public static UnityAction OnAltarStatChanged;
    public static UnityAction WavesStageChanged;
    public static UnityAction CurrentServerTimeChanged;
    public static UnityAction<bool> IsGameSceneActivated;
    public static UnityAction<bool> IsSuccessGetToken;
    public static UnityAction<int> OnChangedDungeonStart;
    public static UnityAction<int> OnChangedSkillDeck;
    public static UnityAction<int> OnChangedExchangeDiaProduct;
    public static UnityAction<int> OnChangedMileageProduct;
}
