using System;
using System.Numerics;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Numerics;
using CodeStage.AntiCheat.ObscuredTypes;
using ContentModel;
using DevionGames.UIWidgets;
using PathologicalGames;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;

/// <summary>
/// This target can receive damage.
/// </summary>
public class DamageTaker : MonoBehaviour
{
    [SerializeField]
    private Transform damagePopUpPoint;
    
    private SpriteRenderer spriteRenderer;
    private Unit unit;
	private bool coroutineInProgress;
    void Awake()
    {
        // _audioManager = AudioManager.Instance;
        // aiBehavior = GetComponent<AiBehavior>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (unit == null) unit = GetComponent<Unit>();
        // Debug.Assert(sprite && healthBar, "Wrong initial parameters");
    }

    WaitForSeconds colorChangeWait = new WaitForSeconds(0.09f);
    private IEnumerator FlashRed() {
        unit.isDamageOn = true;
        spriteRenderer.color = Color.red;
        
        yield return colorChangeWait;
    
        unit.isDamageOn = false;
        spriteRenderer.color = Color.white;
    }

    private UnityEngine.Vector3 _dmgPopupOffset = new UnityEngine.Vector3(0.3f, 0f, 0f);
    public void TakeDamage(BigInteger damage)
    {
        if (unit != null && unit.health > 0)
        {
            unit.health -= damage;
            if (unit.health <= 0) {
                unit.isDead = true;
                unit.bossHealthBar.SetHealth(unit.health);
                unit.health = unit.health;
                return;
            }

            if (unit.bossHealthBar != null)
            {
                unit.bossHealthBar.SetHealth(unit.health);
                unit.health = unit.health;   
            }
        }

    }

    private Coroutine dieCoroutine;
    public void Die()
    {
        if (gameObject.activeSelf)
        {
            DieCoroutine();
        }
    }

    // 즉사스킬
    public void DieMobByDeathSkill()
    {
        unit.health = 0;
        unit.isDead = true;

        Die();
    }

    public void DieBoss()
    {
        // EventManager.TriggerEvent("UnitKilled", gameObject, null);
        if (gameObject.activeSelf) StartCoroutine(DieBossCoroutine());
    }
    
    public void DieAllies(Unit unit) {
        if (unit != null && gameObject.activeSelf) AlliesDieCoroutine(unit);
    }

    public void DieHeros(Unit unit) {
        StartCoroutine(HerosDieCoroutine(unit));
    }

    private void AlliesDieCoroutine(Unit unit)
	{
        aiBehavior.navAgent.move = false;
        aiBehavior.target = null;
		// GetComponent<EffectControl>().enabled = false;

		// aiBehavior.enabled = false;
		// aiBehavior.navAgent.enabled = false;

		// Animator anim = GetComponent<Animator>();

        // TODO: 삭제 (object pool로 변경 고려)
        if (unit.healthBar != null) UI_AlliesHealthBarPool.ReturnObject(unit.healthBar.gameObject);
        // UI_AlliesSummonTimePool.ReturnObject(unit.summonTimeBar.gameObject);

        unit.healthBar = null;
        // unit.summonTimeBar = null;

        Transform myInstance = PoolManager.Pools["InGameParticle_Root"].Spawn(PoolManager.Pools["InGameParticle_Root"].prefabs["SummonDieFX"]);
        if (myInstance != null)
        {
            myInstance.SetPositionAndRotation(transform.position, Quaternion.identity);
        }

        
		// If unit has animator
		// if (anim != null && anim.runtimeAnimatorController != null)
		// {
  //           // 임시
  //           // Search for clip
  //           foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
  //           {
  //              if (clip.name == "Death")
  //              {
  //                  // Play animation
  //                  anim.SetTrigger("death");
  //                  yield return new WaitForSeconds(clip.length - 0.05f);
  //                  break;
  //              }
  //           }
  //       }

        UnitObjectPool.ReturnAllies(unit);
        // StopAllCoroutines();
	}


