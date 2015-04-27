/*
** $Id: ldotNet.h,v 0.0.0.1 2014/01/22 15:59 roberto Exp $
** 'dotNet' functions for Lua
*/
#include "ldotNet.h"

static const luaL_Reg loadedLibs[] = {
  {"_G", luaopen_base},
#if INCLUDE_PACKAGE
  {LUA_LOADLIBNAME, luaopen_package},
#endif
  {LUA_COLIBNAME, luaopen_coroutine},
  {LUA_TABLIBNAME, luaopen_table},
#if INCLUDE_IO
  {LUA_IOLIBNAME, luaopen_io},
#endif
#if INCLUDE_OS
  {LUA_OSLIBNAME, luaopen_os},
#endif
  {LUA_STRLIBNAME, luaopen_string},
  {LUA_BITLIBNAME, luaopen_bit32},
  {LUA_MATHLIBNAME, luaopen_math},
  {LUA_DBLIBNAME, luaopen_debug},
  {LUA_BASE64, luaopen_base64},
  {NULL, NULL}
};

LUALIB_API void ldotNet_openlibs (lua_State *L) {
  const luaL_Reg *lib;
  /* call open functions from 'loadedlibs' and set results to global table */
  for (lib = loadedLibs; lib->func; lib++) {
    luaL_requiref(L, lib->name, lib->func, 1);
    lua_pop(L, 1);  /* remove lib */
  }
}

LDOTNET_API void ldotNet_setDotNetHook(dotNet_hook hook) {
	if(hook != NULL) {
		g_dotNetHook = hook;
	}
}

LDOTNET_API lua_State *ldotNet_initlizeEngine() {
	lua_State *L = luaL_newstate();
	luaL_checkversion(L);
	lua_gc(L, LUA_GCSTOP, 0);  /* stop collector during initialization */
	ldotNet_openlibs(L);
	lua_gc(L, LUA_GCRESTART, 0);
	return L;
}

LDOTNET_API void ldotNet_closeEngine(lua_State *L) {
	lua_popglobaltable(L);
	lua_close(L);
}

LDOTNET_STATIC int ldotNet_closure (lua_State *L) 
{
	ldotNet_invokeMethod call = (ldotNet_invokeMethod)lua_touserdata (L, lua_upvalueindex (1));
	if(call == NULL)
		return 0;
	return call (L);
}

LDOTNET_API void ldotNet_pushdotNetMethod(lua_State *L, ldotNet_invokeMethod call) {
	lua_pushlightuserdata (L, (void *) call);
	lua_pushcclosure (L, ldotNet_closure, 1);
}

LDOTNET_API void ldotNet_pushFunction(lua_State *L, lua_CFunction call) {
	lua_pushcfunction(L, call);
}


LDOTNET_API int ldotNet_runScript(lua_State *L, TString lua_script) {
	return luaL_dostring(L, lua_script);
}

LDOTNET_API int ldotNet_runFile(lua_State *L, TString file) {
	return luaL_dofile(L, file);
}

LDOTNET_API int ldotNet_pushError(lua_State *L) {
	return lua_error(L);
}


LDOTNET_API int ldotNet_getLuaType(lua_State *L, int index) {
	return lua_type(L, index);
}


/*
**  获取数据
*/
LDOTNET_API lua_Number ldotNet_toNumber(lua_State *L, int index) {
	return luaL_checknumber(L, index);
}

LDOTNET_API TString ldotNet_toString(lua_State *L, int index) {
	TString s = luaL_checkstring(L, index);
	return s;
}

LDOTNET_API lua_Integer ldotNet_toInt(lua_State *L, int index) {
	return luaL_checkinteger(L, index);
}

LDOTNET_API int ldotNet_toBool(lua_State *L, int index) {
	return lua_toboolean(L, index);
}

/*
**  插入数据
*/
LDOTNET_API void ldotNet_pushNull(lua_State *L) {
	lua_pushnil(L);
}

LDOTNET_API void ldotNet_pushString(lua_State *L, TString str) {
	lua_pushstring(L, str);
}

LDOTNET_API void ldotNet_pushNumber(lua_State *L, lua_Number number) {
	lua_pushnumber(L, number);
}

LDOTNET_API void ldotNet_pushBool(lua_State *L, int b) {
	lua_pushboolean(L, b);
}

LDOTNET_API void ldotNet_pushInt(lua_State *L, lua_Integer i) {
	lua_pushinteger(L, i);
}


