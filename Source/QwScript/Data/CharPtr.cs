/**
 * Copyright (c) 2015, Harry CU 邱允根 (292350862@qq.com).
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
