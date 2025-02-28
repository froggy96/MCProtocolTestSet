using MCProtocolClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCProtocol
{

    public enum McFrame
    {
        MC3E    // Q, L, FX5U 시리즈 등 (기존에 많이 사용된 PLC)
        , MC4E  // iQ-R, iQ-F, Q 시리즈 일부 (새로 나온 PLC 에서 대량의 데이터를 다루기 위한 메시지 프레임)
    }

    // PLC Device의 종류를 정의한 열거형
    public enum PlcDeviceType
    {
        M = 0x90
      , SM = 0x91
      , L = 0x92
      , F = 0x93
      , V = 0x94
      , S = 0x98
      , X = 0x9C
      , Y = 0x9D
      , B = 0xA0
      , SB = 0xA1
      , DX = 0xA2
      , DY = 0xA3
      , D = 0xA8
      , SD = 0xA9
      , R = 0xAF
      , ZR = 0xB0
      , W = 0xB4
      , SW = 0xB5
      , TC = 0xC0
      , TS = 0xC1
      , TN = 0xC2
      , CC = 0xC3
      , CS = 0xC4
      , CN = 0xC5
      , SC = 0xC6
      , SS = 0xC7
      , SN = 0xC8
      , Z = 0xCC
      , TT
      , TM
      , CT
      , CM
      , A
      , Max
    }

    // PLC와 연결하기 위한 공통 인터페이스를 정의한다.
    public interface PLC : IDisposable
    {
        int Open();
        int Close();
        int SetBitDevice(string iDeviceName, int iSize, int[] iData);
        int SetBitDevice(PlcDeviceType iType, int iAddress, int iSize, int[] iData);
        int GetBitDevice(string iDeviceName, int iSize, int[] oData);
        int GetBitDevice(PlcDeviceType iType, int iAddress, int iSize, int[] oData);
        int WriteDeviceBlock(string iDeviceName, int iSize, int[] iData);
        int WriteDeviceBlock(PlcDeviceType iType, int iAddress, int iSize, int[] iData);
        int ReadDeviceBlock(string iDeviceName, int iSize, int[] oData);
        int ReadDeviceBlock(PlcDeviceType iType, int iAddress, int iSize, int[] oData);
        int SetDevice(string iDeviceName, int iData);
        int SetDevice(PlcDeviceType iType, int iAddress, int iData);
        int GetDevice(string iDeviceName, out int oData);
        int GetDevice(PlcDeviceType iType, int iAddress, out int oData);
    }

    abstract public class McProtocolApp : PLC
    {

        public McFrame CommandFrame { get; set; }

        public string HostName { get; set; }

        public int PortNumber { get; set; }

        protected McProtocolApp(string iHostName, int iPortNumber)
        {
            CommandFrame = McFrame.MC3E; // MC4E 쓰려면, 해당 처리 기능을 추가로 개발해야 한다...
            HostName = iHostName;
            PortNumber = iPortNumber;
        }

        public void Dispose()
        {
            Close();
        }

        public int Open()
        {
            DoConnect();
            Command = new McCommand(CommandFrame);
            return 0;
        }

        public int Close()
        {
            DoDisconnect();
            return 0;
        }

        public int SetBitDevice(string iDeviceName, int iSize, int[] iData)
        {
            PlcDeviceType type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return SetBitDevice(type, addr, iSize, iData);
        }

        public int SetBitDevice(PlcDeviceType iType, int iAddress, int iSize, int[] iData)
        {
            var type = iType;
            var addr = iAddress;
            var data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
            var d = (byte)iData[0];
            var i = 0;
            while (i < iData.Length)
            {
                if (i % 2 == 0)
                {
                    d = (byte)iData[i];
                    d <<= 4;
                }
                else
                {
                    d |= (byte)(iData[i] & 0x01);
                    data.Add(d);
                }
                ++i;
            }
            if (i % 2 != 0)
            {
                data.Add(d);
            }
            byte[] sdCommand = Command.SetCommand(0x1401, 0x0001, data.ToArray());
            byte[] rtResponse = TryExecution(sdCommand);
            int rtCode = Command.SetResponse(rtResponse);
            return rtCode;
        }

        public int GetBitDevice(string iDeviceName, int iSize, int[] oData)
        {
            PlcDeviceType type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return GetBitDevice(type, addr, iSize, oData);
        }

        public int GetBitDevice(PlcDeviceType iType, int iAddress, int iSize, int[] oData)
        {
            PlcDeviceType type = iType;
            int addr = iAddress;
            var data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
            byte[] sdCommand = Command.SetCommand(0x0401, 0x0001, data.ToArray());
            byte[] rtResponse = TryExecution(sdCommand);
            int rtCode = Command.SetResponse(rtResponse);
            byte[] rtData = Command.Response;
            for (int i = 0; i < iSize; ++i)
            {
                if (i % 2 == 0)
                {
                    oData[i] = (rtCode == 0) ? ((rtData[i / 2] >> 4) & 0x01) : 0;
                }
                else
                {
                    oData[i] = (rtCode == 0) ? (rtData[i / 2] & 0x01) : 0;
                }
            }
            return rtCode;
        }

        public int WriteDeviceBlock(string iDeviceName, int iSize, int[] iData)
        {
            PlcDeviceType type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return WriteDeviceBlock(type, addr, iSize, iData);
        }

        public int WriteDeviceBlock(PlcDeviceType iType, int iAddress, int iSize, int[] iData)
        {
            PlcDeviceType type = iType;
            int addr = iAddress;
            var data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
            foreach (int t in iData)
            {
                data.Add((byte)t);
                data.Add((byte)(t >> 8));
            }
            byte[] sdCommand = Command.SetCommand(0x1401, 0x0000, data.ToArray());
            byte[] rtResponse = TryExecution(sdCommand);
            int rtCode = Command.SetResponse(rtResponse);
            return rtCode;
        }

        public int ReadDeviceBlock(string iDeviceName, int iSize, int[] oData)
        {
            PlcDeviceType type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return ReadDeviceBlock(type, addr, iSize, oData);
        }

        public int ReadDeviceBlock(PlcDeviceType iType, int iAddress, int iSize, int[] oData)
        {
            PlcDeviceType type = iType;
            int addr = iAddress;
            var data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
            byte[] sdCommand = Command.SetCommand(0x0401, 0x0000, data.ToArray());
            byte[] rtResponse = TryExecution(sdCommand);
            int rtCode = Command.SetResponse(rtResponse);
            byte[] rtData = Command.Response;
            for (int i = 0; i < iSize; ++i)
            {
                oData[i] = (rtCode == 0) ? BitConverter.ToInt16(rtData, i * 2) : 0;
            }
            return rtCode;
        }

        public int SetDevice(string iDeviceName, int iData)
        {
            PlcDeviceType type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return SetDevice(type, addr, iData);
        }

        public int SetDevice(PlcDeviceType iType, int iAddress, int iData)
        {
            PlcDeviceType type = iType;
            int addr = iAddress;
            var data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , 0x01
                      , 0x00
                      , (byte) iData
                      , (byte) (iData >> 8)
                    };
            byte[] sdCommand = Command.SetCommand(0x1401, 0x0000, data.ToArray());
            byte[] rtResponse = TryExecution(sdCommand);
            int rtCode = Command.SetResponse(rtResponse);
            return rtCode;
        }

        public int GetDevice(string iDeviceName, out int oData)
        {
            PlcDeviceType type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return GetDevice(type, addr, out oData);
        }

        public int GetDevice(PlcDeviceType iType, int iAddress, out int oData)
        {
            PlcDeviceType type = iType;
            int addr = iAddress;
            var data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , 0x01
                      , 0x00
                    };
            byte[] sdCommand = Command.SetCommand(0x0401, 0x0000, data.ToArray());
            byte[] rtResponse = TryExecution(sdCommand);
            int rtCode = Command.SetResponse(rtResponse);
            if (0 < rtCode)
            {
                oData = 0;
            }
            else
            {
                byte[] rtData = Command.Response;
                oData = BitConverter.ToInt16(rtData, 0);
            }
            return rtCode;
        }

        public PlcDeviceType GetDeviceType(string s)
        {
            return
                   (s.StartsWith("CC")) ? PlcDeviceType.CC :
                   (s.StartsWith("CM")) ? PlcDeviceType.CM :
                   (s.StartsWith("CN")) ? PlcDeviceType.CN :
                   (s.StartsWith("CS")) ? PlcDeviceType.CS :
                   (s.StartsWith("CT")) ? PlcDeviceType.CT :

                   (s.StartsWith("DX")) ? PlcDeviceType.DX :
                   (s.StartsWith("DY")) ? PlcDeviceType.DY :
                   (s.StartsWith("D")) ? PlcDeviceType.D :

                   (s.StartsWith("SB")) ? PlcDeviceType.SB :
                   (s.StartsWith("SC")) ? PlcDeviceType.SC :
                   (s.StartsWith("SD")) ? PlcDeviceType.SD :
                   (s.StartsWith("SM")) ? PlcDeviceType.SM :
                   (s.StartsWith("SN")) ? PlcDeviceType.SN :
                   (s.StartsWith("SS")) ? PlcDeviceType.SS :
                   (s.StartsWith("SW")) ? PlcDeviceType.SW :
                   (s.StartsWith("S")) ? PlcDeviceType.S :

                   (s.StartsWith("TC")) ? PlcDeviceType.TC :
                   (s.StartsWith("TM")) ? PlcDeviceType.TM :
                   (s.StartsWith("TN")) ? PlcDeviceType.TN :
                   (s.StartsWith("TS")) ? PlcDeviceType.TS :
                   (s.StartsWith("TT")) ? PlcDeviceType.TT :

                   (s.StartsWith("ZR")) ? PlcDeviceType.ZR :
                   (s.StartsWith("Z")) ? PlcDeviceType.Z :

                   (s.StartsWith("A")) ? PlcDeviceType.A :
                   (s.StartsWith("B")) ? PlcDeviceType.B :
                   (s.StartsWith("F")) ? PlcDeviceType.F :
                   (s.StartsWith("L")) ? PlcDeviceType.L :
                   (s.StartsWith("M")) ? PlcDeviceType.M :
                   (s.StartsWith("R")) ? PlcDeviceType.R :
                   (s.StartsWith("V")) ? PlcDeviceType.V :
                   (s.StartsWith("W")) ? PlcDeviceType.W :
                   (s.StartsWith("X")) ? PlcDeviceType.X :
                   (s.StartsWith("Y")) ? PlcDeviceType.Y :
                                 PlcDeviceType.Max;
        }

        public bool IsBitDevice(PlcDeviceType type)
        {
            return !((type == PlcDeviceType.D)
                  || (type == PlcDeviceType.SD)
                  || (type == PlcDeviceType.Z)
                  || (type == PlcDeviceType.ZR)
                  || (type == PlcDeviceType.R)
                  || (type == PlcDeviceType.W));
        }

        public bool IsHexDevice(PlcDeviceType type)
        {
            return (type == PlcDeviceType.X)
                || (type == PlcDeviceType.Y)
                || (type == PlcDeviceType.B)
                || (type == PlcDeviceType.W);
        }

        public void GetDeviceCode(string iDeviceName, out PlcDeviceType oType, out int oAddress)
        {
            string s = iDeviceName.ToUpper();
            string strAddress;

            // 첫 번째 문자 추출
            string strType = s.Substring(0, 1);
            switch (strType)
            {
                case "A":
                case "B":
                case "D":
                case "F":
                case "L":
                case "M":
                case "R":
                case "V":
                case "W":
                case "X":
                case "Y":
                    // 두 번째 문자 이후는 숫자여야 하므로 변환
                    strAddress = s.Substring(1);
                    break;

                case "Z":
                    // 한 글자를 더 추출
                    strType = s.Substring(0, 2);
                    // 파일 레지스터일 경우 : 2
                    // 인덱스 레지스터일 경우 : 1
                    strAddress = s.Substring(strType.Equals("ZR") ? 2 : 1);
                    break;

                case "C":
                    // 한 글자를 더 추출
                    strType = s.Substring(0, 2);
                    switch (strType)
                    {
                        case "CC":
                        case "CM":
                        case "CN":
                        case "CS":
                        case "CT":
                            strAddress = s.Substring(2);
                            break;
                        default:
                            throw new Exception("잘못된 형식입니다.");
                    }
                    break;

                case "S":
                    // 한 글자를 더 추출
                    strType = s.Substring(0, 2);
                    switch (strType)
                    {
                        case "SD":
                        case "SM":
                            strAddress = s.Substring(2);
                            break;
                        default:
                            throw new Exception("잘못된 형식입니다.");
                    }
                    break;

                case "T":
                    // 한 글자를 더 추출
                    strType = s.Substring(0, 2);
                    switch (strType)
                    {
                        case "TC":
                        case "TM":
                        case "TN":
                        case "TS":
                        case "TT":
                            strAddress = s.Substring(2);
                            break;
                        default:
                            throw new Exception("잘못된 형식입니다.");
                    }
                    break;

                default:
                    throw new Exception("잘못된 형식입니다.");
            }

            // 장치 타입 및 주소 변환
            oType = GetDeviceType(strType);
            oAddress = IsHexDevice(oType) ? Convert.ToInt32(strAddress, BlockSize) : Convert.ToInt32(strAddress);
        }

        abstract protected void DoConnect();
        abstract protected void DoDisconnect();
        abstract protected byte[] Execute(byte[] iCommand);

        private const int BlockSize = 0x0010;
        private McCommand Command { get; set; }

        private byte[] TryExecution(byte[] iCommand)
        {
            byte[] rtResponse;
            int tCount = 10;
            do
            {
                rtResponse = Execute(iCommand);
                --tCount;
                if (tCount < 0)
                {
                    throw new Exception("Failed To Get Correct Values From The PLC");
                }
            } while (Command.IsIncorrectResponse(rtResponse));
            return rtResponse;
        }

        class McCommand
        {
            private McFrame FrameType { get; set; }  // 프레임 종류
            private uint SerialNumber { get; set; }  // 시리얼 번호
            private uint NetwrokNumber { get; set; } // 네트워크 번호
            private uint PcNumber { get; set; }      // PC 번호
            private uint IoNumber { get; set; }      // 요청 대상 유닛 I/O 번호
            private uint ChannelNumber { get; set; } // 요청 대상 유닛 채널 번호
            private uint CpuTimer { get; set; }      // CPU 모니터링 타이머
            private int ResultCode { get; set; }     // 종료 코드
            public byte[] Response { get; private set; }    // 응답 데이터

            public McCommand(McFrame iFrame)
            {
                FrameType = iFrame;
                SerialNumber = 0x0001u; // --> MC4E 포맷을 쓸 때, 대량으로 읽기/쓰기 명령을 보내려면 일정 단위로 나눠줘야 하는데 그 때 명령-응답 패킷을 짝지어서 구분할 순서번호이다. 보통은 명령 한번에 읽기/쓰기 하니까 1 만 있으면 되겠지.
                NetwrokNumber = 0x0000u;
                PcNumber = 0x00FFu;
                IoNumber = 0x03FFu;
                ChannelNumber = 0x0000u;
                CpuTimer = 20u;  // --> 0 으로 설정하면 Read/Write 명령이 완료될 때까지 기다리는 것이고, 1~FFFF 사이의 값이면 그만큼만 기다린다. (1=250ms 이다, 따라서, 20 이면 5_000 ms 이다.
            }

            public byte[] SetCommand(uint iMainCommand, uint iSubCommand, byte[] iData)
            {
                var dataLength = (uint)(iData.Length + 6);
                var ret = new List<byte>(iData.Length + 20);
                uint frame = (FrameType == McFrame.MC3E) ? 0x0050u :
                             (FrameType == McFrame.MC4E) ? 0x0054u : 0x0000u;
                ret.Add((byte)frame);
                ret.Add((byte)(frame >> 8));
                if (FrameType == McFrame.MC4E)
                {
                    ret.Add((byte)SerialNumber);
                    ret.Add((byte)(SerialNumber >> 8));
                    ret.Add(0x00);
                    ret.Add(0x00);
                }
                ret.Add((byte)NetwrokNumber);
                ret.Add((byte)PcNumber);
                ret.Add((byte)IoNumber);
                ret.Add((byte)(IoNumber >> 8));
                ret.Add((byte)ChannelNumber);
                ret.Add((byte)dataLength);
                ret.Add((byte)(dataLength >> 8));
                ret.Add((byte)CpuTimer);
                ret.Add((byte)(CpuTimer >> 8));
                ret.Add((byte)iMainCommand);
                ret.Add((byte)(iMainCommand >> 8));
                ret.Add((byte)iSubCommand);
                ret.Add((byte)(iSubCommand >> 8));
                ret.AddRange(iData);
                return ret.ToArray();
            }

            public int SetResponse(byte[] iResponse)
            {
                int min = (FrameType == McFrame.MC3E) ? 11 : 15;
                if (min <= iResponse.Length)
                {
                    var btCount = new[] { iResponse[min - 4], iResponse[min - 3] };
                    var btCode = new[] { iResponse[min - 2], iResponse[min - 1] };
                    int rsCount = BitConverter.ToUInt16(btCount, 0);
                    ResultCode = BitConverter.ToUInt16(btCode, 0);
                    Response = new byte[rsCount - 2];
                    Buffer.BlockCopy(iResponse, min, Response, 0, Response.Length);
                }
                return ResultCode;
            }

            public bool IsIncorrectResponse(byte[] iResponse)
            {
                var min = (FrameType == McFrame.MC3E) ? 11 : 15;
                var btCount = new[] { iResponse[min - 4], iResponse[min - 3] };
                var btCode = new[] { iResponse[min - 2], iResponse[min - 1] };
                var rsCount = BitConverter.ToUInt16(btCount, 0) - 2;
                var rsCode = BitConverter.ToUInt16(btCode, 0);
                return (rsCode == 0 && rsCount != (iResponse.Length - min));
            }
        }
    }

}