LDOTNET_API void ldotNet_atpanic(lua_State *L, lua_CFunction fn) {
	lua_atpanic(L, fn);
}

LDOTNET_API void ldotNet_registerGlobalMethod(lua_State *L, TString methodName, lua_CFunction fn) {
	lua_register(L, methodName, fn);
}

LDOTNET_API int ldotNet_getRef(lua_State *L, int index) {
	lua_pushvalue(L, index);
	int ref = luaL_ref(L, LUA_REGISTRYINDEX);
	return ref; 
}

LDOTNET_API int ldotNet_getGlobalRef(lua_State *L, TString globalName, int luaType) {
	lua_getglobal(L, globalName);
	luaL_checktype(L, -1, luaType);

	int ref = ldotNet_getRef(L, -1);
	return ref; 
}


LDOTNET_API void ldotNet_loopTable(lua_State *L, int index, lua_CFunction fn) {
	lua_rawgeti(L, LUA_REGISTRYINDEX, index);
	lua_pushnil(L);

	while (lua_next(L, -2) != 0) {
		fn(L);
		lua_settop(L, -2);
	}
}

LDOTNET_API void ldotNet_setTableValue(lua_State *L, int index, lua_CFunction fn) {
	lua_rawgeti(L, LUA_REGISTRYINDEX, index);
	fn(L);
	ldotNet_setTable(L, -3);
}

LDOTNET_API void ldotNet_createTable(lua_State *L) {
	lua_newtable(L);
}

LDOTNET_API void ldotNet_setTable(lua_State *L, int index) {
	lua_settable(L, index);
}

LDOTNET_API void ldotNet_getTable(lua_State *L, int index) {
	lua_gettable(L, index);
}

LDOTNET_API void ldotNet_setGlobalTable(lua_State *L) {
	lua_settable(L, LUA_REGISTRYINDEX);
}

LDOTNET_API void ldotNet_newMetatable(lua_State *L, TString name) {
	luaL_newmetatable(L, name);
}

LDOTNET_API void ldotNet_getMetatable(lua_State *L, TString name) {
	luaL_getmetatable(L, name);
}

LDOTNET_API void ldotNet_setMetatable(lua_State *L, int index) {
	lua_setmetatable(L, index);
}

LDOTNET_STATIC void ldotNet_setDebugInfo(lua_State *L, lua_Debug *ar, ldotNet_Debug *d) {
	d->eventCode=ar->event;
	d->currentline=ar->currentline;
	d->istailcall=ar->istailcall;
	d->isvararg=ar->isvararg;
	d->lastlinedefined=ar->lastlinedefined;
	d->linedefined=ar->linedefined;
	if(ar->name)
		d->name=ar->name;
	if(ar->namewhat)
		d->namewhat=ar->namewhat;
	d->nparams=ar->nparams;
	d->nups=ar->nups;

	strcpy(d->short_src, ar->short_src);

	d->what=ar->what;
	d->source=ar->source;
	d->L = L;
	d->ar = ar;
}

LDOTNET_STATIC void ldotNet_hookHandler(lua_State *L, lua_Debug *ar) {
	if(g_dotNetHook != NULL) {
		lua_getinfo(L, LDOTNET_DEBUG_WHAT, ar);
		ldotNet_Debug *d = new ldotNet_Debug();

		ldotNet_setDebugInfo(L, ar, d);

		g_dotNetHook(L, d);

		delete d;
		d = NULL;
	}
}

LDOTNET_API int ldotNet_setHook(lua_State *L, int mask, int count) {
	if(mask == 0) {
		return lua_sethook(L, NULL, mask, count);
	}
	return lua_sethook(L, ldotNet_hookHandler, mask, count);
}

LDOTNET_API int ldotNet_getHookCount(lua_State *L) {
	return lua_gethookcount(L);
}
LDOTNET_API TString ldotNet_getLocal(lua_State *L, const lua_Debug *debug, int n) {
	return lua_getlocal(L, debug, n);
}
LDOTNET_API TString ldotNet_setLocal(lua_State *L, const lua_Debug *debug, int n) {
	return lua_setlocal(L, debug, n);
}
LDOTNET_API int ldotNet_getHookMask(lua_State *L) {
	return lua_gethookmask(L);
}

LDOTNET_API void ldotNet_globalRawGet(lua_State *L, int index) {
	lua_rawgeti(L, LUA_REGISTRYINDEX, index);
}

