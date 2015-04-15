/*
Example Using Visual Studio 2008, C++ CLR Console Application
This example uses the OptoMMP .Net SDK, version 3.0.
*/
#include "stdafx.h"
#include "windows.h"

using namespace System;
using namespace Opto22::OptoMMP3;

int main(array<System::String ^> ^args)
{
	int i32Result;
	OptoMMP objMmp;

	i32Result = objMmp.Open("10.192.54.46", 2001, OptoMMP::Connection::UdpIp,3000,true);
	if(i32Result < 0)
	{
		Console::WriteLine(L"i32Result is {0}",i32Result);
	}

	///////////////////////////////
	// Disable Read Caching
	objMmp.ReadCacheFreshnessMs = 0;

	// configure the IO first!

	///////////////////////////////
	// Set Temperature to F
	objMmp.WriteFOrCStatus(true);

	///////////////////////////////
	// configure the 4 points of module 1 as outputs (it is a SNAP-ODC5-SRC)
	i32Result = objMmp.WriteDigitalPointConfiguration64(objMmp.GetPointNumberFor64(1, 0), true, OptoMMP::eDigitalFeature::None, false, false, "Mod1_Pt0");
	if(i32Result < 0)
	{
		Console::WriteLine(L"i32Result is {0}", i32Result);
	}
	i32Result = objMmp.WriteDigitalPointConfiguration64(objMmp.GetPointNumberFor64(1, 1), true, OptoMMP::eDigitalFeature::None, false, false, "Mod1_Pt1");
	if(i32Result < 0)
	{
		Console::WriteLine(L"i32Result is {0}", i32Result);
	}
	i32Result = objMmp.WriteDigitalPointConfiguration64(objMmp.GetPointNumberFor64(1, 2), true, OptoMMP::eDigitalFeature::None, false, false, "Mod1_Pt2");
	if(i32Result < 0)
	{
		Console::WriteLine(L"i32Result is {0}", i32Result);
	}
	i32Result = objMmp.WriteDigitalPointConfiguration64(objMmp.GetPointNumberFor64(1, 3), true, OptoMMP::eDigitalFeature::None, false, false, "Mod1_Pt3");
	if(i32Result < 0)
	{
		Console::WriteLine(L"i32Result is {0}", i32Result);
	}

	///////////////////////////////
	// configure the 8 points of a SNAP-AITM-8, point type 5 is a J Thermocouple
	i32Result =objMmp.WriteAnalogPointConfiguration4096(objMmp.GetPointNumberFor4096(5, 0), 5);
	if(i32Result < 0)
	{
		Console::WriteLine(L"i32Result is {0}", i32Result);
	}
	i32Result =objMmp.WriteAnalogPointConfiguration4096(objMmp.GetPointNumberFor4096(5, 1), 5);
	if(i32Result < 0)
	{
		Console::WriteLine(L"i32Result is {0}", i32Result);
	}
	i32Result =objMmp.WriteAnalogPointConfiguration4096(objMmp.GetPointNumberFor4096(5, 2), 5);
	if(i32Result < 0)
	{
		Console::WriteLine(L"i32Result is {0}", i32Result);
	}
	i32Result =objMmp.WriteAnalogPointConfiguration4096(objMmp.GetPointNumberFor4096(5, 3), 5);
	if(i32Result < 0)
	{
		Console::WriteLine(L"i32Result is {0}", i32Result);
	}
	i32Result =objMmp.WriteAnalogPointConfiguration4096(objMmp.GetPointNumberFor4096(5, 4), 5);
	if(i32Result < 0)
	{
		Console::WriteLine(L"i32Result is {0}", i32Result);
	}
	i32Result =objMmp.WriteAnalogPointConfiguration4096(objMmp.GetPointNumberFor4096(5, 5), 5);
	if(i32Result < 0)
	{
		Console::WriteLine(L"i32Result is {0}", i32Result);
	}
	i32Result =objMmp.WriteAnalogPointConfiguration4096(objMmp.GetPointNumberFor4096(5, 6), 5);
	if(i32Result < 0)
	{
		Console::WriteLine(L"i32Result is {0}", i32Result);
	}
	i32Result =objMmp.WriteAnalogPointConfiguration4096(objMmp.GetPointNumberFor4096(5, 7) ,5);
	if(i32Result < 0)
	{
		Console::WriteLine(L"i32Result is {0}", i32Result);
	}

	// loop to read and write digital and analog states as banks
	int index = 0;
	while (true)
	{
		//////////////////////////
		// read digital states (will report inputs and outputs)
		__int64 i64StateBitMask = 0;
		i32Result = objMmp.ReadDigitalStates64(i64StateBitMask);
		if(i32Result < 0)
		{
			Console::WriteLine(L"i32Result is {0}", i32Result);
		}
		else
		{
			Console::WriteLine(L"i32Result is {0:X}", i64StateBitMask);
		}

		//////////////////////////
		// write digital states as a bank
		i64StateBitMask = (__int64)0x01 << (index % 3 + 5);
		Console::WriteLine(L"Writing Digital-64 Bank {0:x}", i64StateBitMask);
		i32Result = objMmp.WriteDigitalStates64(i64StateBitMask);
		if(i32Result < 0)
		{
			Console::WriteLine(L"i32Result is {0}", i32Result);
		}
		index++;

		//////////////////////////
		// read analog inputs, this reads all analog inputs
		array<float> ^f32aryValues = gcnew array<float>(512);
		i32Result = objMmp.ReadAnalogEus512(f32aryValues, 0);
		if(i32Result < 0)
		{
			Console::WriteLine(L"i32Result is {0}",i32Result);
		}

		// write the results from the bank read
		Console::WriteLine(L"AITM-8 Channel 0 is {0}", f32aryValues[objMmp.GetPointNumberFor512(5, 0)]);
		Console::WriteLine(L"AITM-8 Channel 1 is {0}", f32aryValues[objMmp.GetPointNumberFor512(5, 1)]);
		Console::WriteLine(L"AITM-8 Channel 2 is {0}", f32aryValues[objMmp.GetPointNumberFor512(5, 2)]);
		Console::WriteLine(L"AITM-8 Channel 3 is {0}", f32aryValues[objMmp.GetPointNumberFor512(5, 3)]);
		Console::WriteLine(L"AITM-8 Channel 4 is {0}", f32aryValues[objMmp.GetPointNumberFor512(5, 4)]);
		Console::WriteLine(L"AITM-8 Channel 5 is {0}", f32aryValues[objMmp.GetPointNumberFor512(5, 5)]);
		Console::WriteLine(L"AITM-8 Channel 6 is {0}", f32aryValues[objMmp.GetPointNumberFor512(5, 6)]);
		Console::WriteLine(L"AITM-8 Channel 7 is {0}", f32aryValues[objMmp.GetPointNumberFor512(5, 7)]);

		//////////////////////////
		// write analog outputs
		f32aryValues[objMmp.GetPointNumberFor64(2, 0)] = (float)(index);
		Console::WriteLine(L"Writing Analog Output");
		i32Result = objMmp.WriteAnalogStates64(f32aryValues, true, 0);
		if(i32Result < 0)
		{
			Console::WriteLine(L"i32Result is {0}", i32Result);
		}

		// so us people can see the states change on the computer screen or on the I/O
		Sleep(1000);
	}
	return 0;
}
