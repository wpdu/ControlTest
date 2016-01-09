#include "pch.h"
#include "FastBlur.h"


int Multiplication(int i, int j)
{
	int calc = i * j;

	return calc;
}


/*************************************************
Copyright:  Copyright QIUJUER 2013.
Author:		Qiujuer
Date:		2014-04-18
Description:实现图片模糊处理
**************************************************/
#include<malloc.h>

#define ABS(a) ((a)<(0)?(-a):(a))
#define MAX(a,b) ((a)>(b)?(a):(b))
#define MIN(a,b) ((a)<(b)?(a):(b))

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
int BlurCoumpute(int* pix, int w, int h, int radius)
{

	int wm = w - 1;
	int hm = h - 1;
	int wh = w * h;
	int div = radius + radius + 1;

	int *r = (int *)malloc(wh * sizeof(int));
	int *g = (int *)malloc(wh * sizeof(int));
	int *b = (int *)malloc(wh * sizeof(int));
	int rsum, gsum, bsum, x, y, i, p, yp, yi, yw;

	int *vmin = (int *)malloc(MAX(w, h) * sizeof(int));

	int divsum = (div + 1) >> 1;
	divsum *= divsum;
	int *dv = (int *)malloc(256 * divsum * sizeof(int));
	for (i = 0; i < 256 * divsum; i++) {
		dv[i] = (i / divsum);
	}

	yw = yi = 0;

	int(*stack)[3] = (int(*)[3])malloc(div * 3 * sizeof(int));
	int stackpointer;
	int stackstart;
	int *sir;
	int rbs;
	int r1 = radius + 1;
	int routsum, goutsum, boutsum;
	int rinsum, ginsum, binsum;

	for (y = 0; y < h; y++) {
		rinsum = ginsum = binsum = routsum = goutsum = boutsum = rsum = gsum = bsum = 0;
		for (i = -radius; i <= radius; i++) {
			p = pix[yi + (MIN(wm, MAX(i, 0)))];
			sir = stack[i + radius];
			sir[0] = (p & 0xff0000) >> 16;
			sir[1] = (p & 0x00ff00) >> 8;
			sir[2] = (p & 0x0000ff);

			rbs = r1 - ABS(i);
			rsum += sir[0] * rbs;
			gsum += sir[1] * rbs;
			bsum += sir[2] * rbs;
			if (i > 0) {
				rinsum += sir[0];
				ginsum += sir[1];
				binsum += sir[2];
			}
			else {
				routsum += sir[0];
				goutsum += sir[1];
				boutsum += sir[2];
			}
		}
		stackpointer = radius;

		for (x = 0; x < w; x++) {

			r[yi] = dv[rsum];
			g[yi] = dv[gsum];
			b[yi] = dv[bsum];

			rsum -= routsum;
			gsum -= goutsum;
			bsum -= boutsum;

			stackstart = stackpointer - radius + div;
			sir = stack[stackstart % div];

			routsum -= sir[0];
			goutsum -= sir[1];
			boutsum -= sir[2];

			if (y == 0) {
				vmin[x] = MIN(x + radius + 1, wm);
			}
			p = pix[yw + vmin[x]];

			sir[0] = (p & 0xff0000) >> 16;
			sir[1] = (p & 0x00ff00) >> 8;
			sir[2] = (p & 0x0000ff);

			rinsum += sir[0];
			ginsum += sir[1];
			binsum += sir[2];

			rsum += rinsum;
			gsum += ginsum;
			bsum += binsum;

			stackpointer = (stackpointer + 1) % div;
			sir = stack[(stackpointer) % div];

			routsum += sir[0];
			goutsum += sir[1];
			boutsum += sir[2];

			rinsum -= sir[0];
			ginsum -= sir[1];
			binsum -= sir[2];

			yi++;
		}
		yw += w;
	}
	for (x = 0; x < w; x++) {
		rinsum = ginsum = binsum = routsum = goutsum = boutsum = rsum = gsum = bsum = 0;
		yp = -radius * w;
		for (i = -radius; i <= radius; i++) {
			yi = MAX(0, yp) + x;

			sir = stack[i + radius];

			sir[0] = r[yi];
			sir[1] = g[yi];
			sir[2] = b[yi];

			rbs = r1 - ABS(i);

			rsum += r[yi] * rbs;
			gsum += g[yi] * rbs;
			bsum += b[yi] * rbs;

			if (i > 0) {
				rinsum += sir[0];
				ginsum += sir[1];
				binsum += sir[2];
			}
			else {
				routsum += sir[0];
				goutsum += sir[1];
				boutsum += sir[2];
			}

			if (i < hm) {
				yp += w;
			}
		}
		yi = x;
		stackpointer = radius;
		for (y = 0; y < h; y++) {
			// Preserve alpha channel: ( 0xff000000 & pix[yi] )
			pix[yi] = (0xff000000 & pix[yi]) | (dv[rsum] << 16) | (dv[gsum] << 8) | dv[bsum];

			rsum -= routsum;
			gsum -= goutsum;
			bsum -= boutsum;

			stackstart = stackpointer - radius + div;
			sir = stack[stackstart % div];

			routsum -= sir[0];
			goutsum -= sir[1];
			boutsum -= sir[2];

			if (x == 0) {
				vmin[y] = MIN(y + r1, hm) * w;
			}
			p = x + vmin[y];

			sir[0] = r[p];
			sir[1] = g[p];
			sir[2] = b[p];

			rinsum += sir[0];
			ginsum += sir[1];
			binsum += sir[2];

			rsum += rinsum;
			gsum += ginsum;
			bsum += binsum;

			stackpointer = (stackpointer + 1) % div;
			sir = stack[stackpointer];

			routsum += sir[0];
			goutsum += sir[1];
			boutsum += sir[2];

			rinsum -= sir[0];
			ginsum -= sir[1];
			binsum -= sir[2];

			yi += w;
		}
	}

	free(r);
	free(g);
	free(b);
	free(vmin);
	free(dv);
	free(stack);

	//return pix;
	return 1;
}

