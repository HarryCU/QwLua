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
using System.Collections;
using System.Reflection;
using QwLua.Data;
using QwLua.Handler;
using QwLua.Helpers;
using QwLua.Implements;
using QwLua.Reflection;
using QwLua.Wrapper;

namespace QwLua.Metatable
{
    internal abstract class BaseMetatable : ILuaRegister
    {
        private readonly ScriptState _luaState;

        protected BaseMetatable(ScriptState luaState)
        {
            _luaState = luaState;
        }

        protected ScriptState State
        {
            get { return _luaState; }
        }

        public abstract int Reg2Env();

        protected static BaseHandler GetMember(ScriptState luaState, object o, string memberName)
        {
            var runtime = LuaRuntimePool.GetRuntime(luaState);
            var type = o as Type;

            if (type != null)
            {
                // index(t, k) remove k param
                // call static method、field、protery
                LuaCore.RemoveValue(luaState, 1);
            }
            else
            {
                type = o.GetType();
            }

            var key = ScriptHelper.GenerateKey(type, memberName);
            var handler = runtime.ClassMgr.Cache[key] as BaseHandler;
            if (handler == null)
            {
                #region Create Handler

                const BindingFlags flags = ReflectionHelper.DefBindingFlags | BindingFlags.Static;
                var members = type.GetMember(memberName, flags);
                if (members.Length > 0)
                {
                    var member = members[0];
                    switch (member.MemberType)
                    {
                        case MemberTypes.Field:
                            handler = new FieldHandler(luaState);
                            handler.Initilaze(new[] { ReflectionHelper.GetField((FieldInfo)member), o });
                            break;
                        case MemberTypes.Property:
                            handler = new PropertyHandler(luaState);
                            handler.Initilaze(new[] { ReflectionHelper.GetProperty((PropertyInfo)member), o });
                            break;
                        case MemberTypes.Method:
                            handler = new MethodHandler(luaState);
                            handler.Initilaze(new MethodFinder(type, memberName, members));
                            break;
                    }
                }
                if (handler != null)
                {
                    runtime.ClassMgr.Cache.Add(key, handler);
                }

                #endregion
            }
            return handler;
        }

        protected static BaseHandler GetArrayHandler(ScriptState luaState, int index, object data, bool getter)
        {
            if (!LuaCore.RawIsLuaType(luaState, ScriptTypes.Number, index))
                return null;
            var type = data as Type;
            if (type != null)
                return null;
            type = data.GetType();
            var runtime = LuaRuntimePool.GetRuntime(luaState);
            var methodName = getter ? "get_Item" : "set_Item";
            var key = ScriptHelper.GenerateKey(type, methodName);
            var handler = runtime.ClassMgr.Cache[key] as BaseHandler;
            if (handler == null)
            {
                var isArray = type.IsArray;
                if (!isArray)
                {
                    isArray = ReflectionHelper.HasMethod(type, methodName, ReflectionHelper.DefBindingFlags | BindingFlags.Static);
                }
                if (isArray)
                {
                    var method = ReflectionHelper.GetMethod(type.GetMethod(methodName));
                    handler = new ArrayHandler(luaState);
                    handler.Initilaze(new[] { data, method, getter });
                    runtime.ClassMgr.Cache.Add(key, handler);
                }
            }
            return handler;
        }

        protected static BaseHandler GetItemHandler(ScriptState luaState, object data, bool getter)
        {
            if (data == null)
                return null;
            var methodName = getter ? "get_Item" : "set_Item";

            var type = data.GetType();
            var hasItem = ReflectionHelper.HasMethod(type, methodName, ReflectionHelper.DefBindingFlags | BindingFlags.Static);
            if (!hasItem)
                return null;
            var runtime = LuaRuntimePool.GetRuntime(luaState);
            var key = ScriptHelper.GenerateKey(type, methodName);
            var handler = runtime.ClassMgr.Cache[key] as BaseHandler;
            if (handler == null)
            {
                var method = ReflectionHelper.GetMethod(type.GetMethod(methodName));
                handler = new ItemHandler(luaState);
                handler.Initilaze(new[] { data, method, getter });
                runtime.ClassMgr.Cache.Add(key, handler);
            }
            return handler;
        }

    }
}