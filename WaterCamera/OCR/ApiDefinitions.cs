using System;
using CoreGraphics;
using Foundation;
using UIKit;

// @interface Utility : NSObject
[BaseType (typeof(NSObject))]
interface Utility
{
	// +(void)warningAlertWithMessage:(NSString *)mes;
	[Static]
	[Export ("warningAlertWithMessage:")]
	void WarningAlertWithMessage (string mes);

	// +(void)informationAlertWithMessage:(NSString *)mes;
	[Static]
	[Export ("informationAlertWithMessage:")]
	void InformationAlertWithMessage (string mes);

	// +(CGContextRef)CreateARGBBitmapContext:(CGImageRef)image;
	[Static]
	[Export ("CreateARGBBitmapContext:")]
	unsafe CGContextRef* CreateARGBBitmapContext (CGImageRef* image);

	// +(BOOL)GetYDataFromImage:(UIImage *)image pixels:(unsigned char **)pixels;
	[Static]
	[Export ("GetYDataFromImage:pixels:")]
	unsafe bool GetYDataFromImage (UIImage image, byte** pixels);

	// +(int)SaveImageBMP:(NSString *)filename data:(unsigned char **)pYDataBuf width:(int)width height:(int)height;
	[Static]
	[Export ("SaveImageBMP:data:width:height:")]
	unsafe int SaveImageBMP (string filename, byte** pYDataBuf, int width, int height);

	// +(CGFloat)distanceBetweenPoints:(CGPoint)first toPoint:(CGPoint)second;
	[Static]
	[Export ("distanceBetweenPoints:toPoint:")]
	nfloat DistanceBetweenPoints (CGPoint first, CGPoint second);

	// +(UIImage *)createRoundedRectImage:(UIImage *)image size:(CGSize)size;
	[Static]
	[Export ("createRoundedRectImage:size:")]
	UIImage CreateRoundedRectImage (UIImage image, CGSize size);
}

// @protocol BcrResultCallbackDelegate <NSObject>
[Protocol, Model]
[BaseType (typeof(NSObject))]
interface BcrResultCallbackDelegate
{
	// @required -(void)bcrResultCallbackWithValue:(NSInteger)value;
	[Abstract]
	[Export ("bcrResultCallbackWithValue:")]
	void BcrResultCallbackWithValue (nint value);
}

// @interface YMIDCardEngine : NSObject
[BaseType (typeof(NSObject))]
interface YMIDCardEngine
{
	// @property (assign, nonatomic) NSInteger ocrLanguage;
	[Export ("ocrLanguage")]
	nint OcrLanguage { get; set; }

	// @property (nonatomic) BOOL initSuccess;
	[Export ("initSuccess")]
	bool InitSuccess { get; set; }

	// @property (assign, nonatomic) NSString * chNumberStr;
	[Export ("chNumberStr")]
	string ChNumberStr { get; set; }

	// -(id)initWithLanguage:(NSInteger)language andChannelNumber:(NSString *)channelNumberStr;
	[Export ("initWithLanguage:andChannelNumber:")]
	IntPtr Constructor (nint language, string channelNumberStr);

	// -(BOOL)allocBImage:(UIImage *)image;
	[Export ("allocBImage:")]
	bool AllocBImage (UIImage image);

	// -(BOOL)allocVideoBImage:(UIImage *)image;
	[Export ("allocVideoBImage:")]
	bool AllocVideoBImage (UIImage image);

	// -(void)freeBImage;
	[Export ("freeBImage")]
	void FreeBImage ();

	// -(BOOL)rotateBImage;
	[Export ("rotateBImage")]
	[Verify (MethodToProperty)]
	bool RotateBImage { get; }

	// -(CGRect)charDetection:(CGPoint)firstPoint lastPoint:(CGPoint)lastPoint;
	[Export ("charDetection:lastPoint:")]
	CGRect CharDetection (CGPoint firstPoint, CGPoint lastPoint);

	// -(NSDictionary *)doBCRWithRect:(CGRect)rect;
	[Export ("doBCRWithRect:")]
	NSDictionary DoBCRWithRect (CGRect rect);

	// -(UIImage *)getIDCardImage;
	[Export ("getIDCardImage")]
	[Verify (MethodToProperty)]
	UIImage IDCardImage { get; }

	// -(UIImage *)getHeadImage;
	[Export ("getHeadImage")]
	[Verify (MethodToProperty)]
	UIImage HeadImage { get; }

	// -(NSArray *)doBCR;
	[Export ("doBCR")]
	[Verify (MethodToProperty), Verify (StronglyTypedNSArray)]
	NSObject[] DoBCR { get; }

	// -(void)setProgressCallbackDelegate:(id<BCRProgressCallBackDelegate>)delegate;
	[Export ("setProgressCallbackDelegate:")]
	void SetProgressCallbackDelegate (BCRProgressCallBackDelegate @delegate);

	// -(void)progressCancel;
	[Export ("progressCancel")]
	void ProgressCancel ();

	// -(int)doBcrRecognizeVedioWith:(int)width andHeight:(int)height andRect:(BRect)pRect andChannelNumberStr:(NSString *)channelNumberStr;
	[Export ("doBcrRecognizeVedioWith:andHeight:andRect:andChannelNumberStr:")]
	int DoBcrRecognizeVedioWith (int width, int height, BRect pRect, string channelNumberStr);

	// -(void)setBcrResultCallbackDelegate:(id)delegate;
	[Export ("setBcrResultCallbackDelegate:")]
	void SetBcrResultCallbackDelegate (NSObject @delegate);
}

// @protocol BCRProgressCallBackDelegate <NSObject>
[Protocol, Model]
[BaseType (typeof(NSObject))]
interface BCRProgressCallBackDelegate
{
	// @required -(void)progressCallbackWithValue:(NSInteger)value;
	[Abstract]
	[Export ("progressCallbackWithValue:")]
	void ProgressCallbackWithValue (nint value);

	// @required -(void)progressStop;
	[Abstract]
	[Export ("progressStop")]
	void ProgressStop ();
}
