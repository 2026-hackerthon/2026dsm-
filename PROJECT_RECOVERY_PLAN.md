# 프로젝트 0단계 복구 계획서

## 1. 목적

현재 `dev` 브랜치의 프로젝트를 다음 상태까지 복구한다.

- URP 2D 렌더링이 정상 작동한다.
- 플레이어 이동 입력 스크립트가 정상 컴포넌트로 연결된다.
- 시작 맵 로딩과 맵 전환이 예외 없이 동작한다.
- `TitleScene -> GameplayScene` 흐름으로 에디터와 빌드에서 실행된다.
- Unity Console에 컴파일 오류, Missing Script, NullReferenceException이 없다.

이 단계에서는 구조 시스템, 대화, 추가 스테이지 기믹을 구현하지 않는다. Assembly Definition, DI 컨테이너, 서비스 로케이터, 에디터 자동화 도구도 추가하지 않는다.

---

## 2. 현재 확인된 손상 상태

| 영역 | 현재 상태 |
| --- | --- |
| URP | `QualitySettings`가 존재하지 않는 `UniversalRP.asset` GUID를 참조함 |
| Renderer2D | `Assets/Settings/Renderer2D.asset`이 없음 |
| 플레이어 입력 | 파일명 `PlayerInputlReader.cs`와 클래스명 `PlayerInputReader`가 다름 |
| Gameplay 플레이어 | `MapLoading.unity`에 존재하지 않는 스크립트 GUID가 남아 있음 |
| MapDatabase | `MapDatabase.asset`이 `MapManager` 데이터로 덮여 있음 |
| MapManager | `MapLoading.unity`의 `mapDatabase` 참조가 비어 있음 |
| MapDataEntry | 기존 에셋의 `m_Script` 참조가 유효하지 않음 |
| Build Settings | `SampleScene` 하나만 등록되어 있음 |
| 게임 시작 | `GameStartUI`가 고정 빌드 인덱스 `1`을 사용함 |

---

## 3. 작업 원칙

1. Unity 에셋을 복구하거나 이름을 바꿀 때 `.meta` 파일을 함께 보존한다.
2. 한 항목을 수정한 뒤 Unity 컴파일과 Console을 확인하고 다음 항목으로 이동한다.
3. 복구 단계에서는 현재 맵 시스템을 정상화하는 데 집중하고 새 맵 아키텍처로 교체하지 않는다.
4. 손상된 ScriptableObject는 YAML을 직접 고치지 않고 정상 스크립트로 다시 생성한다.
5. 새 맵 생성 성공을 확인하기 전에 기존 맵을 제거하지 않는다.

---

## 4. 작업 순서 요약

```text
1. URP/Renderer2D 에셋 복구
2. 플레이어 입력 코드와 컴포넌트 복구
3. MapData 스크립트 분리 및 데이터 에셋 재생성
4. GameplayScene 구성과 Missing Script 제거
5. Build Settings 및 씬 이동 코드 수정
6. 에디터/빌드 통합 테스트
```

---

# 5. URP와 Renderer2D 설정 복구

## 5.1 C#에서 할 일

없음.

URP Pipeline Asset과 Renderer2D Data는 C# 코드가 아니라 Unity 에셋이다. 이 문제를 런타임 코드로 우회하지 않는다.

## 5.2 파일 및 Git에서 할 일

`main` 브랜치에 남아 있는 다음 에셋과 `.meta`를 현재 브랜치로 복구한다.

```text
Assets/Settings.meta
Assets/Settings/UniversalRP.asset
Assets/Settings/UniversalRP.asset.meta
Assets/Settings/Renderer2D.asset
Assets/Settings/Renderer2D.asset.meta
```

기존 GUID를 보존하면 `QualitySettings`의 `681886c5eb7344803b6206f758bf0b1c` 참조가 다시 연결된다. 복구 전에 Unity Editor를 닫아 파일 재임포트와 Git 작업이 동시에 일어나지 않게 한다.

원본 에셋이 현재 Unity 버전에서 열리지 않을 때만 Unity Editor에서 새 `URP Asset with 2D Renderer`를 생성한다. 원본 복구와 신규 생성을 동시에 하지 않는다.

## 5.3 Unity Editor에서 할 일

1. 프로젝트를 열고 에셋 임포트가 끝날 때까지 기다린다.
2. `Project Settings > Graphics`에서 URP Global Settings가 연결되어 있는지 확인한다.
3. `Project Settings > Quality`의 모든 품질 단계에서 `UniversalRP`를 지정한다.
4. `UniversalRP.asset`의 Renderer List에 `Renderer2D.asset`이 등록되어 있는지 확인한다.
5. 각 카메라가 2D Renderer를 사용하는지 확인한다.

