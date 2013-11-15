// Copyright 2012 Intel Corporation
//
// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.

#define WIN32_LEAN_AND_MEAN
#include <windows.h>

#include <wchar.h>
#include <rpc.h>

#pragma comment(lib,"rpcrt4.lib")

// GetComputerName
// GetVersionEx

#include "shell.h"
#include "device.h"
#include "common.h"

#define CORDOVA_MACHINE_ID		L"MachineID"
#define CORDOVA_VERSION			L"2.1.0"
#define CORDOVA_VERSION_LEN		5


static HRESULT callDotNetMethod(BSTR callback_id, BSTR args)
{
	// Set initial Cordova global variables and fire up deviceready event
	//static wchar_t buf[100 + UUID_BUF_LEN + COMPUTER_NAME_BUF_LEN + CORDOVA_VERSION_LEN];
	//wchar_t computer_name[COMPUTER_NAME_BUF_LEN];
	//DWORD len = COMPUTER_NAME_BUF_LEN;
	//OSVERSIONINFOEX osver;
	//wchar_t* platform = L"Windows";

	//computer_name[0] = L'\0';
	//GetComputerName(computer_name, &len);

	//osver.dwOSVersionInfoSize = sizeof(osver);
	//GetVersionEx((LPOSVERSIONINFO)&osver);

	//wsprintf(buf, L"{uuid:'%s',name:'%s',platform:'%s',version:'%d.%d',cordova:'%s'}",
	//			uuid, computer_name, platform, osver.dwMajorVersion, osver.dwMinorVersion, CORDOVA_VERSION);

	//cordova_success_callback(callback_id, FALSE, buf);

	

	return S_OK;
}

HRESULT dotnet_exec(BSTR callback_id, BSTR action, BSTR args, VARIANT *result)
{
	if (!wcscmp(action, L"callDotNetMethod"))
			return callDotNetMethod(callback_id, args);

	return DISP_E_MEMBERNOTFOUND;
}

static void dotnet_module_init(void)
{
	//acquire_unique_id(uuid);
}

DEFINE_CORDOVA_MODULE(DotNet, L"DotNet", dotnet_exec, dotnet_module_init, NULL)