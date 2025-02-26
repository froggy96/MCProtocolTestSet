namespace MCProtocolPLCEmulator
{
    /*
        완료 코드	설명
        00 00	정상 완료
        C0 00	명령어 형식 오류
        C1 00	지원되지 않는 명령
        C2 00	지정된 장치가 존재하지 않음
        C3 00	데이터 크기 오류
        C4 00	주소 범위 초과
        C5 00	읽기 금지된 영역 접근
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
}
