
namespace MCProtocolEnums
{

    public enum EnumPLCDeviceType
    {
      D = 0xA8
      , W = 0xB4

      , M = 0x90
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
      , SD = 0xA9
      , R = 0xAF
      , ZR = 0xB0
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



    /*
     *  Very tiny set of error codes...
        Code    Description
        00 00	Success
        C0 00	Command Format Error
        C1 00	Unsupported Command
        C2 00	Requested Device Not Present
        C3 00	Data Size Error
        C4 00	Addressing Range Error
        C5 00	Accessing Forbidden Device Error
    */
    public enum EnumEndCode
    {
        Success = 0x0000,
        GeneralError = 0x0001,

        CommandTypeError = 0x00C0,
        CommandNotSupported = 0x00C1,
        DeviceNotFound = 0x00C2,
        DataSizeError = 0x00C3,
        OutOfAddressingRange = 0x00C4,
        ForbiddenReadingRange = 0x00C5,
    }



    public enum EnumMC3ERequestCommand
    {
        Read = 0x0401,
        Write = 0x1401
    }



    public enum MC3ERequestFrameByteIndex
    {
        ProtocolType = 0, // 0x0050 -- MC3E Request
        NetworkNo = 2,
        PCNo = 3,
        IONo = 4,
        ChNo = 6,
        DataLength = 7,
        CPUTimer = 9,
        MainCommand = 11,
        SubCommand = 13,
        Address = 15,
        DeviceType = 18,
        WordCount = 19,
        Values = 21,
    }



    public enum MC3EResponseFrameByteIndex
    {
        ProtocolType = 0,   // 0xD0 -- MC3E Response
        NetworkNo = 2,      // the value recvd from client
        PCNo = 3,           // the value recvd from client
        IONo = 4,           // the value recvd from client
        ChNo = 6,           // the value recvd from client
        DataLength = 7,     // 2 on write request, 2 + n for read request
        EndCode = 9,        // return code to client
        Values = 11,        // the value read from memory to send to the client starts from here
    }

}
