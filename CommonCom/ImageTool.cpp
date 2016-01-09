// ImageBlurRuntime.cpp
#include "pch.h"
#include "ImageTool.h"
#include "../CommonLib/FastBlur.h"

using namespace CommonCom;
using namespace Platform;

ImageOperation::ImageOperation()
{
}

int ImageOperation::PlusArray(const Array<int, 1>^ arr0, Array<int, 1>^* arr1)
{
	int sum = 0;
	sum = arr0[0];
	//for (size_t i = 0; i < arr0->Length; i++)
	//{
	//	sum += arr0[i];
	//}
	auto temp = ref new Array<int, 1>(2);
	temp[0] = 1;
	temp[1] = 2;
	*arr1 = temp;
	return sum;
}

Array<int, 1>^ ImageOperation::RetundArray()
{
	return ref new Array<int, 1>(5) { 1, 2, 3, 4, 5 };
}

/*************************************************
Function:		StackBlur(堆栈模糊)
Description:    使用堆栈方式进行图片像素模糊处理
Calls:          malloc
Table Accessed: NULL
Table Updated:	NULL
Input:          像素点集合，图片宽，图片高，模糊半径
Output:         返回模糊后的像素点集合
Return:         返回模糊后的像素点集合
Others:         NULL
*************************************************/
Array<int, 1>^ ImageOperation::BlurCompute(const Array<int, 1>^ arr0, int w, int h, int radius)
{
	int length = arr0->Length;
	int *b = new int[length];
	for (size_t i = 0; i < length; i++)
	{
		b[i] = arr0[i];
	}

	StackBlur(b, w, h, radius);
	Array<int, 1>^ newArr = ref new Array<int, 1>(length);
	for (size_t i = 0; i < length; i++)
	{
		newArr[i] = b[i];
	}

	delete[]b;
	return newArr;
}

/*************************************************
Function:		StackBlur(堆栈模糊)
Description:    根据起始坐标，以及宽高，模糊半径对图片像素模糊处理
Input:          像素点集合，图片宽高，需要处理的起始位置X,Y,图片宽，高，模糊半径
Return:         返回模糊后的像素点集合, 0：参数错误，
Others:         NULL
*************************************************/
Array<int, 1>^ ImageOperation::BlurCompute(const Array<int, 1>^ arr0, int imgw, int imgh, int x, int y, int w, int h, int radius)
{
	if (w > imgw || h > imgh)
	{
		return ref new Array<int, 1>(1) { 0 };
	}

	int length = w * h;
	int *b = new int[length];
	for (size_t i = 0; i < h; i++)
	{
		for (size_t j = 0; j < w; j++)
		{
			b[w * i + j] = arr0[imgw * (y + i) + j + x];
		}
	}

	StackBlur(b, w, h, radius);
	Array<int, 1>^ newArr = ref new Array<int, 1>(arr0->Length);
	//for (size_t i = 0; i < h; i++)
	//{
	//	for (size_t j = 0; j < w; j++)
	//	{
	//		newArr[imgw * (y + i) + j + x] = b[w * i + j];
	//	}
	//}
	for (size_t i = 0; i < length; i++)
	{
		newArr[i] = b[i];
	}

	delete[]b;
	return newArr;
}


int *pixs;
int len;
int width;
int height;

int ImageOperation::PutPixs(const Array<int, 1>^ arr0, int w, int h)
{
	width = w;
	height = h;
	len = arr0->Length;

	int *b = new int[len];
	for (size_t i = 0; i < len; i++)
	{
		b[i] = arr0[i];
	}

	pixs = b;
	return 1;
}

Array<int, 1>^ ImageOperation::BlurPixs(int radius)
{
	int *newPixs = new int[len];
	for (size_t i = 0; i < len; i++)
	{
		newPixs[i] = pixs[i];
	}
	StackBlur(newPixs, width, height, radius);
	Array<int, 1>^ newArr = ref new Array<int, 1>(len);
	for (size_t i = 0; i < len; i++)
	{
		newArr[i] = newPixs[i];
	}
	delete[]newPixs;
	return newArr;
}


int ImageOperation::ChangeLight(int v)
{
	return 1;
}

int ImageOperation::ChangeColor(int h)
{
	return 1;
}

int ImageOperation::DeletePixs()
{
	delete[]pixs;
	return 1;
}

void ImageOperation::StackBlur(int* pix, int w, int h, int radius) {
	BlurCoumpute(pix, w, h, radius);

}