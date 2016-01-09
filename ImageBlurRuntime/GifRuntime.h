#pragma once

#include <vector>
#include <collection.h>
#include <../BlurLib/GifLib.h>

using namespace Platform;
using namespace std;
using namespace Platform::Collections;

namespace ImageBlurRuntime
{
	public ref class GifRuntime sealed
	{
	private:
		GifWriter *gifw;

	public :
		GifRuntime();

		bool GifBeginRT(Platform::String^ name, int width, int height, int delay);

		bool GifWriteFrameRT(Windows::Storage::Streams::DataReader^ dr, int width, int height, int delay);

		bool GifEndRT();

	};
}