    private IEnumerator HerosDieCoroutine(Unit unit)
	{
        if (aiBehavior.target != null) {
            aiBehavior.target = null;
        }

		// GetComponent<EffectControl>().enabled = false;
		// aiBehavior.enabled = false;
		// aiBehavior.navAgent.enabled = false;

		Animator anim = GetComponent<Animator>();

        // TODO: 삭제 (object pool로 변경 고려)
        // UI_AlliesHealthBarPool.ReturnObject(unit.healthBar.gameObject);
        // UI_AlliesSummonTimePool.ReturnObject(unit.summonTimeBar.gameObject);

		// If unit has animator
		if (anim != null && anim.runtimeAnimatorController != null)
		{
            // 임시
            // Search for clip
            foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
            {
               if (clip.name == "Death")
               {
                   // Play animation
                   anim.SetTrigger("death");
                   yield return new WaitForSeconds(clip.length - 0.05f);
                   break;
               }
            }
        }

        // UnitObjectPool.ReturnAllies(unit);
        // StopAllCoroutines();
	}

    private IEnumerator ChestDieCoroutine()
    {
        Animator anim = GetComponent<Animator>();

        // TODO: 삭제 (object pool로 변경 고려)
        // UI_AlliesHealthBarPool.ReturnObject(unit.healthBar.gameObject);
        // UI_AlliesSummonTimePool.ReturnObject(unit.summonTimeBar.gameObject);

        // If unit has animator
        if (anim != null && anim.runtimeAnimatorController != null)
        {
            // 임시
            // Search for clip
            foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "Open")
                {
                    // Play animation
                    anim.SetTrigger("open");
                    break;
                }
            }
        }
        
        yield return null;
    }
    
    
    // 몹이 죽을 떄 호출.
    private void DieCoroutine()
	{

        this.unit.health = 0;
        this.unit.isDead = true;
        
		// GetComponent<EffectControl>().enabled = false;
		// aiBehavior.enabled = false;
		aiBehavior.navAgent.enabled = false;

        Unit unit = gameObject.GetComponent<Unit>(); // get my unit info

        if (GameManager.Instance._GameState.Equals(GameState.PROCESSING_WAVE))
        {
            if (unit.exp > 0) {
                // 웨이브에 따른 골드 보상 계산
                BigInteger goldAmount = ItemDropManager.Instance.CalcDropGold(DataManager.Instance.User.currentWave);
                if (DataManager.Instance.IsEventOn) // ****** 골드 이벤트 적용
                {
                    goldAmount *= 2;
                }
                
                if (BuffManager.Instance.isGrowthPackagePurchased || DataManager.Instance.UserPurchasedProduct.package_fast_growth > 0)
                {
                    goldAmount *= 2;
                }

                float totalGoldBuff = StatManager.Instance.StatPoint_GoldBonus + BuffManager.Instance.WAVE_MAP_GOLD_BUFF;
                if (BuffManager.Instance.HERO_03_BUFF_GOLD_IS_ON)
                {
                    totalGoldBuff += BuffManager.Instance.HERO_03_BUFF_GOLD_VALUE;
                }
                
                BigInteger finalGold = CalClass.CalIncreasedPercentDamage(goldAmount, totalGoldBuff);
                DataManager.Instance.AddInGameGold(finalGold, true); // Event
                
                // ****************** 경험치
                if (DataManager.Instance.IsEventOn_02)
                {
                    unit.exp *= 2;
                }

                if (BuffManager.Instance.WAVE_MAP_EXP_BUFF > 0)
                {
                    unit.exp = CalClass.CalIncreasedPercentDamage(unit.exp, BuffManager.Instance.WAVE_MAP_EXP_BUFF);
                }
                
                if ((BuffManager.Instance.isGrowthPackagePurchased || DataManager.Instance.UserPurchasedProduct.package_fast_growth > 0))
                {
                    LevelSystem.Instance.AddExperience(unit.exp * 2, true); // Event
                }
                else
                {
                    LevelSystem.Instance.AddExperience(unit.exp, true); // Event
                }
                
                
                
                /*
                 * ************************** [1.1.3] 뼈 드랍
                 */
                float boneDrop = Random.Range(0, 100f);
                if (boneDrop < BuffManager.Instance.WAVE_MAP_BONE_DROP) // 스킬석 드랍 확률 20%
                {
                    Transform boneInstance = PoolManager.Pools["InGameParticle_Root"].Spawn(PoolManager.Pools["InGameParticle_Root"].prefabs["BoneDropAnim"]);
                    if (boneInstance != null)
                    {
                        boneInstance.SetPositionAndRotation(transform.position, Quaternion.identity);
                    }
                    
                    Transform textInstance = PoolManager.Pools["DamageText_Root"].Spawn(PoolManager.Pools["DamageText_Root"].prefabs["BoneDropObject"]);
                    if (textInstance != null) {
                        textInstance.SetPositionAndRotation(unit.transform.position, Quaternion.identity);

                        BoneDropPopUpAnim damagePopUp = textInstance.GetComponent<BoneDropPopUpAnim>();

                        damagePopUp.StartTween();
                    }

                    DataManager.Instance.AddInGameBone(1);
                }

                // if (DataManager.Instance.IsChristmasEventOn) // 크리스마스 이벤트가 진행중이면
                // {
                //     // float dropProb = Random.Range(0, 100f);
                //     // if (dropProb < BuffManager.Instance.WAVE_MAP_EVENT_GOODS_DROP) // 스킬석 드랍 확률 20%
                //     // {
                //     //     Transform textInstance = PoolManager.Pools["DamageText_Root"].Spawn(PoolManager.Pools["DamageText_Root"].prefabs["CookieDropObject"]);
                //     //     if (textInstance != null) {
                //     //         textInstance.SetPositionAndRotation(unit.transform.position, Quaternion.identity);
                //     //
                //     //         BoneDropPopUpAnim damagePopUp = textInstance.GetComponent<BoneDropPopUpAnim>();
                //     //
                //     //         damagePopUp.StartTween();
                //     //     }
                //     //
                //     //     DataManager.Instance.AddInGameEventGoods(1);
                //     // }
                // }
                

                /*
                * ********************** 스킬 강화석 드랍
                */
                if (BuffManager.Instance.WAVE_MAP_SKILL_ENCHANT_DROP >= 100f)
                {
                    int amount = ItemDropManager.Instance.CalcDropSkillEnchantStone(DataManager.Instance.CurrentWaveIndex);

                    float skillEnchantIncreasePercentage = 0f;
                    if (StatManager.Instance.AltarStat_09_SkillEnchantStoneIncrease > 0)
                    {
                        skillEnchantIncreasePercentage += StatManager.Instance.AltarStat_09_SkillEnchantStoneIncrease;
                    }
                    
                    if (DataManager.Instance.User.usingNecroCostume == 1) // 어둠의 수호자 착용시
                    {
                        skillEnchantIncreasePercentage += BuffManager.COSTUME_01_SKILL_ENCHANT_INCREASE;
                        amount = CalClass.CalcIncreasedValue(amount, skillEnchantIncreasePercentage);
                    } else if (DataManager.Instance.User.usingNecroCostume == 3)
                    {
                        skillEnchantIncreasePercentage += BuffManager.COSTUME_03_SKILL_ENCHANT_INCREASE;
                        amount = CalClass.CalcIncreasedValue(amount, skillEnchantIncreasePercentage);
                    }

                    // ItemDropManager.Instance.DropWeaponEnchantStone(amount);
                    DataManager.Instance.AddInGameSkillEnchant(amount, true);
                }
                else
                {
                    float prob = Random.Range(0, 100.0f);
                    if (prob < BuffManager.Instance.WAVE_MAP_SKILL_ENCHANT_DROP) // 스킬석 드랍 확률 20%
                    {
                        // 웨이브 별 스킬 강화석 드랍량
                        int amount = ItemDropManager.Instance.CalcDropSkillEnchantStone(DataManager.Instance.CurrentWaveIndex);

                        float skillEnchantIncreasePercentage = 0f;
                        if (StatManager.Instance.AltarStat_09_SkillEnchantStoneIncrease > 0)
                        {
                            skillEnchantIncreasePercentage += StatManager.Instance.AltarStat_09_SkillEnchantStoneIncrease;
                        }
                    
                        if (DataManager.Instance.User.usingNecroCostume == 1) // 어둠의 수호자 착용시
                        {
                            skillEnchantIncreasePercentage += BuffManager.COSTUME_01_SKILL_ENCHANT_INCREASE;
                            amount = CalClass.CalcIncreasedValue(amount, skillEnchantIncreasePercentage);
                        } else if (DataManager.Instance.User.usingNecroCostume == 3)
                        {
                            skillEnchantIncreasePercentage += BuffManager.COSTUME_03_SKILL_ENCHANT_INCREASE;
                            amount = CalClass.CalcIncreasedValue(amount, skillEnchantIncreasePercentage);
                        }
                        else
                        {
                            if (skillEnchantIncreasePercentage > 0)
                            {
                                amount = CalClass.CalcIncreasedValue(amount, skillEnchantIncreasePercentage);
                            }
                        }

                        // ItemDropManager.Instance.DropWeaponEnchantStone(amount);
                        DataManager.Instance.AddInGameSkillEnchant(amount, true);
                    }
                }
            }   
        }
        else if (GameManager.Instance._GameState == GameState.PROCESSING_CONTENTS_GOLD) // 골드던전 보상
        {
            if (BuffManager.Instance.isGrowthPackagePurchased || DataManager.Instance.UserPurchasedProduct.package_fast_growth > 0)
            {
                BigInteger newGold = (BigInteger)unit.gold * 2;
                ContentsManager.Instance.ResultValue += newGold;
            }
            else
            {
                ContentsManager.Instance.ResultValue += unit.gold;
            }
        } else if (GameManager.Instance._GameState == GameState.PROCESSING_CONTENTS_BOSS)
        {
            // DataManager.Instance._ExpDungeonResult += unit.exp;
        } else if (GameManager.Instance._GameState.Equals(GameState.PROCESSING_CONTENTS_COW))
        {
            if ((BuffManager.Instance.isGrowthPackagePurchased || DataManager.Instance.UserPurchasedProduct.package_fast_growth > 0))
            {
                // ContentsManager.Instance.ResultValue += (unit.exp * 2);
                LevelSystem.Instance.AddExperience(unit.exp * 2, true, true); // Event
            }
            else
            {
                // ContentsManager.Instance.ResultValue += unit.exp;
                LevelSystem.Instance.AddExperience(unit.exp, true, true); // Event
            }
        }


        if (unit._uIFXBar != null)
        {
            UI_StatusFXPoisonPool.ReturnObject(unit._uIFXBar.gameObject);
        }
        
        // TODO: 삭제 (object pool로 변경 고려)
        if (unit.healthBar != null) UI_MobHealthBarPool.ReturnObject(unit.healthBar.gameObject);
        unit.healthBar = null;

        EffectManager.Instance.SpawnMobDieEffect(transform);


        
        if (unit.buffImage != null)
        {
            if (PoolManager.Pools["UI_StatusPool"].IsSpawned(unit.buffImage))
            {
                PoolManager.Pools["UI_StatusPool"].Despawn(unit.buffImage);
            }
        }

        aiBehavior.target = null;

        AchievementManager.Instance.AddDailAchievementCount(0, 1);
        // AchievementManager.Instance.AddMainAchievementCount(1, 1); // 몬스터 처치량 메인업적

        UnitObjectPool.ReturnMobs(unit);

        
        
        // StopAllCoroutines();
        
        // yield return null;
	}

    private IEnumerator DieBossCoroutine()
    {
        if (aiBehavior.target != null) {
            aiBehavior.target = null;
        }
        
        Unit unit = gameObject.GetComponent<Unit>(); // get my unit info
        UiManager.Instance.DisableBossHealthBar();

        // EffectManager.Instance.SpawnMobDieEffect(transform);
        
        if (unit._uIFXBar != null)
        {
            UI_StatusFXPoisonPool.ReturnObject(unit._uIFXBar.gameObject);
        }

        ContentsManager.Instance._uiContentsResultPanel.ClearBossAndExit();
        // StartCoroutine(ContentsManager.Instance.ExitContentsCoroutine(2));
        
        UnitObjectPool.ReturnMobs(unit, true);
        yield return null;
    }
    
    private UnityEngine.Vector3 _pos;
    private void ResetPos() {
        // for (int i = 0; i < boneSpriteList.Count; i++) {

        //     boneSpriteList[i].transform.localPosition = originBoneSpriteTransformList[i].transform.localPosition;
        // };

        transform.localPosition = _pos;
    }
}
