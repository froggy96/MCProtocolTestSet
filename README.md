# MCProtocolTestSet

Mitsubishi MELSEC PLC의 MC Protocol 통신을 PC에서 시험할 수 있는 Windows용 테스트 솔루션입니다. PLC 에뮬레이터가 TCP 서버로 동작하고, 테스트 클라이언트가 워드 데이터를 읽고 씁니다. 실물 PLC 없이 클라이언트 개발과 프레임 검증에 사용할 수 있습니다.

## 지원 기능

| 항목 | 지원 범위 |
| --- | --- |
| 통신 | TCP, 바이너리 MC 3E 및 MC 4E |
| 프레임 선택 | 클라이언트는 연결을 열 때 선택하며 기본값은 MC3E입니다. 서버는 같은 포트에서 요청마다 프레임을 판별합니다. |
| 워드 일괄 읽기 | 명령 `0x0401`, 서브커맨드 `0x0000` |
| 워드 일괄 쓰기 | 명령 `0x1401`, 서브커맨드 `0x0000` |
| 에뮬레이터 메모리 | D와 W 각각 20,000개의 16비트 워드. D 주소는 10진수 `D0`–`D19999`, W 주소는 16진수 `W0`–`W4E1F`입니다. |
| 클라이언트 화면 | 시작 주소와 워드 개수를 지정해 그리드에서 조회할 수 있습니다. 선택한 주소에 16진수, 10진수, 문자열, float, double 값을 쓸 수 있으며, float와 double에는 별도 읽기 버튼이 있습니다. |
| 서버 화면 | 메모리 그리드와 로컬 값 입력 기능을 제공하며, 클라이언트 연결 및 요청·응답 로그를 확인할 수 있습니다. |

에뮬레이터의 통신 범위는 D/W 워드 일괄 읽기·쓰기입니다. ASCII 프레임, UDP, 비트 단위 접근, 그 밖의 장치·명령은 이 범위에 포함되지 않습니다. 에뮬레이터의 메모리 크기는 테스트용 설정이며, 실물 PLC의 장치 범위와 통신 설정은 모델과 모듈에 따라 확인해야 합니다.

## 빌드와 실행

1. Windows에서 **Visual Studio 2022**의 **.NET 데스크톱 개발** 워크로드와 **.NET Framework 4.8 타기팅 팩**을 준비합니다.
2. `MCProtocolTestSet.sln`을 열고 NuGet 패키지를 복원한 뒤 솔루션을 빌드합니다. 서버는 `packages.config`를 통해 `SuperSimpleTcp 3.0.17`을 참조합니다.
3. `MCProtocolPLCEmulator`를 실행하고 **Launch**를 누릅니다. 로컬 테스트 기본 주소는 `127.0.0.1:8000`입니다.
4. `MCProtocolClient`를 실행하고 같은 주소와 포트를 입력합니다. **MC3E** 또는 **MC4E**를 선택한 뒤 **Open**을 누릅니다. 프레임을 바꿀 때는 **Close**로 연결을 닫고 다른 프레임을 선택한 뒤 다시 엽니다.
5. **Start Address**에 `D0000`을 입력하고 워드 개수를 `2`로 지정한 뒤 **Read**를 누릅니다. 그리드에서 첫 행을 선택하고 16진수 값에 `1234`를 입력해 **Write**를 누릅니다. 다시 **Read**를 눌러 같은 주소에 `1234`가 표시되는지 확인합니다. `W0000`과 다른 프레임 형식으로도 같은 절차를 반복할 수 있습니다.

Visual Studio 개발자 명령 프롬프트에서도 패키지를 복원하고 빌드할 수 있습니다.

```powershell
msbuild MCProtocolTestSet.sln /restore /p:RestorePackagesConfig=true /p:Configuration=Debug
```

## 솔루션 구성

| 프로젝트 | 역할과 주요 파일 |
| --- | --- |
| `MCProtocolPLCEmulator` | `MCProtocolServer.cs`가 TCP 수신 데이터를 프레임 단위로 분리합니다. `MCProtocolCommand.cs`가 요청 해석, 워드 명령 실행, 응답 직렬화를 담당합니다. `PLCWordMemory.cs`는 D/W 데이터를 저장하고 주소 범위를 검사합니다. |
| `MCProtocolClient` | `Mainform_TCPClient.cs`가 화면에서 선택한 프레임을 연결에 적용합니다. `MCProtocol.cs`가 요청을 생성하고 응답을 검증합니다. `McProtocolTcp.cs`가 TCP 송신과 프레임 길이 기반 수신을 담당합니다. |
| `CommonInterface` | `MCProtocolEnums.cs`에 에뮬레이터의 장치 코드, 명령, 종료 코드, 3E 기준 필드 위치를 정의합니다. |

## MC 4E 개발 내용

### 바이너리 프레임

