using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace QwLua.Reflection.Implements
{
    internal class ConstructorImplement : MemberImplement, IConstructor
    {
        private Func<object[], object> _invoker;

        public ConstructorImplement(ConstructorInfo ctor)
            : base(ctor)
        {
        }

        public ConstructorInfo Constructor
        {
            get { return Member as ConstructorInfo; }
        }

        public object Inovke(params object[] @params)
        {
            if (_invoker != null)
                return _invoker(@params);
            return null;
        }

        protected override void CreateExpression()
        {
            var constructorInfo = Constructor;
            // parameters to execute
            var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

            // build parameter list
            var parameterExpressions = new List<Expression>();
            var paramInfos = constructorInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                // (Ti)parameters[i]
                var valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                var valueCast = Expression.Convert(valueObj, paramInfos[i].ParameterType);

                parameterExpressions.Add(valueCast);
            }

            // new T((T0)parameters[0], (T1)parameters[1], ...)
            var instanceCreate = Expression.New(constructorInfo, parameterExpressions);

            // (object)new T((T0)parameters[0], (T1)parameters[1], ...)
            var instanceCreateCast = Expression.Convert(instanceCreate, typeof(object));

            var lambda = Expression.Lambda<Func<object[], object>>(instanceCreateCast, parametersParameter);

            _invoker = lambda.Compile();
        }
    }
}
