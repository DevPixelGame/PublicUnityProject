using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using DevionGames.UIWidgets;
using PathologicalGames;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.Events;

public class UnitBase : MonoBehaviour
{
    [Header("Mob Stats")] // 인스펙터창에 표시됨.
    public int level = 1;
    public string name;
    public BigInteger health;
    public int maxHealth;
    public BigInteger exp;
    public int gold;
    public int bloodGem;
    public bool isDead;
    public BigInteger damage;
    public float cooldown; // attack cooldown
    public float critical_attack_chance; // attack cooldown
    public float critical_attack_increase_damage; // attack cooldown
    public float moveSpeed;
    public float lifetime;// 소환수의 쿨타임 (소환시간)
    public SpriteRenderer _spriteRenderer;
    
    [Header("Status Buff")] /// 상태이상 정보
    public bool[] buffAt = { false, false, false };
    public Transform buffImage;

    private void Awake()
    {
        buffAt[0] = false;
        buffAt[1] = false;
        buffAt[2] = false;
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    

    public Coroutine iceStatusSkillCouroutine;
    public Coroutine slowStatusSkillCouroutine;
    public Coroutine wallStopStatusSkilCoroutine;
    public Coroutine sturnStopStatusSkilCoroutine;
    
    public bool enableKnockBack;
    public float knockbackDuration = 0.1f;
    public float knockbackPower = 0.75f;
    public IEnumerator KnockBack(float knockDur, float knockPow, Transform obj) // obj : 스킬 발동자
    {
        float timer = 0;
        Vector2 knockbackPos;
        if (transform.position.x > obj.position.x)
        {
            knockbackPos = transform.position + (transform.position - obj.position).normalized * knockbackPower; // 기본
        }
        else
        {
            knockbackPos = transform.position - (transform.position - obj.position).normalized * knockbackPower;
        }
        
        knockbackPos.y = transform.position.y; // y축 고정

        while (knockbackDuration > timer)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, knockbackPos, knockbackDuration);
        }
        