## 5.4 완료 확인

- Sprite Lit 머티리얼이 분홍색으로 표시되지 않는다.
- Global Light 2D가 씬에서 정상 작동한다.
- Console에 Render Pipeline Asset 또는 Renderer Data 누락 경고가 없다.
- `QualitySettings.asset`이 가리키는 URP GUID가 실제 `.meta`에 존재한다.

---

# 6. 플레이어 입력 스크립트와 씬 참조 복구

## 6.1 C#에서 할 일

### 파일명 수정

```text
PlayerInputlReader.cs
-> PlayerInputReader.cs
```

Unity Project 창에서 이름을 바꿔 기존 `.meta` GUID를 유지한다. 클래스명은 이미 `PlayerInputReader`이므로 파일명과 일치시킨다.

### PlayerInputReader 정리

- 사용하지 않는 `using Unity.VisualScripting;`을 제거한다.
- `Start()` 대신 `Awake()`에서 `PlayerMove`를 찾는다.
- `PlayerMove`가 없으면 명확한 오류를 출력하고 입력 처리를 중지한다.
- `OnMove()`는 입력 벡터를 읽어 `PlayerMove.SetMoveInput()`에 전달한다.
- `OnInteract()`는 0단계에서는 비워 두되 예외가 발생하지 않아야 한다.

필요한 공개 메서드 형태는 다음 정도면 충분하다.

```csharp
public void SetMoveInput(Vector2 input)
public void OnMove(InputAction.CallbackContext context)
public void OnInteract(InputAction.CallbackContext context)
```

### PlayerMove 정리

- 공개 필드 `dirVec` 대신 private 이동 입력을 사용한다.
- `SetMoveInput(Vector2 input)`을 추가한다.
- `FixedUpdate()`에서 Rigidbody2D 속도만 갱신한다.
- `scanObject.name`을 읽는 F키 디버그 코드를 제거한다.
- 상호작용과 Raycast 코드는 0단계 복구 범위에서 제거한다.
- SpriteRenderer와 Rigidbody2D가 없을 때 NullReferenceException이 발생하지 않게 `Awake()`에서 검증한다.

## 6.2 Unity Editor에서 할 일

1. 스크립트 이름 변경 후 컴파일이 끝날 때까지 기다린다.
2. `SampleScene`의 Player에서 잘못 연결된 `PlayerInput` 또는 Missing Script 컴포넌트를 제거한다.
3. `PlayerInputReader` 컴포넌트를 다시 추가한다.
4. `PlayerInput` 컴포넌트의 Actions에 `Assets/PlayerScript/Player.inputactions`를 지정한다.
5. Default Action Map을 `Player`로 지정한다.
6. Behavior는 `Invoke Unity Events` 하나로 통일한다.
7. Move 이벤트를 `PlayerInputReader.OnMove`에 연결한다.
8. Interact 이벤트를 `PlayerInputReader.OnInteract`에 연결한다.
9. Player 오브젝트에 `Player` 태그, Rigidbody2D, Collider2D, SpriteRenderer, PlayerMove가 모두 있는지 확인한다.
10. 정상 Player를 Prefab으로 만든 뒤 GameplayScene에서도 같은 Prefab을 사용한다.

## 6.3 완료 확인

- Player Inspector에 Missing Script가 없다.
- WASD 또는 방향키 입력으로 플레이어가 움직인다.
- 대각선 이동이 수평 이동보다 빠르지 않다.
- 입력을 놓으면 Rigidbody2D 속도가 0이 된다.
- F키를 눌러도 NullReferenceException이 발생하지 않는다.

---

# 7. 손상된 맵 데이터와 누락 스크립트 복구

## 7.1 C#에서 할 일

### MapData.cs 분리

Unity ScriptableObject가 정상적인 MonoScript를 갖도록 다음 파일로 분리한다.

```text
MapObjectClassifier.cs
- MapObjectType enum
- MapObjectClassifier static class

MapDataEntry.cs
- MapDataEntry ScriptableObject

MapDatabase.cs
- MapDatabase ScriptableObject
```

파일명과 public ScriptableObject 클래스명을 반드시 일치시킨다.

### MapDatabase 방어 코드

`GetMap()`은 다음 상황에서 예외 대신 null과 오류 로그를 반환한다.

- maps 리스트가 null인 경우
- mapName이 비어 있는 경우
- 리스트에 null 항목이 있는 경우
- 같은 mapName이 중복된 경우
- 요청한 맵을 찾지 못한 경우

현재 맵 수는 작으므로 Dictionary나 별도 캐시를 추가하지 않고 `List.Find`를 유지한다.

### MapManager 방어 코드

다음 순서로 `LoadMap()`을 수정한다.