#include<math.h>

int Rgb2Hsv(float R, float G, float B, float& H, float& S, float&V)
{
	// r,g,b values are from 0 to 1
	// h = [0,360], s = [0,1], v = [0,1]
	// if s == 0, then h = -1 (undefined)

	float min, max, delta, tmp;
	tmp = min(R, G);
	min = min(tmp, B);
	tmp = max(R, G);
	max = max(tmp, B);
	V = max; // v

	delta = max - min;

	if (max != 0)
		S = delta / max; // s
	else
	{
		// r = g = b = 0 // s = 0, v is undefined
		S = 0;
		H = -1;
		return 0;
	}
	if (R == max)
		H = (G - B) / delta; // between yellow & magenta
	else if (G == max)
		H = 2 + (B - R) / delta; // between cyan & yellow
	else
		H = 4 + (R - G) / delta; // between magenta & cyan

	H *= 60; // degrees
	if (H < 0)
		H += 360;
	return 1;
}

int Hsv2Rgb(float H, float S, float V, float &R, float &G, float &B)
{
	int i;
	float f, p, q, t;

	if (S == 0)
	{
		// achromatic (grey)
		R = G = B = V;
		return 0;
	}

	H /= 60; // sector 0 to 5
	i = floor(H);
	f = H - i; // factorial part of h
	p = V * (1 - S);
	q = V * (1 - S * f);
	t = V * (1 - S * (1 - f));

	switch (i)
	{
	case 0:
		R = V;
		G = t;
		B = p;
		break;
	case 1:
		R = q;
		G = V;
		B = p;
		break;
	case 2:
		R = p;
		G = V;
		B = t;
		break;
	case 3:
		R = p;
		G = q;
		B = V;
		break;
	case 4:
		R = t;
		G = p;
		B = V;
		break;
	default: // case 5:
		R = V;
		G = p;
		B = q;
	}
	return 1;
}

int ToLight(float R, float G, float B, int light, float &oR, float &oG, float &oB)
{

	return 1;
}

int ToColor(float R, float G, float B, int light, float &oR, float &oG, float &oB)
{
	return 1;
}
