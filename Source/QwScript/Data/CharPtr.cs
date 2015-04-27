using System;
using System.Runtime.InteropServices;
using System.Text;

namespace QwLua.Data
{
    public struct CharPtr
    {
        public CharPtr(IntPtr ptrString)
            : this()
        {
            _ptrString = ptrString;
        }

        static public implicit operator CharPtr(IntPtr ptr)
        {
            return new CharPtr(ptr);
        }

        static private string PointerToString(IntPtr ptr)
        {
            return Marshal.PtrToStringAnsi(ptr);
        }

        static private string PointerToString(IntPtr ptr, int length)
        {
            return Marshal.PtrToStringAnsi(ptr, length);
        }

        static private byte[] PointerToBuffer(IntPtr ptr, int length)
        {
            var buff = new byte[length];
            Marshal.Copy(ptr, buff, 0, length);
            return buff;
        }

        public override string ToString()
        {
            if (_ptrString == IntPtr.Zero)
                return string.Empty;

            return PointerToString(_ptrString);
        }

        public string ToString(int length)
        {
            if (_ptrString == IntPtr.Zero)
                return string.Empty;

            var buff = PointerToBuffer(_ptrString, length);
            if (length > 3 && buff[0] == 0x1B && buff[1] == 0x4C && buff[2] == 0x75 && buff[3] == 0x61)
            {
                var s = new StringBuilder(length);
                foreach (byte b in buff)
                    s.Append((char)b);
                return s.ToString();
            }
            return PointerToString(_ptrString, length);
        }

        private readonly IntPtr _ptrString;
    }
}
