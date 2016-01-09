#pragma once
#include "RC_Helpers.h"

extern "C"
{
#include <string.h>
}

using namespace Platform;

namespace ImageBlurRuntime
{
	class RC_Helpers
	{
	public:
		static char* ConvertStringToChar(String^ str);
		static String^ ConvertCharToString(const char *ch);
		static wchar_t* ConvertCharToWchar_t(const char* stringToConvert);
		static String^ ConvertCharToCxString(const char* stringToConvert);

		//static void charTowchar(const char *chr, wchar_t *wchar, int size);
		//static void wcharTochar(const wchar_t *wchar, char *chr, int length); 
	};
}
