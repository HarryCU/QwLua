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
            foreach (var p in @params)
            {
                string typeName = null;
                var instance = p as ParameterInfo;
                if (instance != null)
                {
                    typeName = instance.ParameterType.FullName;
                }
                else
                {
                    var t = p as Type;
                    typeName = t != null ? t.FullName : p.GetType().FullName;
                }
                builder.Append(typeName);
                if (count != 1)
                    builder.Append(',');
                count--;
            }
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
