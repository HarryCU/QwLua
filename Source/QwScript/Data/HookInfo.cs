using System.Runtime.InteropServices;
using QwLua.Helpers;
using QwLua.Implements;

namespace QwLua.Data
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct HookInfo
    {
        /// int
        public int eventCode;

        /// char*
        [MarshalAs(UnmanagedType.LPStr)]
        public string name;

        /// char*
        [MarshalAs(UnmanagedType.LPStr)]
        public string namewhat;

        /// char*
        [MarshalAs(UnmanagedType.LPStr)]
        public string what;

        /// char*
        [MarshalAs(UnmanagedType.LPStr)]
        public string source;

        /// int
        public int currentline;

        /// int
        public int linedefined;

        /// int
        public int lastlinedefined;

        /// unsigned char
        public byte nups;

        /// unsigned char
        public byte nparams;

        /// char
        public byte isvararg;

        /// char
        public byte istailcall;

        /// char[60]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 60)]
        public string short_src;

        public ScriptState L;

        public LuaDebug ar;

        public object GetVariate(string varName)
        {
            int index;
            return SearchGlobal(varName) ?? SearchLocal(varName, out index);
        }

        public void SetVariate(string varName, object v)
        {
            var value = SearchGlobal(varName);
            if (value != null)
            {
                DataHelper.PushObject(L, v);
                LuaCore.SetGlobal(L, varName);
                LuaCore.Pop(L, 1);
                return;
            }
            int index;
            if (SearchLocal(varName, out index) != null)
            {
                DataHelper.PushObject(L, v);
                LuaCore.SetLocal(L, ar, index);
                LuaCore.Pop(L, 1);
            }
        }

        private object SearchLocal(string varName, out int index)
        {
            int i = 1;
            string localName;
            object result = null;
            while ((localName = LuaCore.GetLocal(L, ar, i)) != string.Empty)
            {
                if (localName == varName)
                {
                    result = DataHelper.GetObject(L, -1);
                    LuaCore.Pop(L, 1);
                    break;
                }
                LuaCore.Pop(L, 1);
                i++;
            }
            index = i;
            return result;
        }

        private object SearchGlobal(string varName)
        {
            LuaCore.GetGlobal(L, varName);
            var result = DataHelper.GetObject(L, -1);
            LuaCore.Pop(L, 1);
            return result;
        }
    }
}