LDOTNET_API int *ldotNet_newUserData(lua_State *L, int value) {
	int *p = (int *)lua_newuserdata(L, sizeof(int));
	*p = value;
	return p;
}
LDOTNET_API int ldotNet_toUserData(lua_State *L, int index) {
	int *p = (int *)lua_touserdata(L, index);
	if(p == NULL)
		return -1;
	return *p;
}
LDOTNET_API int ldotNet_checkUserData(lua_State *L, int index, TString name) {
	int *p = (int *)luaL_checkudata(L, index, name);
	if(p == NULL)
		return -1;
	return *p;
}

LDOTNET_API void ldotNet_pushValue(lua_State *L, int index) {
	lua_pushvalue(L, index);
}

LDOTNET_API void ldotNet_removeValue(lua_State *L, int index) {
	lua_remove(L, index);
}

LDOTNET_API void ldotNet_rawsSet(lua_State *L, int index) {
	lua_rawset(L, index);
}

LDOTNET_API void ldotNet_rawsGet(lua_State *L, int index) {
	lua_rawget(L, index);
}

LDOTNET_API int ldotNet_loadBuffer (lua_State *L, TString buff, size_t sz, TString name) {
	if (sz == 0)
		sz = strlen (buff);
	return luaL_loadbuffer (L, buff, sz, name);
}

LDOTNET_API int ldotNet_getTop(lua_State *L) {
	return lua_gettop(L);
}
LDOTNET_API void ldotNet_setTop(lua_State *L, int topIndex) {
	lua_settop(L, topIndex);
}

LDOTNET_API int ldotNet_call(lua_State *L, int nargs, int nresults, int errfunc) {
	return lua_pcall(L, nargs, nresults, errfunc);
}


LDOTNET_API void ldotNet_getGlobal(lua_State *L, TString globalName) {
	lua_getglobal(L, globalName);
}
LDOTNET_API void ldotNet_setGlobal(lua_State *L, TString globalName) {
	lua_setglobal(L, globalName);
}

LDOTNET_API void ldotNet_rawSetI(lua_State *L, int tableIndex, int index) {
	lua_rawseti(L, tableIndex, index);
}
LDOTNET_API void ldotNet_rawGetI(lua_State *L, int tableIndex, int index) {
	lua_rawgeti(L, tableIndex, index);
}

LDOTNET_API void ldotNet_where(lua_State *L, int level) {
	luaL_where(L, level);
}

LDOTNET_API size_t ldotNet_rawLength(lua_State *L, int index) {
	return lua_rawlen(L, index);
}

LDOTNET_API int ldotNet_rawIsLuaType(lua_State *L, int luaType, int index) {
	switch (luaType) {
	case LUA_TNIL:
		return lua_isnil(L, index);
	case LUA_TBOOLEAN:
		return lua_isboolean(L, index);
	case LUA_TLIGHTUSERDATA:
		return lua_islightuserdata(L, index);
	case LUA_TNUMBER:
		return lua_isnumber(L, index);
	case LUA_TSTRING:
		return lua_isstring(L, index);
	case LUA_TTABLE:
		return lua_istable(L, index);
	case LUA_TFUNCTION:
		return lua_isfunction(L, index);
	case LUA_TUSERDATA:
		return lua_isuserdata(L, index);
	case LUA_TTHREAD:
		return lua_isthread(L, index);
	}
	return 0;
}


LDOTNET_API void ldotNet_gc(lua_State *L, int what, int data) {
	lua_gc(L, what, data);
}

LDOTNET_API int ldotNet_getInfo(lua_State *L, ldotNet_Debug *d) {
	lua_Debug *ar = new lua_Debug;
	int result = lua_getinfo(L, LDOTNET_DEBUG_WHAT, ar);
	ldotNet_setDebugInfo(L, ar, d);
	//delete ar;
	//ar = NULL;
	return result;
}

LDOTNET_API void ldotNet_pop(lua_State *L, int index) {
	lua_pop(L, index);
}

LDOTNET_API int ldotNet_getStack(lua_State *L, int level, ldotNet_Debug *d) {
	lua_Debug *ar = new lua_Debug;
	int result = lua_getstack(L, level, ar);
	ldotNet_setDebugInfo(L, ar, d);
	//delete ar;
	//ar = NULL;
	return result;
}


LDOTNET_API void ldotNet_freeDebug(ldotNet_Debug *d) {
	if(d != NULL && d->ar != NULL) {
		delete d->ar;
		d->ar = NULL;
	}
}