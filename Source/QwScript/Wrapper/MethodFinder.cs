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
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using QwLua.Reflection;

namespace QwLua.Wrapper
{
    internal sealed class MethodFinder
    {
        private readonly string _memberName;
        private readonly Type _type;
        private readonly IEnumerable<MemberInfo> _members;
        private readonly IDictionary<string, IMethod> _cache;
        private bool _isStatic;

        public Type Type
        {
            get { return _type; }
        }

        public string MemberName
        {
            get { return _memberName; }
        }

        public bool IsStatic
        {
            get { return _isStatic; }
        }

        public MethodFinder(Type type, string memberName, IEnumerable<MemberInfo> members)
        {
            _type = type;
            _memberName = memberName;
            _members = members;
            _isStatic = false;
            _cache = new Dictionary<string, IMethod>(5);

            InitMethodCache();
        }

        private static string GenerateKey(Type type, string memberName, ICollection<object> @params)
        {
            var builder = new StringBuilder(256);
            builder.Append(type).Append(".").Append(memberName).Append("(");
            var count = @params.Count;
            builder.Append(count);
            //foreach (var p in @params)
            //{
            //    string typeName = null;
            //    var instance = p as ParameterInfo;
            //    if (instance != null)
            //    {
            //        typeName = instance.ParameterType.FullName;
            //    }
            //    else
            //    {
            //        var t = p as Type;
            //        typeName = t != null ? t.FullName : p.GetType().FullName;
            //    }
            //    builder.Append(typeName);
            //    if (count != 1)
            //        builder.Append(',');
            //    count--;
            //}
            builder.Append(")");
            var key = builder.ToString();
            builder.Clear();
            return key;
        }

        public string CreateKey(ICollection<object> @params)
        {
            return GenerateKey(_type, _memberName, @params);
        }

        private void InitMethodCache()
        {
            foreach (var memberInfo in _members)
            {
                if (memberInfo.MemberType != MemberTypes.Method)
                    continue;
                var method = (MethodInfo)memberInfo;

                var m = ReflectionHelper.GetMethodWithoutCache(method);
                if (m == null) continue;
                if (m.IsStatic)
                    _isStatic = true;
                _cache.Add(GenerateKey(_type, _memberName, method.GetParameters()), m);
            }
        }

        private IMethod GetMethod(string key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            return null;
        }

        public IMethod Search(IList<object> @params)
        {
            var key = CreateKey(@params);
            var m = GetMethod(key);
            int index = @params.Count - 1;
            object objectType = typeof(object);
            while (m == null && index >= 0)
            {
                @params[index] = objectType;
                key = CreateKey(@params);
                m = GetMethod(key);
                index--;
            }
            return m;
        }
    }
}
