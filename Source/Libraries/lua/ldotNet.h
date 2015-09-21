/**
* Copyright (c) 2015, Harry CU ÇñÔÊ¸ù (292350862@qq.com).
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

/*
** $Id: ldotNet.h,v 0.0.0.1 2014/01/22 15:59 roberto Exp $
** 'dotNet' functions for Lua
*/

#ifndef ldotnet_h
#define ldotnet_h

#include "luaSrc\lua.hpp"
#include "windows.h"
extern "C" {
#include "lbase64.h"
}
#define LDOTNET_BUILD_AS_DLL

#ifdef __cplusplus
	#if defined(LDOTNET_BUILD_AS_DLL)
		#define LDOTNET_API extern "C" __declspec(dllexport)
	#else
		#define LDOTNET_API	extern "C"
	#endif
#else
	#if defined(LDOTNET_BUILD_AS_DLL)
		#define LDOTNET_API __declspec(dllexport)
	#else
		#define LDOTNET_API	extern
	#endif
#endif

#if defined(WIN32) || defined(WP8)
#define LUA_STDCALL __stdcall
#else
#define LUA_STDCALL
#endif

#define LDOTNET_STATIC static
#define TString const char * 
#define lua_popglobaltable(L)  \
	lua_rawseti(L, LUA_REGISTRYINDEX, LUA_RIDX_GLOBALS)

#define LDOTNET_DEBUG_WHAT "nSlutfL"

struct ldotNet_Debug {
	int eventCode;
	const char *name;	/* (n) */
	const char *namewhat;	/* (n) 'global', 'local', 'field', 'method' */
	const char *what;	/* (S) 'Lua', 'C', 'main', 'tail' */
	const char *source;	/* (S) */
	int currentline;	/* (l) */
	int linedefined;	/* (S) */
	int lastlinedefined;	/* (S) */
	unsigned char nups;	/* (u) number of upvalues */
	unsigned char nparams;/* (u) number of parameters */
	char isvararg;        /* (u) */
	char istailcall;	/* (t) */
	const char *short_src; /* (S) */
	lua_State *L;
	lua_Debug *ar;
};

typedef struct ldotNet_Debug ldotNet_Debug;

typedef void (*dotNet_Function)(lua_State *L);
typedef int (LUA_STDCALL *ldotNet_invokeMethod) (lua_State *L);
typedef void (*dotNet_hook)(lua_State *L, ldotNet_Debug *d);

LDOTNET_STATIC dotNet_hook g_dotNetHook;
LDOTNET_API void ldotNet_setDotNetHook(dotNet_hook hook);

LDOTNET_STATIC void ldotNet_setDebugInfo(lua_State *L, lua_Debug *ar, ldotNet_Debug *d);

LDOTNET_API lua_State *ldotNet_initlizeEngine();
LDOTNET_API void ldotNet_closeEngine(lua_State *L);
LDOTNET_API void ldotNet_pushdotNetMethod(lua_State *L, ldotNet_invokeMethod call);
LDOTNET_API void ldotNet_pushFunction(lua_State *L, lua_CFunction call);
LDOTNET_API int ldotNet_runScript(lua_State *L, TString lua_script);
LDOTNET_API int ldotNet_runFile(lua_State *L, TString file);
LDOTNET_API int ldotNet_pushError(lua_State *L);

LDOTNET_API int ldotNet_getLuaType(lua_State *L, int index);

LDOTNET_API void ldotNet_pushNull(lua_State *L);
LDOTNET_API void ldotNet_pushString(lua_State *L, TString str);
LDOTNET_API void ldotNet_pushNumber(lua_State *L, lua_Number number);
LDOTNET_API void ldotNet_pushBool(lua_State *L, int b);
LDOTNET_API void ldotNet_pushInt(lua_State *L, lua_Integer i);

LDOTNET_API lua_Number ldotNet_toNumber(lua_State *L, int index);
LDOTNET_API TString ldotNet_toString(lua_State *L, int index);
LDOTNET_API lua_Integer ldotNet_toInt(lua_State *L, int index);
LDOTNET_API int ldotNet_toBool(lua_State *L, int index);

LDOTNET_API void ldotNet_atpanic(lua_State *L, lua_CFunction fn);
LDOTNET_API void ldotNet_registerGlobalMethod(lua_State *L, TString methodName, lua_CFunction fn);

LDOTNET_API int ldotNet_getRef(lua_State *L, int index);
LDOTNET_API int ldotNet_getGlobalRef(lua_State *L, TString globalName, int luaType);

LDOTNET_API void ldotNet_loopTable(lua_State *L, int index, lua_CFunction fn);
LDOTNET_API void ldotNet_setTableValue(lua_State *L, int index, lua_CFunction fn);

LDOTNET_API void ldotNet_createTable(lua_State *L);
LDOTNET_API void ldotNet_getTable(lua_State *L, int index);
LDOTNET_API void ldotNet_setTable(lua_State *L, int index);
LDOTNET_API void ldotNet_setGlobalTable(lua_State *L);

LDOTNET_API void ldotNet_newMetatable(lua_State *L, TString name);
LDOTNET_API void ldotNet_getMetatable(lua_State *L, TString name);
LDOTNET_API void ldotNet_setMetatable(lua_State *L, int index);

LDOTNET_API int ldotNet_setHook(lua_State *L, int mask, int count);
LDOTNET_API int ldotNet_getHookCount(lua_State *L);
LDOTNET_API TString ldotNet_getLocal(lua_State *L, const lua_Debug *debug, int n);
LDOTNET_API TString ldotNet_setLocal(lua_State *L, const lua_Debug *debug, int n);
LDOTNET_API int ldotNet_getHookMask(lua_State *L);

LDOTNET_API void ldotNet_globalRawGet(lua_State *L, int index);


LDOTNET_API int *ldotNet_newUserData(lua_State *L, int value);
LDOTNET_API int ldotNet_toUserData(lua_State *L, int index);
LDOTNET_API int ldotNet_checkUserData(lua_State *L, int index, TString name);

LDOTNET_API void ldotNet_pushValue(lua_State *L, int index);
LDOTNET_API void ldotNet_removeValue(lua_State *L, int index);


LDOTNET_API void ldotNet_rawsSet(lua_State *L, int index);
LDOTNET_API void ldotNet_rawsGet(lua_State *L, int index);

LDOTNET_API int ldotNet_loadBuffer (lua_State *L, TString buff, size_t sz, TString name);

LDOTNET_API int ldotNet_getTop(lua_State *L);
LDOTNET_API void ldotNet_setTop(lua_State *L, int topIndex);

LDOTNET_API int ldotNet_call(lua_State *L, int nargs, int nresults, int errfunc);

LDOTNET_API void ldotNet_rawSetI(lua_State *L, int tableIndex, int index);
LDOTNET_API void ldotNet_rawGetI(lua_State *L, int tableIndex, int index);

LDOTNET_API void ldotNet_where(lua_State *L, int level);

LDOTNET_API size_t ldotNet_rawLength(lua_State *L, int index);

LDOTNET_API int ldotNet_rawIsLuaType(lua_State *L, int luaType, int index);

LDOTNET_API void ldotNet_gc(lua_State *L, int what, int data);

LDOTNET_API int ldotNet_getInfo(lua_State *L, ldotNet_Debug *d);
LDOTNET_API void ldotNet_pop(lua_State *L, int index);

LDOTNET_API int ldotNet_getStack(lua_State *L, int level, ldotNet_Debug *d);

LDOTNET_API void ldotNet_freeDebug(ldotNet_Debug *d);

#endif