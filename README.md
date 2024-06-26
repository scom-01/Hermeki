# Portfolio_Hermeki
## 게임명: Hermeki   
>장르 : 2D 어드밴처   
>플랫폼 : Mobile   
>제작의도 : 앙빅류 어드밴처 게임. 레벨 디자인에 중점을 두고 개발   

<br /><br />

## 플로우차트

```mermaid
 flowchart TB

플레이팹{플레이팹로그인};
구글로그인{구글로그인};
타이틀씬;
구글로그인-.->|Y|플레이팹;
구글로그인-.->|N|게스트로그인;
게스트로그인-.-플레이팹;
플레이팹-.->|Y|타이틀씬;
플레이팹-.->|N|종료;

타이틀씬-->|스테이지 설정|인게임씬;
타이틀씬-.->|종료버튼|종료;
인게임씬-.-인게임;
subgraph 인게임
direction LR
  subgraph 스테이지캔버스
  direction LR
  캐릭터설정([캐릭터 외형 설정 UI]);
  게임시작([게임시작 UI]);
  캐릭터선택([캐릭터 선택창 UI]);
  캐릭터선택-.-레벨선택([난이도 선택]);  
end
게임시작-.-스테이지1;
스테이지1-->스테이지2-->스테이지3-->|보스룸|스테이지4-.->|스테이지 Canvas 열기|스테이지캔버스;
end
인게임-.->|뒤로가기 버튼|타이틀씬;
```
* * *
<br/><br/>

## 적용 기술   

### 어드레서블
>Resources는 패키지 내에 포함이 되어 빌드파일 크기가 증가하고 Resources폴더 내에 들어있는 리소스파일들은 사용 여부와 상관없이 패키징되기에 용량에 낭비가 크다.   
>또한 Resources파일이 클 수록 애플리케이션이 처음 구동될 때 지연시간이 증가한다.(Resources폴더를 순회하면서 Lookup테이블을 생성하여 필요한 파일들을 찾는 과정에 비용이 많이 소모된다.)   
>Resources 폴더내에서 파일을 찾는 과정도 비용 소모가 크다.
>위의 이유로 AssetBundle을 사용하고 관리 툴인 Addressable을 사용한다.
><br/><br/>
>![image](https://github.com/scom-01/Hermeki/assets/78716085/ab7eb0c9-19a0-40ef-b585-8730f2f3c796)   
>Amazon S3 서버에서 객체를 다운로드 받는 모습   
>애플리케이션 실행 시 업데이트 파일을 체크 후 다운로드

### 스프라이트 아틀라스 에셋 사용   
>드로우콜을 줄이기 위해 단일 텍스쳐를 호출함으로 써 하나의 드로우콜로 큰 성능 소모없이 패킹된 텍스쳐를 동시에 액세스 할 수 있음.   

### A* Path   
>A* 알고리즘으로 AI 길찾기 구현.

### PostProcessing
>포스트 프로세싱을 사용하여 레벨 디자인 및 연출

### 구글 플레이 로그인
>GPGS 로그인 기능

### PlayFab 로그인
>구글플레이 유저 정보로 로그인   
>PlayFab으로 유저 관리 및 데이터 분석   

### 옵저버 패턴   
>대상의 상태에 변화가 있을 때마다 옵저버들에게 통지하고, 옵저버는 알림을 받아 조치를 취하는 패턴으로 스테이지의 변경, 레벨의 변경 등에 따라 옵저버들이 변화하도록 구현하였습니다.

## 문제 해결
>> Error
>> ### Addressable
>>1.Addressable PlayMode 를  "Use Existing Build" 빌드 모드를 사용했을 때 스프라이트에 일반 텍스처가 누락되는 문제   
>>-> 1.21.17 버전에서 수정됐다고 Doc에서 확인   
>>->하지만 현재 프로젝트는 1.19.19버전이 최대   
>>->프로젝트 종료 후 Packages/manifest.json 에서  "com.unity.addressables": "1.19.19"->"com.unity.addressables": "1.21.19"로 수정   
>>->프로젝트 실행 후 PackageManager에서 Addressable 업데이트
>><br/><br/>
>>2.UnityEngine.AddressableAssets.InvalidKeyException: Exception of type 'UnityEngine.AddressableAssets.InvalidKeyException' was thrown. No Location found for Key=default
>>  Addressables.GetDownloadSizeAsync(label); 이런 식으로 Label이름으로 가져올 때 해당 Label로 설정된 Asset이 하나도 없으면 Exception을 일으킨다.
>><br/><br/>
>>3.SpriteAtlasV1 Enable for Build 사용 시 SpriteAtlas 패키징된 Sprite가 흰색으로 나타나는 오류
>> ->Always Enabled로 수정
>> ### 구글 스토어 내부테스트
>> 1. 프로젝트 설정
>>> Scpriting Backend Mono -> IL2CPP로 수정
>>> 2019년 8월 1일부터 64비트를 지원해야하기에 Target Architectures ARM64 체크
>>> TargetAPI 30이상으로 수정
>>2. 앱 서명
>>> java keystore의 키 내보내기 및 업로드
>>> 자바 버전이 맞지않음 -> jdk 버전 업데이트 -> OpenJdk 버전으로 수정 -> 환경변수 설정 -> 생성된 zip 업로드