        yield return 0f;
    }
    
    public IEnumerator StopMove(float duration)
    {
        float timer = 0;
        while (knockbackDuration > timer)
        {
            timer += Time.deltaTime;
            
        }
        
        yield return 0f;
    }
    
    public IEnumerator Sturn(float duration)
    {
        float timer = 0;
        while (knockbackDuration > timer)
        {
            timer += Time.deltaTime;
            
        }
        
        yield return 0f;
    }
    
    
    
    public IEnumerator Slow(float duringTime, float slowPercentage)
    {
        Unit my = GetComponent<Unit>();

        float timer = 0;
        float originMoveSpeed = moveSpeed;
        float slowTo = originMoveSpeed * slowPercentage * 0.01f;
        
        my.moveSpeed = moveSpeed - slowTo;
        yield return YieldInstructionCache.WaitForSeconds(duringTime);
        my.moveSpeed = originMoveSpeed;

        yield return 0f;
    }
    
    Coroutine heroSkillCoroutine;
    public void StartIceStatusSkill(float duringTime)
    {
        if (heroSkillCoroutine != null)
        {
            StopCoroutine(heroSkillCoroutine);
        }
        heroSkillCoroutine = StartCoroutine(IceSkill(duringTime));
    }

    public void StartSlowStatusSkill(float duringTime, float value)
    {
        if (heroSkillCoroutine != null)
        {
            StopCoroutine(heroSkillCoroutine);
        }
        heroSkillCoroutine = StartCoroutine(Slow(duringTime, value));
    }
    
    public void StartWallStopStatusSkill(float duringTime, BigInteger value)
    {
        if (heroSkillCoroutine != null)
        {
            StopCoroutine(heroSkillCoroutine);
        }
        heroSkillCoroutine = StartCoroutine(MoveStop(duringTime, value));
    }
    
    public void StartSturnStatusSkill(float duringTime)
    {
        if (heroSkillCoroutine != null)
        {
            StopCoroutine(heroSkillCoroutine);
        }

        AudioManager.Instance.PlaySFXManager("Status_Sturn_SFX");

        
        Unit my = GetComponent<Unit>();
            
        Transform myInstance = PoolManager.Pools["UI_StatusPool"].Spawn(PoolManager.Pools["UI_StatusPool"].prefabs["UI_FX_Status_Sturn"]);
        buffImage = myInstance;

        // my.SetDamage(damage, AttackType.SkillEffect);

        UI_FX_Bar timerBar = myInstance.GetComponent<UI_FX_Bar>();
        if (transform.CompareTag(ConstantValues.MOB_TAG) || transform.CompareTag(ConstantValues.BOMB_GOBLIN_TAG))
        {
            my._uIFXBar = timerBar;
            timerBar.target = gameObject;
            timerBar.targetHealthBarPosition = my.healthBarPoint;
            timerBar.FixedPosition();
        }
        else
        {
            my._uIFXBar = timerBar;
            timerBar.target = gameObject;
            timerBar.targetHealthBarPosition = my.damageTextHitPosition;
            timerBar.FixedPositionByDamageText();
        }
        
        heroSkillCoroutine = StartCoroutine(OnSturnCoroutines(duringTime));
    }

    public void StopStatusSkillCoroutine()
    {
        if (heroSkillCoroutine != null)
        {
            StopCoroutine(heroSkillCoroutine);
        }
    }
    
    

    WaitForSeconds iceTime = new WaitForSeconds(3f);
    public IEnumerator IceSkill(float duringTime)
    {
        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.blue;
        Animator anim = GetComponent<Animator>();
        anim.enabled = false;
        
        AiBehavior aiBehavior = transform.GetComponent<AiBehavior>();
        aiBehavior.enabled = false;
        aiBehavior.navAgent.move = false;

        if (gameObject.CompareTag(ConstantValues.DUNGEON_BOSS_TAG) || gameObject.CompareTag(ConstantValues.BOSS_TAG))
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);
        }
        else
        {
            yield return YieldInstructionCache.WaitForSeconds(duringTime);
        }

        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        aiBehavior = transform.GetComponent<AiBehavior>();
        
        anim.enabled = true;
        spriteRenderer.color = Color.white;
        if (!aiBehavior.enabled)
        {
            aiBehavior.enabled = true;
        }
        

        if (!aiBehavior.navAgent.isMobReached) aiBehavior.navAgent.move = true;

        yield return 0f;
    }

    public void StopSkillStatusCoroutine()
    {
        if (iceStatusSkillCouroutine != null)
        {
            StopCoroutine(iceStatusSkillCouroutine);
        }
    }
    
    
    
    
    
    WaitForSeconds stopTime = new WaitForSeconds(1.0f);


    public IEnumerator MoveStop(float duringTime, BigInteger damage)
    {

        AiBehavior aiBehavior = transform.GetComponent<AiBehavior>();
        aiBehavior.enabled = false;
        aiBehavior.navAgent.move = false;
        
        Unit unit = GetComponent<Unit>();
        unit.SetDamage(damage, AttackType.SkillEffect);
        yield return YieldInstructionCache.WaitForSeconds(duringTime);
        aiBehavior = transform.GetComponent<AiBehavior>();
        
        if (!aiBehavior.enabled)
        {
            aiBehavior.enabled = true;
        }
        
        if (!aiBehavior.navAgent.isMobReached) aiBehavior.navAgent.move = true;
        

        yield return null;
    }
    
    
    public IEnumerator Debuff(float duringTime)
    {
        yield return null;
    }
    
    
    
    
    
    public void DeathSkill()
    {
        Unit unit = GetComponent<Unit>();

        Transform myInstance = PoolManager.Pools["DamageText_Root"].Spawn(PoolManager.Pools["DamageText_Root"].prefabs["TextStatusPopUp"]);

        if (myInstance != null) {
            myInstance.SetPositionAndRotation(transform.position, UnityEngine.Quaternion.identity);
            TextStatusPopUp textStatusPopUp = myInstance.GetComponent<TextStatusPopUp>();
            // damagePopUp.tmp.color.a = 1f;
            textStatusPopUp.StartTween("즉사");
        }
        
        if (!unit.ValidateIsDeath()) unit.damageTaker.DieMobByDeathSkill();
    }

    
    public IEnumerator WraithDeathSkill(BigInteger damage)
    {
        Unit unit = GetComponent<Unit>();
        if (gameObject.CompareTag(ConstantValues.DUNGEON_BOSS_TAG) || gameObject.CompareTag(ConstantValues.BOSS_TAG))
        {
            unit.SetDamage(damage, AttackType.SkillEffect);

            yield break;
        }
        
        
        float healthNokoruPercentage = CalClass.GetBigIntPercentage(unit.health, unit.maxHealth);
        if (healthNokoruPercentage < 0.25f)
        {
            float randProb = Random.Range(0, 100f);
            if (randProb < 5f)
            {
                Transform myInstance = PoolManager.Pools["DamageText_Root"].Spawn(PoolManager.Pools["DamageText_Root"].prefabs["TextStatusPopUp"]);

                if (myInstance != null) {
                    myInstance.SetPositionAndRotation(transform.position, UnityEngine.Quaternion.identity);
                    TextStatusPopUp textStatusPopUp = myInstance.GetComponent<TextStatusPopUp>();
                    // damagePopUp.tmp.color.a = 1f;
                    textStatusPopUp.StartTween("즉사");
                }
            
                if (!unit.ValidateIsDeath()) unit.damageTaker.DieMobByDeathSkill();
            }
            else
            {
                unit.SetDamage(damage, AttackType.SkillEffect);
            }
        }
        else
        {
            unit.SetDamage(damage, AttackType.SkillEffect);
        }


        
        yield return null;
    }
    
    
    WaitForSeconds poisonTime = new WaitForSeconds(3f);
    WaitForSeconds poisonPerTime = new WaitForSeconds(0.5f);
    public void PoisonSkill(float duringTime, BigInteger fixedDamagePerSecond)
    {
        if (buffAt[0])
        {

        }
        Unit my = GetComponent<Unit>();

        if (my._uIFXBar == null)
        {
            
            Transform myInstance = PoolManager.Pools["UI_StatusPool"].Spawn(PoolManager.Pools["UI_StatusPool"].prefabs["UI_FX_Status_Poison"]);
            buffImage = myInstance;

            UI_FX_Bar timerBar = myInstance.GetComponent<UI_FX_Bar>();
            my._uIFXBar = timerBar;
            // life time: 소환시간
        
            timerBar.offset = new UnityEngine.Vector3(0f, my.spriteRenderer.bounds.size.y * 0.6f, 0f);
            timerBar.targetHealthBarPosition = my.healthBarPoint;
            timerBar.StartReposition = true;
            timerBar.target = gameObject;
        }

        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.green;

        StartCoroutine(OnBuffCoroutines("Addition", duringTime, fixedDamagePerSecond, 0)); // index: 0 - 독
    }

    public void FireSkill(float duringTime, BigInteger fixedDamagePerSecond)
    {
        if (buffAt[0])
        {

        }
        Unit my = GetComponent<Unit>();

        if (my._uIFXBar == null)
        {
            
            Transform myInstance = PoolManager.Pools["UI_StatusPool"].Spawn(PoolManager.Pools["UI_StatusPool"].prefabs["UI_FX_Status_Poison"]);
            buffImage = myInstance;

            UI_FX_Bar timerBar = myInstance.GetComponent<UI_FX_Bar>();
            my._uIFXBar = timerBar;
            // life time: 소환시간
        
            timerBar.offset = new UnityEngine.Vector3(0f, my.spriteRenderer.bounds.size.y * 0.6f, 0f);
            timerBar.targetHealthBarPosition = my.healthBarPoint;
            timerBar.StartReposition = true;
            timerBar.target = gameObject;
        }

        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.green;

        StartCoroutine(FireEndSkillCoroutine(duringTime, fixedDamagePerSecond, 0)); // index: 0 - 독
    }
    
    IEnumerator FireEndSkillCoroutine(float time, BigInteger damage, int index)
    {

        buffAt[index] = true;
        Unit unit = GetComponent<Unit>();
        while (time > 0)
        {
            unit.SetPoisonDamage(damage);
            time--;
            yield return YieldInstructionCache.WaitForSeconds(1f);
        }

        buffAt[index] = false;
        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;

        if (buffImage != null)
        {
            if (PoolManager.Pools["UI_StatusPool"].IsSpawned(buffImage))
            {
                PoolManager.Pools["UI_StatusPool"].Despawn(buffImage);
            }
        }
    }
    

    IEnumerator OnBuffCoroutines(string operation, float time, BigInteger damage, int index)
    {

        buffAt[index] = true;
        Unit unit = GetComponent<Unit>();
        while (time > 0)
        {
            unit.SetPoisonDamage(damage);
            time--;
            yield return YieldInstructionCache.WaitForSeconds(1f);
        }

        buffAt[index] = false;
        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;

        if (buffImage != null)
        {
            if (PoolManager.Pools["UI_StatusPool"].IsSpawned(buffImage))
            {
                PoolManager.Pools["UI_StatusPool"].Despawn(buffImage);
            }
        }
    }


    
    public void SummonAttackSpeedUpBySkill()
    {
        cooldown = 0.75f;
        buffAt[0] = true; // 공격속도 증가
        if (_SpriteRenderer == null)
        {
            _SpriteRenderer = transform.GetComponent<SpriteRenderer>();
            _SpriteRenderer.color = Color.red;
        }
        else
        {
            _SpriteRenderer.color = Color.red;
        }
        // moveSpeed = 1.15f;
        // health += 1;
        // healthBar.SetHealth(health);
        //
        // spriteRenderer.color = Color.red;
        
    }

    public void ResetBuff()
    {
        if (buffImage != null)
        {
            if (PoolManager.Pools["UI_StatusPool"].IsSpawned(buffImage)) PoolManager.Pools["UI_StatusPool"].Despawn(buffImage);
        }
        
        buffImage = null;
        buffAt[0] = false; // 스턴
        buffAt[1] = false; //
        buffAt[2] = false;
    }
    
    IEnumerator OnSturnCoroutines(float time)
    {
        AiBehavior aiBehavior = transform.GetComponent<AiBehavior>();
        aiBehavior.enabled = false;
        aiBehavior.navAgent.move = false;
        
        Animator anim = GetComponent<Animator>();
        anim.enabled = false;
        
        while (time > 0)
        {
            time--;
            yield return YieldInstructionCache.WaitForSeconds(1);
        }

        anim.enabled = true;
        
        if (aiBehavior == null) transform.GetComponent<AiBehavior>();
        if (!aiBehavior.enabled)
        {
            aiBehavior.enabled = true;
        }
        
        if (!aiBehavior.navAgent.isMobReached) aiBehavior.navAgent.move = true;
        if (PoolManager.Pools["UI_StatusPool"].IsSpawned(buffImage))
        {
            PoolManager.Pools["UI_StatusPool"].Despawn(buffImage);
        }
    }



    public void BuffSummonHeal()
    {
        Transform myInstance = PoolManager.Pools["InGameParticle_Root"].Spawn(PoolManager.Pools["InGameParticle_Root"].prefabs["FX_Buff_Heal"]);
        myInstance.SetPositionAndRotation(transform.position, Quaternion.identity);
    }
    
    
    
    public void SkeletonArcherSpeedUp(float value)
    {
        cooldown = value;
        buffAt[0] = true; // 공격속도 증가
    }
}
