using System.Runtime.InteropServices;
using CoreGraphics;
using UIKit;

static class CFunctions
{
	// extern UIImage * resizedImage (UIImage *inImage, CGRect thumbRect);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern UIImage resizedImage (UIImage inImage, CGRect thumbRect);
}

[StructLayout (LayoutKind.Sequential)]
public struct BEngine
{
	public unsafe void* pBcrEngine;

	public sbyte[] pBuffer;
}

[StructLayout (LayoutKind.Sequential)]
public struct TRect
{
	public short lx;

	public short ly;

	public short rx;

	public short ry;
}

[StructLayout (LayoutKind.Sequential)]
public struct BRect
{
	public short lx;

	public short ly;

	public short rx;

	public short ry;
}

[StructLayout (LayoutKind.Sequential)]
public struct TImage
{
	public short width;

	public short height;

	public short xres;

	public short yres;

	public unsafe byte** pixels;

	public ushort type;

	public byte status;

	public byte key;

	public short x1;

	public short x2;

	public short y1;

	public short y2;

	public TRect CropRect;

	public unsafe void* ptr;

	public unsafe T_MastImage* secImage;

	public byte[] msk;
}

[StructLayout (LayoutKind.Sequential)]
public struct BField
{
	public short fid;

	public short blocktype;

	public short mem;

	public unsafe sbyte* text;

	public int conf;

	public int addconf;

	public BRect rect;

	public BRect rectint;

	public short label;

	public short size;

	public sbyte[] reserve;

	public unsafe B_Field* child;

	public unsafe B_Field* prev;

	public unsafe B_Field* next;
}

public enum OCR_Language : uint
{
	Nil = 0,
	English = 1,
	Chinese = 2,
	European = 3,
	Russian = 4,
	Tamil = 5,
	Japan = 6,
	Centeuro = 7,
	Korea = 8,
	Turkish = 9,
	MixedEC = 16,
	Max = 18
}

public enum IDCard : uint
{
	Non,
	cardNo,
	cardName,
	addr,
	birthday,
	sex
}
