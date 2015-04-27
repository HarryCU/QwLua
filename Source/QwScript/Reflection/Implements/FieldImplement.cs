using System;
using System.Linq.Expressions;
using System.Reflection;

namespace QwLua.Reflection.Implements
{
    internal class FieldImplement : MemberImplement, IField
    {
        private Func<object, object> _getter;
        private Action<object, object> _setter;

        public bool IsStatic
        {
            get { return Field.IsStatic; }
        }

        public FieldInfo Field
        {
            get { return Member as FieldInfo; }
        }

        public FieldImplement(FieldInfo field)
            : base(field)
        {
        }

        protected override void CreateExpression()
        {
            var fieldInfo = Field;

            var instance = Expression.Parameter(typeof(object), "instance");
            var instanceCast = fieldInfo.IsStatic ? null :
                Expression.Convert(instance, fieldInfo.ReflectedType);
            var fieldAccess = Expression.Field(instanceCast, fieldInfo);
            {
                var castFieldValue = Expression.Convert(fieldAccess, typeof(object));
                var lambda = Expression.Lambda<Func<object, object>>(castFieldValue, instance);
                _getter = lambda.Compile();
            }
            {
#if NET4
                var paramter = Expression.Parameter(typeof(object), "paramter");
                var assignFieldValue = Expression.Assign(fieldAccess, paramter);
                var lambda = Expression.Lambda<Action<object, object>>(assignFieldValue, instance);
                m_Setter = lambda.Compile();
#else
                _setter = (obj, value) =>
                {
                    Field.SetValue(obj, value);
                };
#endif
            }
        }

        public void SetValue(object instance, object value)
        {
            if (_setter != null)
                _setter(instance, value);
        }

        public object GetValue(object instance)
        {
            if (_getter != null)
                return _getter(instance);
            return null;
        }
    }
}
