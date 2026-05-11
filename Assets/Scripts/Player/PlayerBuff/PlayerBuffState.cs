using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBuffState : MonoBehaviour
{
    private List<PlayerBuff> _activeBuffs = new();
    private PlayerStats _stats;
    public PlayerStatsUI playerStatsUI;

    private void Awake() => _stats = GetComponent<PlayerStats>();

    public void AddBuff(PlayerBuff newBuff)
    {
        var existing = _activeBuffs.Find(b => b.Data.buffID == newBuff.Data.buffID);

        if (existing == null) // 아예 새로운 버프인 경우
        {
            _activeBuffs.Add(newBuff);
            newBuff.OnApply(_stats); // 여기서 에러나는 경우 버프 플레이어에게 등록했는지 확인
        }
        else if (existing != null) // 존재하는 버프가 중첩되는 경우
        {
            if (existing is PlayerBuffShield shieldBuff || existing is PlayerBuffAttack attackbuff) // 인데 쉴드나 데미지 증가인 경우 그냥추가
            {
                _activeBuffs.Add(newBuff);
                newBuff.OnApply(_stats);
                return;
            }
            else // 가 아니라 단순 중첩인 경우 지속시간 증가
            {
                existing.Duration += newBuff.Duration;
            }
        }
    }

    public void RemoveBuff(BuffID_Player id)
    {
        var target = _activeBuffs.Find(b => b.Data.buffID == (int)id);
        if (target != null)
        {
            target.OnRemove(_stats);
            _activeBuffs.Remove(target);
        }
    }

    // [실드 로직] 리스트를 순회하며 데미지를 깎음
    public float AbsorbDamageWithShields(float damage)
    {
        var shields = _activeBuffs.OfType<PlayerBuffShield>().OrderBy(s => s.Duration).ToList();

        foreach (var shield in shields)
        {
            if (damage <= 0) break;
            float absorb = Mathf.Min(damage, shield.Amount);
            shield.Amount -= absorb;
            damage -= absorb;

            if (shield.Amount <= 0) RemoveBuff(BuffID_Player.PLAYER_SHIELD);
        }
        return damage;
    }

    public void OnTurnEnd()
    {
        for (int i = _activeBuffs.Count - 1; i >= 0; i--)
        {
            _activeBuffs[i].OnTick(_stats);
            if (_activeBuffs[i].Duration <= 0)
            {
                _activeBuffs[i].OnRemove(_stats);
                _activeBuffs.RemoveAt(i);
            }
        }

        playerStatsUI.UpdateBuffs(_activeBuffs);
        Debug.Log($"PlayerBuff : 현재 버프 개수 = {_activeBuffs.Count}");
    }

    public bool HasPlayerCC()
    {
        foreach (var buff in _activeBuffs)
        {
            if (buff.ID == (int)BuffID_Player.SILENCE) return true;
            if (buff.ID == (int)BuffID_Player.DOWN) return true;
            if (buff.ID == (int)BuffID_Player.STUN) return true;
        }

        return false;
    }

    public bool HasPlayerBuffs(BuffID_Player id)
    {
        foreach (var buff in _activeBuffs)
        {
            if (buff.ID == (int)id) return true;
        }
        return false;
    }

    public PlayerBuff GetPlayerBuff(BuffID_Player id)
    {
        foreach (var buff in _activeBuffs)
        {
            if (buff.ID == (int)id) return buff;
        }
        return null;
    }

    // 스탯 보정치 계산 메서드들...
    public float GetCalculatedAttack(float baseAtk)
    {
        float baseAttack = baseAtk;

        foreach (var buff in _activeBuffs)
        {
            baseAttack = buff.ModifyAttack(baseAttack);
        }

        return baseAttack;
    }

    public float GetAdditionalManaRegen(float baseRegen)
    {
        float manaRegen = baseRegen;

        foreach (var buff in _activeBuffs)
        {
            if (buff is PlayerBuffManaRegen manabuff)
            {
                manaRegen *= 1 + (manabuff.value * 0.01f);
            }
        }

        return manaRegen;
    }

    public float GetCurrentShield()
    {
        float shield = 0;

        foreach (var buff in _activeBuffs)
        {
            if (buff is PlayerBuffShield shieldbuff)
            {
                shield += shieldbuff.Amount;
            }

            if (buff is PlayerBuffShieldCounter shieldCounter)
            {
                shield += shieldCounter.Amount;
            }
        }

        return shield;
    }
}