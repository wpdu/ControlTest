#pragma once

extern "C"

int _declspec(dllexport)Multiplication(int i, int j);

// 
int _declspec(dllexport)BlurCoumpute(int* pix, int w, int h, int radius);

int _declspec(dllexport)Rgb2Hsv(float R, float G, float B, float& H, float& S, float&V);


int _declspec(dllexport)Hsv2Rgb(float H, float S, float V, float &R, float &G, float &B);

int _declspec(dllexport)ToLight(float R, float G, float B, int light, float &oR, float &oG, float &oB);

int _declspec(dllexport)ToColor(float R, float G, float B, int light, float &oR, float &oG, float &oB);

