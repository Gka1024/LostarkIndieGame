# 🎮 Turn-Based Card Boss Battle (Unity)

Unity로 개발 중인 **턴제 카드 기반 보스전 게임 프로젝트**입니다.  
카드(스킬) 선택 → 트라이포드 분기 → 보스 패턴 AI → 버프/디버프 → UI 연출까지  
전투 시스템 구조 설계와 **확장성**에 초점을 맞춰 제작하고 있습니다.

---

## 🧭 Overview

| 항목 | 내용 |
|------|------|
| 장르 | Turn-Based / Card / Boss Battle |
| 엔진 | Unity |
| 언어 | C# |
| 핵심 | 패턴형 보스 AI / Buff & Debuff / Damage Popup / UI Tooltip |

---

## ✨ Key Features

- ✔ 턴제 전투 + 카드 선택  
- ✔ 트라이포드 기반 스킬 분기 시스템  
- ✔ Skill Queue 기반 스킬 처리 구조  
- ✔ 패턴 기반 보스 AI  
- ✔ Buff / Debuff 시스템 (ScriptableObject + Factory)  
- ✔ 데미지 / 무력화 / 파괴 수치 처리  
- ✔ 데미지 팝업 & 버프 아이콘 UI  

---

## 🔄 Turn Flow

```text
BossStart → PlayerTurn → BossAttack → TurnEnd → Loop
```

TurnStateMachine이 전체 전투 흐름을 비동기로 제어합니다.
플레이어 입력과 보스 행동을 명확히 분리한 구조입니다.

🃏 Card & Skill Queue System
플레이어가 사용한 카드는 SkillQueueData로 큐에 저장되고,
턴 종료 시 큐에 들어간 스킬을 순차 실행합니다.

public class SkillQueueData
{
    public int skillId;
    public int tripodIndex;
    public float damage;
    public float stagger;
    public float manaCost;
    public bool isChainSkill;
}
🤖 Boss AI & Pattern System
보스는 단순 공격이 아니라
패턴 선택 → 예고 → 실행 구조로 행동합니다.

다음 패턴 자동 선택

상태(그로기, 도발 등)에 따라 행동 제한

AI와 패턴 로직 분리 설계

🧪 Buff & Debuff System
ScriptableObject 기반 BuffData / DebuffData

Factory 패턴으로 런타임 객체 생성

ID 기반 Dictionary 관리

데미지 계산 파이프라인에 개입

Base Damage
 → Buff.ModifyIncomeDamage()
 → Debuff.ModifyIncomeDamage()
 → Final Damage
🧾 UI System
🟦 Buff / Debuff Icon UI
보스가 가진 상태를 아이콘으로 표시

마우스 오버 시 툴팁으로 설명 출력

🔢 Damage Popup
보스 머리 위에서 데미지 숫자 출력

월드 좌표 → Screen 좌표 변환 → Canvas 표시

🎥 FX & Visual
그로기 상태: 머리 위 Loop 이펙트

피격 시: Body Anchor 기반 단발성 Hit Effect

아이콘 영역: RectMask2D로 마스킹 처리

🗂 Project Structure
Assets/
├── Scripts/
│   ├── Turn/
│   ├── Boss/
│   ├── BuffSystem/
│   ├── Cards/
│   └── UI/
├── ScriptableObjects/
│   ├── BuffData/
│   └── SkillData/
├── Prefabs/
│   ├── UI/
│   └── Effects/
🛠 Tech Stack
Unity (2022+ LTS)

C#

ScriptableObject

TextMeshPro

State Machine

Factory Pattern

🚧 In Progress / Roadmap
상태	내용
🚧 진행중	추가 보스 패턴
🛠 작업중	크리티컬 연출
⏳ 예정	카드 시너지 시스템
📈 장기	전투 밸런싱 & 확장
🎯 Goal
이 프로젝트는
✔ 구조적으로 확장 가능한 전투 시스템
✔ 유지보수하기 쉬운 AI / Buff 설계
✔ 실전 게임 아키텍처 연습
을 목표로 제작 중입니다.

📸 Screenshots / Demo
(추후 플레이 영상 / GIF / 스크린샷 추가 예정)

📜 License
MIT License


---
