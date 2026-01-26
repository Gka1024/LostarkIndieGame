🎮 Turn-Based Card Boss Battle (Unity)

Unity로 제작 중인 턴제 카드 기반 보스전 게임
카드 + 패턴 + 상태효과 + UI 연출을 구조적으로 설계한 전투 시스템 프로젝트

🧭 Overview
항목	내용
장르	Turn-Based / Card / Boss Battle
엔진	Unity
언어	C#
특징	패턴형 보스 AI / Buff & Debuff / Damage Popup / UI Tooltip
✨ Key Features

✔ 턴제 전투 + 카드 선택
✔ 트라이포드 기반 스킬 분기
✔ 보스 패턴 AI
✔ Buff / Debuff 구조화 시스템
✔ 데미지 / 무력화 / 파괴 수치 처리
✔ 실시간 UI 연출

🔄 Turn Flow
BossStart → PlayerTurn → BossAttack → TurnEnd → Loop


TurnStateMachine이 전체 전투 흐름을 비동기 구조로 제어합니다.

🃏 Card & Skill Queue

플레이어가 사용한 스킬은 SkillQueueData로 큐에 저장

턴 종료 시 큐에 들어간 스킬을 순차 실행

public class SkillQueueData
{
    public int skillId;
    public int tripodIndex;
    public float damage;
    public float stagger;
}

🤖 Boss AI & Pattern System

보스는 단순히 “공격”하는 것이 아니라
패턴을 선택 → 예고 → 실행하는 구조입니다.

다음 패턴 자동 선택

상태(그로기/도발)에 따라 행동 제한

AI와 패턴 로직 분리

🧪 Buff & Debuff System

ScriptableObject 기반 데이터 설계

Factory 패턴으로 런타임 객체 생성

ID 기반 Dictionary 관리

Damage → Buff.ModifyIncomeDamage() → Final Damage

🧾 UI System
🟦 Buff Icon UI

보스가 가진 상태를 아이콘으로 표시

마우스 오버 시 설명 툴팁 출력

🔢 Damage Popup

보스 머리 위에서 데미지 숫자 출력

월드 → Screen 좌표 변환 후 Canvas에 표시

🗂 Project Structure
Assets/
├── Scripts/
│   ├── Turn/
│   ├── Boss/
│   ├── BuffSystem/
│   ├── Cards/
│   └── UI/

🛠 Tech Stack

Unity

C#

ScriptableObject

TextMeshPro

State Machine

Factory Pattern

🚧 In Progress

추가 보스 패턴

크리티컬 연출

카드 시너지 시스템

보스 상태별 전용 이펙트

🎯 Goal

확장 가능하고 유지보수 쉬운
전투 시스템 아키텍처 설계를 목표로 한 프로젝트입니다.
