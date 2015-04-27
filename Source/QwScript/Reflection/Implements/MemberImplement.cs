
using System.Reflection;

namespace QwLua.Reflection.Implements
{
    internal abstract class MemberImplement : IMember
    {
        private readonly MemberInfo _member;

        protected MemberImplement(MemberInfo member)
        {
            _member = member;

            CreateExpression();
        }

        protected abstract void CreateExpression();

        public string Name
        {
            get { return Member.Name; }
        }

        public MemberInfo Member
        {
            get { return _member; }
        }
    }
}
