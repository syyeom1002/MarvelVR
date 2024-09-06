# [MarvelVR Game Portfolio]


## 1. 프로젝트 소개

<div align="center">

  <img src="https://github.com/user-attachments/assets/bbf87c18-8bb5-4f78-8f3b-89815df475c1" width="49%" height="280"/>
  <img src="https://github.com/user-attachments/assets/a0516eea-442e-41ec-bed1-6819eab7db4c" width="49%" height="280"/>
  <img src="https://github.com/user-attachments/assets/0a0f3334-245d-4794-94d4-1f2665892f00" width="49%" height="280"/>
  <img src="https://github.com/user-attachments/assets/cdb9eb56-9511-4ea5-b8d5-fc3420dbd165" width="49%" height="280"/>

  < 게임 플레이 사진 >

</div>

> + 장르 : 1인칭 VR 슈팅게임
>   
> + K- 디지털 과정 수업을 들으며 진행한 프로젝트로, 사용자가 캡틴아메리카가 되어 사용자의 영웅 심리를 자극하는 게임입니다.
> 방패를 던지거나 방패로 바닥을 찍어 적을 공격할 수 있습니다.  
> 모든 적을 물리치고 스페이스 스톤을 획득하면 스테이지 클리어가 되고 다음 스테이지가 열립니다.
> 
> + 개발기간: 2023.11.01 ~ 2024.01.15 ( 약 2개월 )
> + 로드맵
> <img src="https://github.com/user-attachments/assets/10d0871b-5dd1-48fc-880e-e0973c026a50" width="50%" height="140"/>

<br>

## 2. 개발 환경

+ 개발 엔진 : Unity 2022.3.5f1 

+ 언어 : C#

+ 플랫폼 : oculus

+ 형상 관리: SVN

<br>

## 3. 사용 기술
| 기술 | 설명 |
|:---:|:---|
| Oculus Interaction | Grab, OVRInput, Haptics, Hand Grab Pose, Ray |
| Queue | 미션 UI의 팔로우 로직 및 방패 찍기 구현 |
| Curved UI | 캐릭터 선택 ScrollView UI 구현 |
| 라이트맵| 성능의 최적화를 위해 라이트맵을 이용하여 게임 맵 구성 |
| 내적(Dot) | 방패 던지기 방향 구현 |
| OverlapSphere | 반경 안에 있는 적 감지 |
| Dotween | 방패 회전 구현 |
| Event Wrapper | 스페이스 스톤 획득 시 씬 전환 이벤트 처리 |

<br>

## 4. 개발 인원 및 담당

+ 인원 : 총 2인 (프로그래머 2인)
+ 담당 : 방패 던지기 및 막기, 캐릭터 선택 UI 제작, 공중 적 생성 및 공격 패턴 , 모듈 통합 , Mission UI 팔로우 로직, 엔딩 , 사운드

<br>

## 5. 핵심 기능


#### 캐릭터 선택
<img src="https://github.com/user-attachments/assets/11cc95ff-1e0a-41db-99a3-302b18869e95" width="49%" height="280"/>

+ Controller의 Thumbstick 으로 좌우 이동
+ Controller의 A버튼 클릭하면 캐릭터 선택 완료, 게임씬으로 전환


#### 이동 시스템
<div align="center">

  <img src="https://github.com/user-attachments/assets/0c9152d7-3a7b-4341-8474-9505adc12a4f" width="49%" height="280"/>
  <img src="https://github.com/user-attachments/assets/2dad20b5-6b8c-481f-a554-48394df6d80d" width="49%" height="280"/>

</div>

+ 자동 이동
+ Leap : Contoller의 A 버튼을 이용하여 이동 가능


#### 공격
<div align="center">

  <img src="https://github.com/user-attachments/assets/e561759c-4568-4fc2-9fe4-ba2b972b0adb" width="49%" height="280"/>
  <img src="https://github.com/user-attachments/assets/a9fb460c-7c83-4f43-928b-f6cc181c6bed" width="49%" height="280"/>

</div>

+ 방패 던지기 : Controller의 HandTrigger버튼으로 조작 가능
+ 방패 찍기 : Controller의 IndexTrigger 버튼으로 조작 가능


#### 적
<div align="center">

  <img src="https://github.com/user-attachments/assets/681a87de-7626-4a73-904e-093a1984104b" width="49%" height="280"/>
  <img src="https://github.com/user-attachments/assets/e2b303b9-a74e-4939-9d5d-6e0f50b46941" width="49%" height="280"/>

</div>

+ 공중적- 레이저 공격
+ 지상적- 총알 공격 , 순간 빠른 속도로 이동


#### 엔딩
<img src="https://github.com/user-attachments/assets/04ec79d1-8c07-4b55-b1f4-5a899eb42117" width="49%" height="280"/>

+ 모든 적을 물리치면 생성
+ 스페이스 스톤 획득 시 씬 전환, 다음 스테이지 잠금 해제 

<br>

## 6. 기술 문서( 담당한 핵심기능 코드 포함)
+ https://docs.google.com/presentation/d/1T0axGK3wXZ9rHkEpkYmO2pyhAodyl-_pG6aENf-RMZU/edit?hl=ko#slide=id.g2f0c2aa774c_0_13

## 7. 티스토리 개발일지
+ https://syyeom1002.tistory.com/category/%EB%A7%88%EB%B8%94%20VR%20%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8%20%EC%A0%9C%EC%9E%91


## 8. 플레이 영상
+ https://www.youtube.com/watch?v=FLcZElzEC0A&t
+ 타이틀씬부터 다음 스테이지 잠금 해제 부분까지 전체적인 구현을 보실 수 있습니다.

---

**담당한 부분(작성한 스크립트)**

****SYS 폴더에 모아놨습니다.***


1.AerialEnemy(공중적)

2.Laser

3.Enemy

4.TutorialAerialEnemy

5.LastEnemyMove

6.Shield

7.CharacterButton

8.SelectCharaterMain

9.MyOVRScreenFade(oculus 제공 scripts 변형)

10.GameSceneMain

11.CubeGenerator

12.LightMove

13.MeleeAttackUI( EventDispather 부분)

14.MissionUI

15.SpaceStone
