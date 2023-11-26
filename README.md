# EventSystem

### 설치 필요
- Quartz (Scheduler)
- NUnit (Test)
- Moq (Test)

### 동적으로 이벤트를 추가하여 스케줄링 할 수 있는 모듈
#### 사용방법
- 리스너 등록
  - 이벤트를 수신할 클래스에 IListener 인터페이스를 상속하여 Filter로 이벤트 수신 필터 설정
  - GlobalEventManager.Instance.AddListner를 통해 리스너 등록.
- Command 등록
  - IGlobalEventCommand를 상속하여 이벤트의 행동양식을 정의하는 클래스를 작성합니다.
  - Command를 생성했으면, 나중에 사양서 작성시 이벤트의 행동을 지정할수 있도록 GlobalEventCommandType Enum을 추가합니다.
  - 작성된 Command는 따로 등록하지 않아도 GlobalEventCommandLoader에 의해 자동으로 주입됩니다.
- 이벤트 Spec 작성
  - StartTime, EndTime: 통해 이벤트의 시작과 끝을 스케줄링
  - Filter: 이벤트 발신 필터링
  - Params: 이벤트 발생시 전달할 파라미터
  - CommandType: 이벤트의 행동양식
- 이벤트 추가
  - GlobalEventManager.Instance.AddEvent를 통해 이벤트를 추가합니다.
 
※ 프로그램시작시 AssemblyTypeLoader.Load()를 실행시켜야 Command등록이 됩니다.
  
