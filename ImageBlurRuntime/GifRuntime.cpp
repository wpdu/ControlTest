#include "pch.h"
#include "RC_Helpers.h"
#include "GifRuntime.h"
#include "../BlurLib/GifLib.h"

using namespace Platform;
using namespace ImageBlurRuntime;

std::string wstos(std::wstring ws)
{
	std::string s;
	s.assign(ws.begin(), ws.end());
	return s;
}

GifRuntime::GifRuntime()
{
	gifw = new GifWriter();
}

bool GifRuntime::GifBeginRT(Platform::String^ name, int width, int height, int delay)
{
	//char *cname = RC_Helpers::ConvertStringToChar(name);
	std::string sname = wstos(std::wstring(name->Data()));
	const char* cname = sname.c_str();
	return GifBegin(gifw, cname, width, height, delay, 8, false);
}

bool GifRuntime::GifWriteFrameRT(Windows::Storage::Streams::DataReader^ dr, int width, int height, int delay)
{
	unsigned int len = width * height * 4;
	uint8 *image = new uint8[len];

	dr->ReadBytes(Platform::ArrayReference<uint8>(image, len));

	return GifWriteFrame(gifw, image, width, height, delay, 8, false);
}

bool GifRuntime::GifEndRT()
{
	//delete gifw;
	return GifEnd(gifw);
}