| 항목 | MC 3E | MC 4E |
| --- | --- | --- |
| 요청 시작 바이트 | `50 00` | `54 00` |
| 응답 시작 바이트 | `D0 00` | `D4 00` |
| 시작 바이트 뒤의 추가 필드 | 없음 | 시리얼 번호 2바이트, 예약값 `00 00` |
| 데이터 길이 필드 오프셋 | 7 | 11 |
| 전체 프레임 크기 | `9 + 데이터 길이` | `13 + 데이터 길이` |

오프셋은 0부터 계산합니다. 데이터 길이는 2바이트 리틀엔디언 값입니다. 요청에서는 모니터링 타이머부터 요청 데이터 끝까지, 응답에서는 종료 코드부터 응답 데이터 끝까지의 바이트 수를 나타냅니다. 4E 시리얼 번호도 리틀엔디언이며, 서버는 요청에서 받은 번호를 응답에 그대로 넣습니다. [Mitsubishi Electric SLMP Reference Manual, §4](https://dl.mitsubishielectric.com/dl/fa/document/manual/plc/sh080956eng/sh080956engm.pdf)

다음은 시리얼 번호 `0x1234`로 `D0`에서 1워드를 읽어 값 `0x1234`를 받는 4E 예시입니다.

```text
요청  54 00 34 12 00 00  00 FF FF 03 00  0C 00  14 00 01 04 00 00 00 00 00 A8 01 00
응답  D4 00 34 12 00 00  00 FF FF 03 00  04 00  00 00 34 12
```

응답의 길이 `04 00`에는 정상 종료 코드 `00 00`과 읽은 워드 `34 12`가 포함됩니다.

### 서버 처리

- 클라이언트별로 수신 데이터를 누적합니다. 프레임이 여러 TCP 읽기에 걸쳐 도착하면 완성될 때까지 기다리고, 한 번에 여러 프레임이 들어오면 순서대로 처리합니다.
- 서브헤더에 따라 필드 위치를 계산하고, 선언 길이와 쓰기 데이터의 워드 개수를 확인한 뒤 기존 D/W 메모리 로직으로 실행합니다.
- 요청과 같은 프레임 형식으로 정상 또는 오류 응답을 보냅니다. 에뮬레이터의 종료 코드에는 장치 없음 `0x00C2`와 주소 범위 초과 `0x00C4`가 있습니다. 잘못된 서브헤더나 프레임 형식은 로그에 남기고 해당 TCP 연결을 종료합니다.

### 클라이언트 처리

- 4E 시리얼 번호는 연결 초기화 시 `0x0001`부터 시작해 요청마다 증가합니다. 요청을 한 건씩 처리하고 응답의 서브헤더, 시리얼 번호, 예약 바이트, 데이터 길이를 확인합니다.
- TCP 수신에서는 길이 필드에 지정된 만큼 읽습니다. 정상 응답은 명령에서 기대한 데이터 크기와도 비교합니다.
- 응답이 불완전하거나 검증에 실패하면 호출자에게 예외를 전달합니다. 쓰기 응답을 받지 못했을 때는 대상 값을 다시 읽어 실행 여부를 확인할 수 있습니다.

## 검증 결과

Visual Studio MSBuild로 솔루션의 Debug 빌드를 완료했고, 로컬 TCP에서 다음을 확인했습니다.

| 검증 항목 | 확인한 내용 |
| --- | --- |
| MC 3E 회귀 및 MC 4E 추가 | 두 프레임 모두 D/W에 두 워드를 쓰고 같은 값으로 읽었습니다. |
| 주소 범위 초과 | 두 프레임 모두 D 메모리 범위를 벗어난 요청에 `0x00C4`를 반환했습니다. |
| 서버 수신 경계 | 한 연결에서 분할된 4E 요청과 뒤따르는 3E 요청을 처리했고, 두 응답의 바이트가 기대값과 일치했습니다. |
| 클라이언트 응답 검증 | 나누어 전송된 4E 응답을 읽었고, 다음 요청에서 시리얼 번호가 증가했으며 번호가 다른 응답을 거부했습니다. |

검증은 로컬에서 서버와 클라이언트 구현을 호출하는 방식으로 수행했습니다. WinForms 화면 조작과 실물 PLC 연결은 검증 범위에 포함되지 않았습니다.

## 참고 자료

- Mitsubishi Electric의 MC Protocol 및 SLMP 사양. [MELSEC Communication Protocol Reference Manual](https://dl.mitsubishielectric.com/dl/fa/document/manual/plc/sh080008/sh080008ab.pdf) · [SLMP Reference Manual](https://dl.mitsubishielectric.com/dl/fa/document/manual/plc/sh080956eng/sh080956engm.pdf)
- 서버가 사용하는 TCP 라이브러리. [SuperSimpleTcp](https://github.com/jchristn/SuperSimpleTcp)
- 클라이언트의 MC Protocol 구현 참고 소스. [SecondShiftEngineer/McProtocol](https://github.com/SecondShiftEngineer/McProtocol/blob/master/McProtocol/MCProtocol.cs)
