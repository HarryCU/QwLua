using System;
using System.Linq.Expressions;
using System.Reflection;

namespace QwLua.Reflection.Implements
{
    internal class PropertyImplement : MemberImplement, IProperty
    {
        private Func<object, object> _getter;
        private MethodImplement _setter;

        public bool IsStatic
        {
            get { return Property.GetGetMethod(true).IsStatic; }
        }

        public PropertyInfo Property
        {
            get { return Member as PropertyInfo; }
        }

        public Type Type { get { return Property.PropertyType; } }

        public PropertyImplement(PropertyInfo property)
            : base(property)
        {
        }

        protected override void CreateExpression()
        {
            var propertyInfo = Property;
            if (propertyInfo.CanRead)
            {
                var instance = Expression.Parameter(typeof(object), "instance");
                var instanceCast = propertyInfo.GetGetMethod(true).IsStatic ? null :
                    Expression.Convert(instance, propertyInfo.ReflectedType);
                var propertyAccess = Expression.Property(instanceCast, propertyInfo);
                var castPropertyValue = Expression.Convert(propertyAccess, typeof(object));
                var lambda = Expression.Lambda<Func<object, object>>(castPropertyValue, instance);
                _getter = lambda.Compile();
            }
            if (propertyInfo.CanWrite)
            {
                _setter = new MethodImplement(propertyInfo.GetSetMethod(true));
            }
        }

        public void SetValue(object instance, object value)
        {
            if (_setter != null)
                _setter.Invoke(instance, value);
        }

        public object GetValue(object instance)
        {
            if (_getter != null)
                return _getter(instance);
            return null;
        }
    }
}
