# 🎮 Turn-Based Card Boss Battle (Unity)

Unity로 개발 중인 **턴제 카드 기반 보스전 게임 프로젝트**입니다.  
전투 흐름, 스킬 처리 구조, 보스 AI, Buff/Debuff 파이프라인 등  
**게임 전투 시스템 아키텍처 설계**에 초점을 맞춰 제작하고 있습니다.

---

## 🧭 Project Overview

| 항목 | 내용 |
|------|------|
| 장르 | Turn-Based / Card / Boss Battle |
| 엔진 | Unity |
| 언어 | C# |
| 핵심 | 전투 흐름 설계 / 패턴형 보스 AI / Buff & Debuff 시스템 |

---

## 🔄 1. Turn Flow Architecture

전투는 `TurnStateMachine`이 전체 흐름을 관리합니다.

```text
BossStart → PlayerTurn → SkillQueue → BossAction → TurnEnd → Loop
```

플레이어 입력과 보스 행동을 명확히 분리

스킬은 즉시 실행되지 않고 큐에 저장 → 턴 종료 시 처리

상태 전환은 비동기 코루틴 기반으로 안전하게 제어

🃏 2. Card & Skill Queue System
플레이어가 사용한 카드는 SkillQueueData로 변환되어 큐에 저장됩니다.
턴 종료 시 큐에 쌓인 스킬을 순차 실행하는 구조입니다.

public class SkillQueueData
{
    public int skillId;
    public int tripodIndex;
    public float damage;
    public float stagger;
    public bool isChainSkill;
}
설계 포인트
카드 → 데이터 → 큐 → 실행 흐름 분리

체인 스킬 / 분기 스킬도 동일한 파이프라인에서 처리

UI / 연출 / 계산 로직이 서로 직접 의존하지 않도록 분리

🤖 3. Boss AI & Pattern System
보스는 단순 공격이 아니라 패턴 기반 AI 구조로 동작합니다.

패턴 선택 → 예고 → 실행 → 종료 → 다음 패턴 선택
설계 포인트
AI는 패턴을 선택만 하고, 실행은 BossPattern이 담당

상태(그로기, 도발 등)에 따라 행동 제한

AI / 패턴 / 애니메이션 로직 분리

🧪 4. Buff & Debuff Damage Pipeline
Buff / Debuff는 ScriptableObject 기반 데이터 + 런타임 객체 구조입니다.

Base Damage
 → Buff.ModifyIncomeDamage()
 → Debuff.ModifyIncomeDamage()
 → Final Damage
설계 포인트
Buff는 Factory 패턴으로 런타임 객체 생성

ID 기반 Dictionary로 중복/중첩 관리

데미지 계산 흐름에 직접 개입하는 파이프라인 구조
