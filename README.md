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


## 2. 개발 환경

+ 개발 엔진 : Unity 2022.3.5f1 

+ 언어 : C#

+ 플랫폼 : oculus

+ 형상 관리: SVN


## 3. 사용 기술
| 기술 | 설명 |
|:---:|:---|
| 디자인 패턴 | ● **싱글톤** 패턴을 사용하여 Manager 관리 <br> ● **State** 패턴을 사용하여 캐릭터의 기능을 직관적으로 관리 |
| Queue | 미션 UI의 팔로우 로직 및 방패 찍기 구현 |
| Curved UI | 구글 스프레드 시트를 사용해 데이터 관리 |
| 라이트맵| 성능의 최적화를 위해 라이트맵을 이용하여 게임 맵 구성 |
| Raycasting | 자주 사용되는 객체는 Pool 관리하여 재사용 |
| OverlapSphere | 반경 안에 있는 적 감지 |
| Dotween | ㅈ |


## 4. 핵심 기능


## 5. 개발 인원 및 담당

+총 2인 


## 6. 기술 문서( 담당한 기능 코드리뷰 포함)


## 7. 티스토리 개발일지


## 8. 플레이 영상

---

**담당한 부분(작성한 스크립트)**

1.AerialEnemy(공중적)

2.Laser

3.Enemy

4.TutorialAerialEnemy

5.LastEnemyMove

6.Shield

7.CharacterButton

8.SelectCharaterMain

9.MyOVRScreenFade(oculus 제공 scripts 변형)

10.TitleSceneMain

11.CubeGenerator

12.LightMove

13.MeleeAttackUI( EventDispather 부분)

14.MissionUI

15.SpaceStone
