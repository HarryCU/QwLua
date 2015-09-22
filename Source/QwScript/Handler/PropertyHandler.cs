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
using QwLua.Data;
using QwLua.Helpers;
using QwLua.Reflection;

namespace QwLua.Handler
{
    internal sealed class PropertyHandler : BaseHandler, INewIndexer
    {
        private IProperty _property;
        private object _instance;

        public PropertyHandler(ScriptState luaState)
            : base(luaState)
        {
        }

        public IProperty Property
        {
            get { return _property; }
        }

        public override void Initilaze(object data)
        {
            var objs = data as object[];
            if (objs != null)
            {
                _property = objs[0] as IProperty;
                _instance = objs[1];
            }
        }

        private object GetInstance()
        {
            object instance = null;
            if (!Property.IsStatic)
                instance = _instance;
            return instance;
        }

        public override int Reg2Env()
        {
            var instance = GetInstance();
            var value = Property.GetValue(instance);
            DataHelper.PushObject(State, value);
            return 1;
        }

        public void SetValue(object value)
        {
            var instance = GetInstance();
            object v = Convert.ChangeType(value, Property.Type);
            Property.SetValue(instance, v);
        }
    }
}