1. MapDatabase 존재 여부 검사
2. MapDataEntry 존재 여부 검사
3. prefab 존재 여부 검사
4. prefab에 MapLoader가 있는지 검사
5. 새 맵 생성
6. SpawnPoint 확인
7. 새 맵이 정상일 때 기존 맵 제거
8. 플레이어 이동 및 현재 맵 참조 교체

`MapManager.Instance`는 중복 인스턴스가 생기면 오류를 출력하고 중복 오브젝트를 제거한다.

### MapLoader 스폰 처리

- 스폰 이름을 정확히 찾지 못하면 첫 위치로 조용히 이동하지 않는다.
- 찾지 못한 스폰 이름과 현재 맵 이름을 오류로 출력한다.
- 0단계에서는 데이터 이름을 `PlayerLocation_01` 같은 전체 이름으로 통일한다.
- 이름 접두사를 자동으로 붙이는 보정 코드는 추가하지 않는다.

## 7.2 Unity Editor에서 할 일

1. 컴파일이 끝난 뒤 기존의 손상된 `MapDatabase.asset`을 삭제한다.
2. 기존 TestMap 데이터의 프리팹 참조와 mapName을 메모한다.
3. 정상 `MapDataEntry` 메뉴로 TestMap01, TestMap02, TestMap03 에셋을 다시 만든다.
4. 각 MapDataEntry의 mapName과 prefab을 지정한다.
5. 정상 `MapDatabase` 에셋을 새로 만들고 TestMap01~03을 등록한다.
6. 비어 있는 TestMap04는 0단계 DB에 등록하지 않는다. 사용할 계획이 없으면 에셋을 제거한다.
7. GameplayScene의 MapManager에 새 MapDatabase와 시작 MapDataEntry를 지정한다.
8. 모든 MapTrigger의 targetMapName이 새 mapName과 정확히 일치하는지 확인한다.
9. 모든 targetPlayerLocationName을 `PlayerLocation_01` 형식의 실제 오브젝트 이름으로 수정한다.
10. 각 맵 프리팹 루트에 MapLoader가 있는지 확인한다.
11. `MapLoading.unity` Player에 남아 있는 GUID `342a54a11959d8f47bdce79eef80a1b9`의 Missing Script를 제거한다.
12. TestMap04의 TestDisplay/TestTrigger는 잠긴 문 기능을 다시 작업할 때까지 Gameplay 흐름에서 제외한다.

## 7.3 완료 확인

- MapDatabase Inspector에 정상 스크립트와 `maps` 리스트가 표시된다.
- MapDataEntry Inspector에 `mapName`과 `prefab` 필드가 표시된다.
- 시작 맵이 한 번만 생성된다.
- TestMap01, TestMap02, TestMap03 사이를 모두 이동할 수 있다.
- 각 입구에서 지정한 PlayerLocation으로 플레이어가 이동한다.
- 잘못된 맵 이름이나 스폰 이름은 Console 오류로 확인할 수 있고 기존 맵은 유지된다.

---

# 8. GameplayScene과 Build Settings 복구

## 8.1 C#에서 할 일

### GameStartUI

고정 인덱스 로딩을 제거한다.

```csharp
SceneManager.LoadScene("GameplayScene");
```

씬 이름이 빌드 목록에 없는 경우 오류를 출력하도록 `Application.CanStreamedLevelBeLoaded()`로 검사한다.

### GameOverUI

비어 있는 `Restart()`를 구현한다.

```csharp
Time.timeScale = 1f;
SceneManager.LoadScene("GameplayScene");
```

### GameStopUI

비어 있는 `Restart()`에도 같은 재시작 동작을 구현한다. `BackTitle()`은 `TitleScene`을 이름으로 로드한다.

씬 로딩을 위한 별도 SceneManager 래퍼, 인터페이스, 서비스 클래스는 만들지 않는다.

## 8.2 Unity Editor에서 할 일

1. `MapLoading.unity`를 Project 창에서 `GameplayScene.unity`로 이름 변경해 `.meta` GUID를 보존한다.
2. GameplayScene에 다음 오브젝트가 하나씩만 존재하는지 확인한다.
   - Main Camera
   - Global Light 2D
   - MapManager
   - Player Prefab
   - 인게임 Canvas
3. MapManager의 MapDatabase, startMap, player 필드를 연결한다.
4. Player에는 `Player` 태그를 지정한다.
5. `File > Build Profiles` 또는 `Build Settings`의 Scene List를 다음 순서로 구성한다.

```text
0: Assets/Scenes/TitleScene.unity
1: Assets/Scenes/GameplayScene.unity
```

