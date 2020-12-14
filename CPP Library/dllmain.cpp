// dllmain.cpp : Definiert den Einstiegspunkt für die DLL-Anwendung.
#include "pch.h"

#define PINVOKE extern "C" __declspec(dllexport)

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}

struct CSharpMethodsDefinitions {
	void (*Function1)(double test);
};

static int Test = 123;
static CSharpMethodsDefinitions* GlobalCSharpMethods;

#pragma region Events

struct EventDefinitions {
	void* Event1;
	void* Event2;
};

double Event1() {
	Test = 543;
	return Test;
}

double Event2() {
	GlobalCSharpMethods->Function1(Test);
	return Test;
}

#pragma endregion

PINVOKE bool Initialize(void __stdcall RegisterCPPEvents(EventDefinitions), CSharpMethodsDefinitions* CSharpMethods) {

	// save C# method pointers
	GlobalCSharpMethods = CSharpMethods;

	// CSharpMethods->Function1(555);

	// Expose own methods
	EventDefinitions events = EventDefinitions();
	events.Event1 = Event1;
	events.Event2 = Event2;
	RegisterCPPEvents(events);

	// Message loop
	while(true){}
	
	return true;
}