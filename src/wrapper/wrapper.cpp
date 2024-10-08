// copyright-holders:K.Ito
#include "stdafx.h"

#include "windows.h"
#include "wrapper.h"
//#include "..\frontend\mame\mame.h"
#include "..\mame\MAmidiMEmo.h"
#include <vcclr.h>

using namespace System;
using namespace zanac::MAmidiMEmo;
using namespace System;
using namespace System::IO;
using namespace System::Reflection;


static Assembly ^LoadFromSameFolder(Object ^sender, ResolveEventArgs ^args)
{
	String ^folderPath = Path::GetDirectoryName(Assembly::GetExecutingAssembly()->Location);
	String ^assemblyPath = Path::Combine(folderPath, (gcnew AssemblyName(args->Name))->Name + ".dll");
	if (File::Exists(assemblyPath) == false) return nullptr;
	Assembly ^assembly = Assembly::LoadFrom(assemblyPath);
	return assembly;
}

extern "C" {

	//MAmidiMEmo

	// 明示的ロードでVC、DOTNETから使用するのでヘッダーファイルは不要
	// なお、__cdeclでもDOTNETから正しく呼び出せることを確認している。
	// 通常、__stdcallを適用する(__stdcall = WINAPI)。

	__declspec(dllexport) void __stdcall InitializeDotNet()
	{
		// put this somewhere you know it will run (early, when the DLL gets loaded)
		System::AppDomain ^currentDomain = AppDomain::CurrentDomain;
		currentDomain->AssemblyResolve += gcnew ResolveEventHandler(LoadFromSameFolder);
	}

	__declspec(dllexport) void __stdcall MainWarpper(HMODULE hParentModule)
	{
		zanac::MAmidiMEmo::Program::Main((IntPtr)hParentModule);
	}

	__declspec(dllexport) int __stdcall HasExited()
	{
		return zanac::MAmidiMEmo::Program::HasExited();
	}

	__declspec(dllexport) void __stdcall VstStarted()
	{
		return zanac::MAmidiMEmo::Program::VstStarted();
	}

	__declspec(dllexport) void __stdcall SoundUpdating()
	{
		zanac::MAmidiMEmo::Program::SoundUpdating();
	}

	__declspec(dllexport) void __stdcall SoundUpdated()
	{
		zanac::MAmidiMEmo::Program::SoundUpdated();
	}

	__declspec(dllexport) void __stdcall RestartApplication()
	{
		zanac::MAmidiMEmo::Program::RestartApplication();
	}

	int vstiMode = 0;

	__declspec(dllexport) int __stdcall IsVSTiMode()
	{
		return vstiMode;
	}

	__declspec(dllexport) void __stdcall SetVSTiMode()
	{
		vstiMode = 1;
		zanac::MAmidiMEmo::Program::SetVSTiMode();
	}

	__declspec(dllexport) void __stdcall SendMidiEvent(LONG64 eventID, LONG64 frameId, unsigned char data1, unsigned char data2, unsigned char data3)
	{
		zanac::MAmidiMEmo::Midi::MidiManager::MidiEventReceived(
			zanac::MAmidiMEmo::Midi::MidiPort::PortA, eventID, frameId, data1, data2, data3);
	}

	__declspec(dllexport) void __stdcall SendMidiEvents(LONG64 eventID, LONG64 frameId, unsigned char *data1, unsigned char *data2, unsigned char *data3, int length)
	{
		zanac::MAmidiMEmo::Midi::MidiManager::MidiEventsReceived(
			zanac::MAmidiMEmo::Midi::MidiPort::PortA, eventID, frameId, data1, data2, data3, length);
	}

	__declspec(dllexport) void __stdcall SendMidiSysEvent(LONG64 eventID, LONG64 frameId, unsigned char *data, int length)
	{
		zanac::MAmidiMEmo::Midi::MidiManager::MidiSysEventReceived(
			zanac::MAmidiMEmo::Midi::MidiPort::PortA, eventID, frameId, data, length);
	}

	__declspec(dllexport) void __stdcall CloseApplication()
	{
		zanac::MAmidiMEmo::Program::CloseApplication();
	}

	__declspec(dllexport) void __stdcall LoadData(unsigned char * data, int length)
	{
		zanac::MAmidiMEmo::Program::LoadData(data, length);
	}

	__declspec(dllexport) int __stdcall SaveData(void** saveBuf)
	{
		return zanac::MAmidiMEmo::Program::SaveData(saveBuf);
	}

	__declspec(dllexport) void __stdcall SoundTimerCallback()
	{
		zanac::MAmidiMEmo::Instruments::HighPrecisionTimer::SoundTimerCallback();
	}

	__declspec(dllexport) void __stdcall DirectAccessToChip(unsigned char device_id, unsigned char unit, unsigned int address, unsigned int data)
	{
		zanac::MAmidiMEmo::Instruments::InstrumentManager::DirectAccessToChip(device_id, unit, address, data);
	}

}

