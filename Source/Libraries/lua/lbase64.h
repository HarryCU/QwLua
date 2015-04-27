/*
* lbase64.c
* base64 encoding and decoding for Lua 5.2
* Luiz Henrique de Figueiredo <lhf@tecgraf.puc-rio.br>
* 07 Aug 2012 23:22:44
* This code is hereby placed in the public domain.
*/

#ifndef lbase64_h
#define lbase64_h

#include <string.h>

#include "luaSrc/lua.h"
#include "luaSrc/lauxlib.h"

#define MYNAME		"base64"
#define MYVERSION	MYNAME " library for " LUA_VERSION " / Aug 2012"

#define LUA_BASE64 MYNAME

#define uint unsigned int

LUALIB_API int luaopen_base64(lua_State *L);

#endif
