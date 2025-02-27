using MCProtocolEnums;
using System;
using System.Collections.Generic;

namespace MCProtocolPLCEmulator
{

    public class PLCWordMemory
    {

        public event EventHandler<EventArgs> MemoryWritten;
        private void OnMemoryWritten(EventArgs e)
        {
            MemoryWritten?.Invoke(this, e);
        }

        public char DeviceLetter;

        public ushort[] Words;

        public PLCWordMemory(char device_letter)
        {
            DeviceLetter = device_letter;
            Words = new ushort[20_000];
        }

        public PLCWordMemory(int word_count, char device_letter)
        {
            DeviceLetter = device_letter;
            Words = new ushort[word_count];
        }

        public Dictionary<string, ushort> CaptureMemory(int start, int length, string address_number_format = "D6")
        {
            var ret = new Dictionary<string, ushort>();

            if (start + length <= Words.Length)
            {
                for (int i = start; i < length; i++)
                {
                    ret.Add(DeviceLetter + i.ToString(address_number_format), Words[i]);
                }
            }

            return ret;
        }

        public EnumEndCode Read(int start, int length, out ushort[] iData)
        {
            if (start + length <= Words.Length)
            {
                iData = new ushort[length];
                for (int i = 0; i < length; i++)
                {
                    iData[i] = Words[start + i];
                }
                return EnumEndCode.Success;
            }
            else
            {
                iData = null;
                return EnumEndCode.OutOfAddressingRange;
            }
        }


        public EnumEndCode Write(int start, int length, ushort[] iData)
        {
            if (start + length <= Words.Length)
            {
                for (int i = 0; i < length; i++)
                {
                    Words[start + i] = iData[i];
                }

                //
                OnMemoryWritten(new EventArgs());
                //

                return EnumEndCode.Success;
            }
            else
            {
                return EnumEndCode.OutOfAddressingRange;
            }
        }

        public EnumEndCode WriteString(int start, string str)
        {
            if (start + (str.Length + 1) / 2 + (str.Length + 1) % 2 <= Words.Length)
            {
                int w = 0;
                for (int i = 0; i < str.Length; i += 2)
                {
                    ushort ch1 = (ushort)str[i];
                    ushort ch2 = i + 1 < str.Length ? (ushort)str[i + 1] : (ushort)0x00;
                    Words[start + w] = (ushort)(ch1 << 8 | ch2);
                    w++;
                }

                //
                OnMemoryWritten(new EventArgs());
                //

                return EnumEndCode.Success;
            }
            else
            {
                return EnumEndCode.OutOfAddressingRange;
            }
        }

    }
}
