#pragma once


#include <vector>
#include <collection.h>
#include <../CommonLib/FastBlur.h>

using namespace Platform;
using namespace std;
using namespace Platform::Collections;

namespace CommonCom
{
	public ref class ImageOperation sealed
	{

	public:
		ImageOperation();

		int ImageOperation::PlusArray(const Array<int, 1>^ arr0, Array<int, 1>^* arr1);

		Array<int, 1>^ ImageOperation::RetundArray();

		Array<int, 1>^ ImageOperation::BlurCompute(const Array<int, 1>^ arr0, int w, int h, int radius);

		Array<int, 1>^ ImageOperation::BlurCompute(const Array<int, 1>^ arr0, int imgw, int imgh, int x, int y, int w, int h, int radius);

		int ImageOperation::PutPixs(const Array<int, 1>^ arr0, int w, int h);

		Array<int, 1>^ ImageOperation::BlurPixs(int radius);

		int ImageOperation::ChangeLight(int v);

		int ImageOperation::ChangeColor(int h);

	    int ImageOperation::DeletePixs();

		void StackBlur(int* pix, int w, int h, int radius);


	};
}