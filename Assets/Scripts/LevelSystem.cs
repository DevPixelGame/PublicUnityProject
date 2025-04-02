using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;
using TMPro;
public class LevelSystem : Singleton<LevelSystem>
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;
    private int level;
    private BigInteger experience;
    private BigInteger experienceToNextLevel;

    [SerializeField]
    private GameObject LevelUpFX_OverUI;
    
    [SerializeField]
    private TextMeshProUGUI percentageText;
    public LevelSystem()
    {
        level = 0;
        experience = 0;
        experienceToNextLevel = 300;
    }

    private void Start()
    {
        level = DataManager.Instance.User.level;
        float percentage = GetPercentage(DataManager.Instance.InGameExp, CalClass.CalcMaxExp(DataManager.Instance.User.level));
        percentageText.text = (percentage * 100f).ToString("N1");
    }

    private BigInteger CalcPercentageOfBigInt(BigInteger origin, float addAmount)
    {
        // 전체값 X 퍼센트 ÷ 100
        // BigInt 계산시에 double로 한번 casting 한다.
        double castedDamage = (double)origin;
        double result = (castedDamage * addAmount) * 0.01f;
        // And since you want a BigInteger at the end
        BigInteger bigIntResult = new BigInteger(result);
					
        return bigIntResult;
    }
    
    // 오프라인 보상 또한 여기로 들어온다
    public void AddExperience(BigInteger amount, bool isByMob = false, bool isFromDungeon = false)
    {
        amount = CalClass.CalIncreasedExpPercentage(amount, StatManager.Instance.StatPoint_ExpBonus, isByMob); // 성장 포인트 적용
        if (!isFromDungeon)
        {
            if (WaveManager.Instance.isOfflineRewardsRecordStart && isByMob) // 처치한 몹에 관하여만
            {
                if (DataManager.Instance.IsEventOn_02)
                {
                    DataManager.Instance.OfflineRewardsDB.getExp += CalcPercentageOfBigInt(amount, 50f);
                }
                else
                {
                    DataManager.Instance.OfflineRewardsDB.getExp += amount;
                }
            }
        }
        
        DataManager.Instance.InGameExp += amount;

        float percentage = GetPercentage(DataManager.Instance.InGameExp, CalClass.CalcMaxExp(DataManager.Instance.User.level));
        percentageText.text = (percentage * 100f).ToString("N1");

        if (DataManager.Instance.InGameExp >= CalClass.CalcMaxExp(DataManager.Instance.User.level)) // Level Up
        {
            DataManager.Instance.InGameExp = 0;
            level++;
            percentageText.text = "100";
            AudioManager.Instance.PlayUISFX("LevelUp");
            LevelUpFX_OverUI.SetActive(true);

            if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
        }

        if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
    }

    public void SetOfflineRewards(BigInteger receivedExp)
    {
        
        int tempLv = DataManager.Instance.User.level;
        
        int levelUpCount = 0;
        int nokoruValue = 0;
        // BigInteger tempEx = DataManager.Instance.InGameExp + receivedExp;
        BigInteger currentExp = 0;
        while ((receivedExp + DataManager.Instance.InGameExp) > CalClass.CalcMaxExp(tempLv))
        {
            receivedExp -= (CalClass.CalcMaxExp(tempLv) - DataManager.Instance.InGameExp);
            DataManager.Instance.InGameExp = 0;
        
            levelUpCount++;
            tempLv++;
        }
        
        if (levelUpCount > 0)
        {
            DataManager.Instance.User.level += levelUpCount;
            AchievementManager.Instance.AddMainAchievementCount(0, levelUpCount); // 유저레벨 메인 업적
            DataManager.Instance.InGameUserLevel = DataManager.Instance.User.level;
            DataManager.Instance.User.statPoint += levelUpCount;
            DataManager.Instance.InGameSP_Count += levelUpCount;
        }
        
        if (receivedExp > 0)
        {
            AddExperience(receivedExp);
        }
    }
    
        private float GetPercentage(BigInteger divide, BigInteger max) {
            float result = 0f;
            if (divide > 0)
            {
                divide *= 1000;
                divide /= max;
    
                result = (float)divide;
                result /= 1000f;
            }
            else
            {
                result = 0;
            }
    
            return result;
        }
        

    public int GetLevelNumber()
    {
        return level;
    }

    // public float GetExperienceNormalized()
    // {
    //     
    //     float result = 0f;
    //
    //     if (DataManager.Instance.InGameExp > 0)
    //     {
    //         health *= 1000;
    //         health /= maxHealth;
    //
    //         result = (float)health;
    //         result /= 1000f;
    //     }
    //     else
    //     {
    //         result = 0;
    //     }
    //     
    //     float result = 0f;
    //     BigInteger exp = (BigInteger)DataManager.Instance.InGameExp * 1000;
    //     exp /= CalClass.CalcMaxExp(DataManager.Instance.User.level);
    //     result = (float)exp;
    //     
    //     result /= 1000f;
    //     // return (float)DataManager.Instance.InGameExp / (float)StatManager.Instance.CalcNextEXP(DataManager.Instance.User.level);
    //     // return BigInteger.Divide(DataManager.Instance.InGameExp, CalClass.CalcMaxExp(DataManager.Instance.User.level);
    //     
    //     return result;
    // }
}
