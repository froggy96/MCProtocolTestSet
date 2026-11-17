using MCProtocolEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MCProtocolPLCEmulator
{
    public static class MCProtocolCommand
    {

        #region [Request Command : PC -> PLC]

        /// <summary>
        /// Binary 3E or 4E request (PC --> PLC)
        /// </summary>
        public class MC3ERequestCommand
        {

            public ushort ProtocolType { get; set; } = 0x0050; // 0x0050 = MC3E, 0x0054 = MC4E

            public ushort SerialNumber { get; set; }

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

        /// <summary>
        /// Binary 3E or 4E response (PLC --> PC)
        /// </summary>
        public class MC3EResponseMessage
        {
            public ushort ProtocolType { get; set; } = 0x00D0;

            public ushort SerialNumber { get; set; }

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

        public const ushort Request3E = 0x0050;
        public const ushort Request4E = 0x0054;
        public const int RESPONSE_HEADER_SIGN = 0x00D0;
        public const ushort Response4E = 0x00D4;
        public const int SerialAndReservedLength = 4;

        public static EnumEndCode ExecuteRequest(MC3ERequestCommand req, PLCWordMemory plc)
        {
            if (req.MainCommand != (ushort)EnumMC3ERequestCommand.Write &&
                req.MainCommand != (ushort)EnumMC3ERequestCommand.Read)
            {
                return EnumEndCode.CommandNotSupported;
            }
            if (req.SubCommand != 0)
            {
                return EnumEndCode.CommandTypeError;
            }
            if (plc == null)
            {
                return EnumEndCode.DeviceNotFound;
            }
            if (req.MainCommand == (ushort)EnumMC3ERequestCommand.Write)
            {
                return plc.Write((int)req.Address, req.WordCount, req.Values);
            }
            var endCode = plc.Read((int)req.Address, req.WordCount, out ushort[] values);
            req.Values = values;
            return endCode;
        }

        public static MC3EResponseMessage SetResponseMessage(MC3ERequestCommand req, ushort end_code, ushort[] iData)
        {
            var cmd = new MC3EResponseMessage();

            cmd.ProtocolType = req.ProtocolType == Request4E ? Response4E : (ushort)RESPONSE_HEADER_SIGN;
            cmd.SerialNumber = req.SerialNumber;

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
            if (end_code != 0 && cmd.ProtocolType == Response4E)
            {
                cmd.Values = new byte[]
                {
                    req.NetworkNo, req.PCNo, (byte)req.IONo, (byte)(req.IONo >> 8), req.ChNo,
                    (byte)req.MainCommand, (byte)(req.MainCommand >> 8),
                    (byte)req.SubCommand, (byte)(req.SubCommand >> 8)
                };
            }
            else if (end_code == 0 && req.MainCommand == (ushort)EnumMC3ERequestCommand.Read && iData != null)
            {
                var data = new List<byte>();
                foreach (int t in iData)
                {
                    data.Add((byte)t);
                    data.Add((byte)(t >> 8));
                }
                cmd.Values = data.ToArray();
            }
            cmd.DataLength = checked((ushort)(2 + (cmd.Values?.Length ?? 0)));
            //

            return cmd;
        }

        public static byte[] EncodeMC3EResponseMessage(MC3EResponseMessage res)
        {
            return EncodeResponseMessage(res);
        }

        public static byte[] EncodeResponseMessage(MC3EResponseMessage res)
        {
            bool is4E = res.ProtocolType == Response4E;
            int headerExtension = is4E ? SerialAndReservedLength : 0;
            var ret = new List<byte>(MIN_RES_CMD_LEN + headerExtension + res.DataLength - 2);
            ret.Add((byte)res.ProtocolType);
            ret.Add((byte)(res.ProtocolType >> 8));

            if (is4E)
            {
                ret.Add((byte)res.SerialNumber);
                ret.Add((byte)(res.SerialNumber >> 8));
                ret.Add(0x00);
                ret.Add(0x00);
            }

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
            if (req.Length < MIN_REQ_CMD_LEN || BitConverter.ToUInt16(req, 0) != Request3E)
            {
                throw new ArgumentException("Expected a binary 3E word request.", nameof(req));
            }
            return DecodeRequestCommand(req);
        }

        public static MC3ERequestCommand DecodeRequestCommand(byte[] req)
        {
            if (req.Length < 2)
            {
                throw new ArgumentException("Incomplete request subheader.", nameof(req));
            }

            ushort protocol = BitConverter.ToUInt16(req, 0);
            bool is4E = protocol == Request4E;
            int extension = is4E ? SerialAndReservedLength : 0;
            int bodyStart = (int)MC3ERequestFrameByteIndex.CPUTimer + extension;
            if ((protocol != Request3E && !is4E) || req.Length < bodyStart + 6 ||
                (is4E && (req[4] != 0 || req[5] != 0)))
            {
                throw new ArgumentException("Invalid request header.", nameof(req));
            }

            var cmd = new MC3ERequestCommand
            {
                ProtocolType = protocol,
                SerialNumber = is4E ? BitConverter.ToUInt16(req, 2) : (ushort)0,
                NetworkNo = req[(int)MC3ERequestFrameByteIndex.NetworkNo + extension],
                PCNo = req[(int)MC3ERequestFrameByteIndex.PCNo + extension],
                IONo = BitConverter.ToUInt16(req, (int)MC3ERequestFrameByteIndex.IONo + extension),
                ChNo = req[(int)MC3ERequestFrameByteIndex.ChNo + extension],
                DataLength = BitConverter.ToUInt16(req, (int)MC3ERequestFrameByteIndex.DataLength + extension),
                CPUTimer = BitConverter.ToUInt16(req, bodyStart),
                MainCommand = BitConverter.ToUInt16(req, (int)MC3ERequestFrameByteIndex.MainCommand + extension),
                SubCommand = BitConverter.ToUInt16(req, (int)MC3ERequestFrameByteIndex.SubCommand + extension)
            };
            if (req.Length != bodyStart + cmd.DataLength)
            {
                throw new ArgumentException("Request data length does not match the frame.", nameof(req));
            }

            bool isWrite = cmd.MainCommand == (ushort)EnumMC3ERequestCommand.Write;
            if (cmd.SubCommand != 0 || (!isWrite && cmd.MainCommand != (ushort)EnumMC3ERequestCommand.Read))
            {
                return cmd;
            }

            int valuesStart = MIN_REQ_CMD_LEN + extension;
            if (req.Length < valuesStart)
            {
                throw new ArgumentException("Incomplete word request.", nameof(req));
            }
            int addressStart = (int)MC3ERequestFrameByteIndex.Address + extension;
            cmd.Address = (uint)(req[addressStart] | (req[addressStart + 1] << 8) | (req[addressStart + 2] << 16));
            cmd.DeviceType = req[(int)MC3ERequestFrameByteIndex.DeviceType + extension];
            cmd.WordCount = BitConverter.ToUInt16(req, (int)MC3ERequestFrameByteIndex.WordCount + extension);
            if (req.Length != valuesStart + (isWrite ? cmd.WordCount * 2 : 0))
            {
                throw new ArgumentException("Word payload length does not match the request.", nameof(req));
            }
            if (isWrite)
            {
                cmd.Values = new ushort[cmd.WordCount];
                for (int i = 0; i < cmd.WordCount; i++)
                {
                    cmd.Values[i] = BitConverter.ToUInt16(req, valuesStart + i * 2);
                }
            }

            return cmd;
        }
    }
}