6. `SampleScene`, `InGameUIScene`, `save.unity`는 0단계 빌드 목록에서 제외한다.
7. TitleScene의 시작 버튼에 GameStartUI가 연결되어 있는지 확인한다.
8. GameplayScene의 재시작 및 타이틀 버튼 참조가 비어 있지 않은지 확인한다.

## 8.3 완료 확인

- 에디터 Play를 TitleScene에서 시작할 수 있다.
- 시작 버튼이 GameplayScene을 연다.
- GameplayScene에서 플레이어 이동과 시작 맵 로딩이 모두 동작한다.
- 재시작 버튼이 GameplayScene을 새로 시작한다.
- 타이틀 버튼이 TitleScene으로 돌아간다.
- 빌드 실행 시에도 같은 흐름으로 동작한다.

---

# 9. Missing Script 정리 절차

## 9.1 C#에서 할 일

없음.

한 번만 사용할 Missing Script 검색용 Editor 스크립트는 만들지 않는다. 알려진 씬과 프리팹을 직접 점검하는 편이 더 짧다.

## 9.2 Unity Editor에서 할 일

다음 순서로 씬과 프리팹을 열어 Inspector의 Missing Script를 확인한다.

```text
TitleScene
GameplayScene
Player Prefab
TestMap01 Prefab
TestMap02 Prefab
TestMap03 Prefab
InGame UI Prefab 또는 Canvas
```

Missing Script를 발견하면 다음 기준을 사용한다.

- 현재 존재하는 기능이면 올바른 스크립트를 다시 추가하고 필드를 재연결한다.
- 삭제된 테스트 기능이면 Missing 컴포넌트만 제거한다.
- 어떤 기능인지 불명확하면 즉시 삭제하지 않고 원래 브랜치의 파일과 GUID를 먼저 확인한다.

## 9.3 완료 확인

- Unity Console에 `The referenced script ... is missing` 메시지가 없다.
- 모든 MonoBehaviour Inspector에 정상 클래스명이 표시된다.
- 씬을 저장하고 Unity를 다시 열어도 참조가 유지된다.

---

# 10. 통합 테스트 체크리스트

## 10.1 에디터 테스트

- [ ] 프로젝트를 열었을 때 컴파일 오류가 없다.
- [ ] URP 2D 렌더링과 Global Light 2D가 정상이다.
- [ ] TitleScene 시작 버튼이 GameplayScene을 연다.
- [ ] 플레이어가 이동한다.
- [ ] F키 입력으로 예외가 발생하지 않는다.
- [ ] 시작 맵이 정상 생성된다.
- [ ] 모든 테스트 맵 전환이 동작한다.
- [ ] 스폰 위치가 맵 입구와 일치한다.
- [ ] 재시작과 타이틀 복귀가 동작한다.
- [ ] Missing Script 경고가 없다.
- [ ] NullReferenceException이 없다.

## 10.2 빌드 테스트

- [ ] 새 빌드를 실행하면 TitleScene이 먼저 열린다.
- [ ] 시작 버튼으로 GameplayScene에 진입한다.
- [ ] 에디터와 동일하게 플레이어와 맵 전환이 동작한다.
- [ ] 종료 후 다시 실행해도 씬 참조 오류가 없다.

---

# 11. 예상 변경 파일

## C# 파일

```text
Assets/PlayerScript/PlayerInputReader.cs
Assets/PlayerScript/PlayerMove.cs
Assets/Scripts/MapObjectClassifier.cs
Assets/Scripts/MapDataEntry.cs
Assets/Scripts/MapDatabase.cs
Assets/Scripts/MapManager.cs
Assets/Scripts/MapLoader.cs
Assets/UI/MainMenu/Script/GameStartUI.cs
Assets/UI/InGame/Script/GameOverUI.cs
Assets/UI/InGame/Script/GameStopUI.cs
```

## Unity 에셋 및 설정

```text
Assets/Settings/UniversalRP.asset
Assets/Settings/Renderer2D.asset
Assets/MapDatas/*.asset
Assets/Scenes/GameplayScene.unity
Assets/Scenes/TitleScene.unity
ProjectSettings/QualitySettings.asset
ProjectSettings/EditorBuildSettings.asset
```

---

# 12. 0단계 종료 조건

다음 명령형 시나리오가 에디터와 빌드에서 모두 성공하면 프로젝트 복구를 완료한 것으로 본다.

```text
TitleScene 실행
-> 시작 버튼 클릭
-> GameplayScene 진입
-> 플레이어 이동
-> 시작 맵 생성
-> 맵 출구 진입
-> 지정 스폰 위치로 다음 맵 이동
-> 재시작
-> 타이틀 복귀
```

이 시나리오가 완료되기 전에는 학생 구조, 상점, 추가 스테이지 기믹 작업을 시작하지 않는다.
