using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using Unity.VisualScripting;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class Unit : UnitBase
{
    public HealthBar healthBar;
    public HealthBarBoss bossHealthBar;
    
    // public SummonTimeBar summonTimeBar;
    
    public Transform damageTextHitPosition;
    public Transform healthBarPoint;
    // 나를 때린 몹

    public ObscuredInt mobCount; // 몹의 경우만.
    public bool isDamageOn;
    public bool isStatusOn;
    
    public DamageTaker damageTaker;
    
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    /// <summary>
    /// Animation Event (Get Animation Event from sprite frame)
    /// </summary>
    public bool canFire;

    public Animator anim;
    
    public UI_FX_Bar _uIFXBar;
    
    float maxSummonTime;
    bool isTimerStart;
    
    // [SerializeField]
    // private DamageFlash.Scripts.DamageFlash _damageFlash;

    void Awake()
    {
        isDamageOn = false;
        if (anim == null) anim = GetComponent<Animator>();
        damageTaker = GetComponent<DamageTaker>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }

        canFire = false;
    }
    
    public void Init(BigInteger hp = new BigInteger(), int damage = 0, float level_growth = 1.3f) {
        isDamageOn = false;
        isDead = false;
        if (mobSetting != null) // 일반 몹 세팅
        {
            if (GameManager.Instance._GameState == GameState.PROCESSING_CONTENTS_GOLD)
            {
                mobSetting.InitGoldDungeonGoblin(this, hp, WaveManager.Instance.goldDungeonLevel);
            }
            else if (GameManager.Instance._GameState == GameState.PROCESSING_CONTENTS_RAID)
            {
                // unit.Init(CalcRaidMobHP(5000, raidDungeonLevel), raidDungeonLevel * 10000);
                mobSetting.InitRaidDungeonMob(this, hp, WaveManager.Instance.raidDungeonLevel);
            }
            else
            {
                mobSetting.InitByWaveIndexLevel(this, hp, damage, level_growth);
            }
            
        } else if (mobSetting != null) // 보스 몹 세팅
        {
            if (GameManager.Instance._GameState == GameState.PROCESSING_CONTENTS_BOSS) // 보스전에서
            {
                mobSetting.InitDungeonBoss(this);

                return;
            }
            // mobSetting.InitBossByDB(this);
        }

        if (summonSetting != null) // 소환수 세팅
        {
            summonSetting.Init(this);
        }
    }
    
    
    public void InitCow(BigInteger hp = new BigInteger(), int damage = 0, float level_growth = 1.3f, BigInteger exp = new BigInteger()) {
        isDamageOn = false;
        isDead = false;
        if (mobSetting != null) // 일반 몹 세팅
        {
            mobSetting.InitCowDungeonMob(this, hp, damage, WaveManager.Instance.cowDungeonLevel, level_growth, exp);
        }
    }

    public bool ValidateIsDeath() {
        if (!gameObject.activeSelf || health <= 0 || isDead) { // 죽음 판정
            return true;
        }

        return false;
    }
    
    public void SetDamage(BigInteger damage, AttackType attackType = AttackType.Normal)
    {
        if (damageTaker != null)
        {
            damageTaker.TakeDamage(damage, attackType);

            if (attackType == AttackType.BONE_DRAGON)
            {
                // AudioManager.Instance.PlaySFXManager("SkillHitSFX");
                // SFXManager.Main.PlayFromSFXObjectLibrary("SkillHitSFX");
            }
            else if (attackType == AttackType.SkillEffect)
            {
                CameraShake.myCameraShake.ShakeCamera(0.035f, 0.07f);
                // AudioManager.Instance.PlaySFXManager("SkillHitSFX");

                Transform hitInstance = PoolManager.Pools["InGameParticle_Root"].Spawn(PoolManager.Pools["InGameParticle_Root"].prefabs["FX_Skill_Hit"]);
                if (hitInstance != null)
                {
                    float randomX = Random.Range(-0.1f, 0.1f); // Random Spawning Point
                    float randomY = Random.Range(-0.1f, 0.1f); // Random Spawning Point
                
                    Quaternion rot = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
                
                    UnityEngine.Vector3 newPos = new UnityEngine.Vector3(transform.position.x + randomX, transform.position.y + randomY, 0);
                    hitInstance.SetPositionAndRotation(newPos, rot);
                }
            }
        }
    }

    public void SetPoisonDamage(BigInteger damage)
    {
        if (damageTaker != null && !ValidateIsDeath())
        {
            damageTaker.TakeDamage(damage, AttackType.Poison);
        }
    }


    // 컨텐츠 등으로 전환될때 강제로 죽임
    public void EnforceDieAllies()
    {
        AiBehavior aiBehavior = GetComponent<AiBehavior>();
        aiBehavior.target = null;
        
        // TODO: 삭제 (object pool로 변경 고려)
        if (buffImage != null)
        {
            
        }

        if (healthBar != null)
        {
            UI_AlliesHealthBarPool.ReturnObject(healthBar.gameObject);
            healthBar = null;
        }

        // if (summonTimeBar != null)
        // {
        //     UI_AlliesSummonTimePool.ReturnObject(summonTimeBar.gameObject);
        //     summonTimeBar = null;
        // }
        
        Transform myInstance = PoolManager.Pools["InGameParticle_Root"].Spawn(PoolManager.Pools["InGameParticle_Root"].prefabs["SummonDieFX"]);
        if (myInstance != null) myInstance.SetPositionAndRotation(transform.position, Quaternion.identity);
        
        isDead = true;
        UnitObjectPool.ReturnAlliesEnforce(this);
    }
    
    
    public void EnforceDieAlliesOnce(Unit unit)
    {
        AiBehavior aiBehavior = unit.GetComponent<AiBehavior>();
        aiBehavior.target = null;
        
        // TODO: 삭제 (object pool로 변경 고려)
        if (buffImage != null)
        {
            
        }

        if (unit.healthBar != null)
        {
            UI_AlliesHealthBarPool.ReturnObject(unit.healthBar.gameObject);
            unit.healthBar = null;
        }

        // if (summonTimeBar != null)
        // {
        //     UI_AlliesSummonTimePool.ReturnObject(summonTimeBar.gameObject);
        //     summonTimeBar = null;
        // }
        
        Transform myInstance = PoolManager.Pools["InGameParticle_Root"].Spawn(PoolManager.Pools["InGameParticle_Root"].prefabs["SummonDieFX"]);
        if (myInstance != null) myInstance.SetPositionAndRotation(transform.position, Quaternion.identity);
        
        unit.isDead = true;
        UnitObjectPool.ReturnAlliesEnforceOnce(unit);
    }

    public void EnforceDieBombMob()
    {
        damageTaker.DieMobByDeathSkill();
    }

    public void EnforceDieMobs()
    {
        AiBehavior aiBehavior = GetComponent<AiBehavior>();
        aiBehavior.target = null;

        if (healthBar != null)
        {
            UI_MobHealthBarPool.ReturnObject(healthBar.gameObject);
            healthBar = null;
        }

        UnitObjectPool.ReturnMobsEnforce(this);
    }

    public void EnforceDieBossMob()
    {
        if (GameManager.Instance._GameState != GameState.PROCESSING_CONTENTS_RAID && GameManager.Instance._GameState != GameState.PROCESSING_CONTENTS_AWAKENING)
        {
            AiBehavior aiBehavior = GetComponent<AiBehavior>();
            aiBehavior.target = null;
        
            UiManager.Instance.DisableBossHealthBar();
            // UiManager.Instance.UI_HealthBarBoss.Init(0);
            bossHealthBar = null;
        }
        else
        {
            PrefabManager.Instance.DestroyRaid();
        }

        UnitObjectPool.ReturnMobsEnforce(this);
    }

    float hitTimeLeft;
    float hitMaxTime = 0.15f;
    private Coroutine flashRoutine;

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        
        flashRoutine = StartCoroutine(MobHitEffect());
    }

    public IEnumerator MobHitEffect()
    {
        hitTimeLeft = 0f;
        isDamageOn = true;
        
        spriteRenderer.material.SetFloat("_HitEffectBlend", 0f);

        while (hitTimeLeft < hitMaxTime)
        {
            hitTimeLeft += Time.deltaTime;
            // yield return delayTime;

            float scale = hitTimeLeft / hitMaxTime;
            if (scale < 0.6f)
            {
                spriteRenderer.material.SetFloat("_HitEffectBlend", (hitTimeLeft / hitMaxTime));
            }

            yield return null;
        }
        
        spriteRenderer.material.SetFloat("_HitEffectBlend", 0f);
        isDamageOn = false;
        hitTimeLeft = 0f;
    }

    BigInteger summonBasisDamage = new BigInteger();
    public BigInteger IsNearTarget()
    {
        AiBehavior aiBehavior = GetComponent<AiBehavior>();
        if (aiBehavior != null && aiBehavior.target != null)
        {
            if (aiBehavior.target.gameObject.name.Equals(ConstantValues.RAID_NAME_Clone) || aiBehavior.target.gameObject.name.Equals(ConstantValues.AWAKENING_BOSS_NAME))
            {
                if (Abs(Vector2.Distance(transform.position, aiBehavior.target.transform.position)) <= 1.75f)
                {
                    if (StatManager.Instance.Stat_KeepWeaponTotalPercentage > 0)
                    {
                        summonBasisDamage = CalClass.CalIncreasedPercentDamage(damage, StatManager.Instance.Stat_KeepWeaponTotalPercentage);
                    }
                    else
                    {
                        summonBasisDamage = damage;
                    }

                    summonBasisDamage = StatManager.Instance.CalcSummonAttackIncreasedDamage(summonBasisDamage); // 군단 강화 적용
                    if (critical_attack_increase_damage > 0)
                    {
                        summonBasisDamage = CalClass.CalIncreasedPercentDamage(summonBasisDamage, critical_attack_increase_damage);
                    }

                    if (summonBasisDamage > 0)
                    {
                        return summonBasisDamage;
                    }
                    
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (Abs(Vector2.Distance(transform.position, aiBehavior.target.transform.position)) <= 0.7f)
                {
                    if (StatManager.Instance.Stat_KeepWeaponTotalPercentage > 0)
                    {
                        summonBasisDamage = CalClass.CalIncreasedPercentDamage(damage, StatManager.Instance.Stat_KeepWeaponTotalPercentage);
                    }
                    else
                    {
                        summonBasisDamage = damage;
                    }

                    summonBasisDamage = StatManager.Instance.CalcSummonAttackIncreasedDamage(summonBasisDamage); // 군단 강화 적용
                    if (critical_attack_increase_damage > 0)
                    {
                        summonBasisDamage = CalClass.CalIncreasedPercentDamage(summonBasisDamage, critical_attack_increase_damage);
                    }

                    if (summonBasisDamage > 0)
                    {
                        return summonBasisDamage;
                    }
                    
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
        }
        
        return 0;
    }

    private float Abs(float res)
    {
        if (res > 0)
        {
            return res;
        }

        return res * -1;
    }
    

    float timeLeft;

    public IEnumerator SummonLifeTime(float unitSummonTime)
    {
        timeLeft = unitSummonTime;
        while (timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        if (!ValidateIsDeath())
        {
            EnforceDieAllies();
        }
    }
}
