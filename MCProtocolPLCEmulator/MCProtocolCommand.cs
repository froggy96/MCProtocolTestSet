using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCProtocolPLCEmulator
{
    public static class MCProtocolCommand
    {

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

        /*
            Code    Description
            00 00	Success
            C0 00	Command Format Error
            C1 00	Unsupported Command
            C2 00	Requested Device Not Present
            C3 00	Data Size Error
            C4 00	Addressing Range Error
            C5 00	Accessing Forbidden Device Error
        */

        #region [Request Command : PC -> PLC]

        public enum EnumMC3ERequestCommand
        {
            Read = 0x0401,
            Write = 0x1401
        }

        public enum MC3ERequestCommandByteIndex
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



        /// <summary>
        /// MC3E Type Request Command (PC --> PLC)
        /// </summary>
        public class MC3ERequestCommand
        {

            public ushort ProtocolType { get; set; } = 0x0050; // 0x0050 = MC3E, 0x0054 = MC4E

            public byte NetworkNo { get; set; } = 0x00;

            public byte PCNo { get; set; } = 0xFF;

            public ushort IONo { get; set; } = 0x03FF;

            public byte ChNo { get; set; } = 0x00;

            public ushort DataLength { get; set; } = 0x0000;

            public ushort CPUTimer { get; set; } = 0x0000;

            public ushort MainCommand { get; set; } = 0x0000; // 0x1401 : Write, 0x0401 : Read

            public ushort SubCommand { get; set; } = 0x0000;

            public uint Address { get; set; } = 0x00000000;

            public byte DeviceType { get; set; } = 0xA8; // A8: 'D', B4: 'W'

            public ushort WordCount { get; set; } = 0x0000;

            public ushort[] Values { get; set; } = null;


            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
                {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(this);

                    if (name == "Values" && value != null)
                    {
                        int index = 0;
                        foreach (var item in value as ushort[])
                        {
                            sb.AppendLine($"{name}[{index++}]={item}  [0x{item:X4}]");
                        }
                    }
                    else
                    {
                        sb.AppendLine($"{name}={value}  [0x{value:X4}]");
                    }
                }
                return sb.ToString();
            }
        }

        #endregion



        #region [Response Command : PLC -> PC]

        public enum MC3EResponseCommandByteIndex
        {
            ProtocolType = 0,   // 0xD0 -- MC3E Response
            NetworkNo = 2,      // 받은 거 그대로
            PCNo = 3,           // 받은 거 그대로
            IONo = 4,           // 받은 거 그대로
            ChNo = 6,           // 받은 거 그대로
            DataLength = 7,     // Write 인 경우, EndCode 만 이므로 2, Read 인 경우 읽어낸 Byte 수 + 2 (응답 코드)
            EndCode = 9,        // 응답코드
            Values = 11,        // Read 요청인 경우...여기서부터 Byte 로 쓴다.
        }

        /// <summary>
        /// MC3E Response Command (PLC --> PC)
        /// </summary>
        public class MC3EResponseMessage
        {
            public ushort ProtocolType { get; set; } = 0x00D0; // 0x00D0 = MC3E, 0x00D0 = MC4E

            public byte NetworkNo { get; set; } = 0x00; // 받은거 그대로

            public byte PCNo { get; set; } = 0x00; // 받은거 그대로

            public ushort IONo { get; set; } = 0x0000; // 받은거 그대로

            public byte ChNo { get; set; } = 0x00; // 받은거 그대로

            public ushort DataLength { get; set; } = 0x0002; // 최소 2 bytes (EndCode)

            public ushort EndCode { get; set; } = 0x0000; // 응답코드

            public byte[] Values { get; set; } = null;

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
                {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(this);

                    if (name == "Values" && value != null)
                    {
                        int index = 0;
                        foreach (var item in value as byte[])
                        {
                            sb.AppendLine($"{name}[{index++}]={item}  [0x{item:X2}]");
                        }
                    }
                    else
                    {
                        sb.AppendLine($"{name}={value}  [0x{value:X4}]");
                    }
                }
                return sb.ToString();
            }

        }

        #endregion


        // MC3E Request Packet Byte Length (Min size)
        public const int MIN_REQ_CMD_LEN = 21;

        // MC3E Response Packet Byte Length (Min size)
        public const int MIN_RES_CMD_LEN = 11;

        // MC3E/4E Common Response Header 0xD0 (same)
        public const int RESPONSE_HEADER_SIGN = 0x00D0; //

        public static EnumEndCode ExecuteRequest(MC3ERequestCommand req, PLCWordMemory plc)
        {
            if (req.MainCommand == (ushort)EnumMC3ERequestCommand.Write)
            {
                return plc.Write((int)req.Address, req.WordCount, req.Values);
            }
            else if (req.MainCommand == (ushort)EnumMC3ERequestCommand.Read)
            {
                var end_code = plc.Read((int)req.Address, req.WordCount, out ushort[] iData);
                req.Values = iData;
                return end_code;
            }
            else
            {
                return EnumEndCode.CommandNotSupported;
            }
        }

        public static MC3EResponseMessage SetResponseMessage(MC3ERequestCommand req, ushort end_code, ushort[] iData)
        {
            var cmd = new MC3EResponseMessage();

            cmd.ProtocolType = RESPONSE_HEADER_SIGN;

            // below 4 items return the values as received
            cmd.NetworkNo = req.NetworkNo;
            cmd.PCNo = req.PCNo;
            cmd.IONo = req.IONo;
            cmd.ChNo = req.ChNo;

            // at least 2 bytes
            cmd.DataLength = 2;
            //

            // set end code
            cmd.EndCode = end_code;

            //
            if (req.MainCommand == (ushort)EnumMC3ERequestCommand.Read && iData != null)
            {
                var data = new List<byte>();
                foreach (int t in iData)
                {
                    data.Add((byte)t);
                    data.Add((byte)(t >> 8));
                }
                cmd.Values = data.ToArray();
                cmd.DataLength += (ushort)(iData == null ? 0 : iData.Length * 2);
            }
            //

            return cmd;
        }

        public static byte[] EncodeMC3EResponseMessage(MC3EResponseMessage res)
        {
            // now, encode cmd into byte array
            var ret = new List<byte>(MIN_RES_CMD_LEN + res.DataLength - 2);
            ret.Add((byte)res.ProtocolType);
            ret.Add((byte)(res.ProtocolType >> 8));


            // MC4E frame support...
            //if (FrameType == McFrame.MC4E)
            //{
            //    ret.Add((byte)SerialNumber);
            //    ret.Add((byte)(SerialNumber >> 8));
            //    ret.Add(0x00);
            //    ret.Add(0x00);
            //}

            ret.Add((byte)res.NetworkNo);
            ret.Add((byte)res.PCNo);
            ret.Add((byte)res.IONo);
            ret.Add((byte)(res.IONo >> 8));
            ret.Add((byte)res.ChNo);
            ret.Add((byte)res.DataLength);
            ret.Add((byte)(res.DataLength >> 8));
            ret.Add((byte)res.EndCode);
            ret.Add((byte)(res.EndCode >> 8));

            if (res.Values != null)
            {
                ret.AddRange(res.Values);
            }

            return ret.ToArray();
        }

        public static byte[] EncodeWordsIntoBytes(int[] iData)
        {
            if (iData == null) return null;

            //
            var data = new List<byte>();
            foreach (int t in iData)
            {
                data.Add((byte)t);
                data.Add((byte)(t >> 8));
            }
            return data.ToArray();
        }

        public static int[] EncodeStringIntoWords(string s)
        {
            if (s == null) return null;

            var data = new List<int>();
            for (int i = 0; i < s.Length; i += 2)
            {
                int word = s[i];
                if (i + 1 < s.Length)
                {
                    word += s[i + 1] << 8;
                }
                data.Add(word);
            }

            return data.ToArray();
        }



        public static MC3ERequestCommand DecodeMC3ERequestCommand(byte[] req)
        {

            if (req.Length < MIN_REQ_CMD_LEN)
            {
                throw new ArgumentException($"Command Lengh < {MIN_REQ_CMD_LEN}");
            }

            //
            var cmd = new MC3ERequestCommand();

            // get protocol type
            cmd.ProtocolType = BitConverter.ToUInt16(req, (int)MC3ERequestCommandByteIndex.ProtocolType);
            if (cmd.ProtocolType != 0x0050)
            {
                throw new ArgumentException($"ProtocolType != 0x0050");
            }


            // get network no, pc no, io no, ch no
            cmd.NetworkNo = req[(int)MC3ERequestCommandByteIndex.NetworkNo];
            cmd.PCNo = req[(int)MC3ERequestCommandByteIndex.PCNo];
            cmd.IONo = BitConverter.ToUInt16(req, (int)MC3ERequestCommandByteIndex.IONo);
            cmd.ChNo = req[(int)MC3ERequestCommandByteIndex.ChNo];

            // get data length
            cmd.DataLength = BitConverter.ToUInt16(req, (int)MC3ERequestCommandByteIndex.DataLength);

            // get cpu timer
            cmd.CPUTimer = BitConverter.ToUInt16(req, (int)MC3ERequestCommandByteIndex.CPUTimer);

            // get main/sub command
            cmd.MainCommand = BitConverter.ToUInt16(req, (int)MC3ERequestCommandByteIndex.MainCommand);
            cmd.SubCommand = BitConverter.ToUInt16(req, (int)MC3ERequestCommandByteIndex.SubCommand);

            // get address, devicetype
            cmd.Address = (uint)(req[(int)MC3ERequestCommandByteIndex.Address + 2] << 16);
            cmd.Address += (uint)(req[(int)MC3ERequestCommandByteIndex.Address + 1] << 8);
            cmd.Address += (uint)(req[(int)MC3ERequestCommandByteIndex.Address]);
            cmd.DeviceType = req[(int)MC3ERequestCommandByteIndex.DeviceType];

            // get word count
            cmd.WordCount = BitConverter.ToUInt16(req, (int)MC3ERequestCommandByteIndex.WordCount);

            // Write Command Only...
            if (cmd.MainCommand == (ushort)EnumMC3ERequestCommand.Write)
            {
                // get values as words
                cmd.Values = new ushort[cmd.WordCount];
                for (int i = 0; i < cmd.WordCount; i++)
                {
                    cmd.Values[i] = BitConverter.ToUInt16(req, (int)MC3ERequestCommandByteIndex.Values + i * 2);
                }
            }

            return cmd;
        }
    }
}
