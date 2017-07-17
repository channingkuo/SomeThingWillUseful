//
// Auto-generated from generator.cs, do not edit
//
// We keep references to objects, so warning 414 is expected

#pragma warning disable 414

using System;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using UIKit;
using GLKit;
using Metal;
using MapKit;
using ModelIO;
using SceneKit;
using Security;
using AudioUnit;
using CoreVideo;
using CoreMedia;
using QuickLook;
using Foundation;
using CoreMotion;
using ObjCRuntime;
using AddressBook;
using CoreGraphics;
using CoreLocation;
using AVFoundation;
using NewsstandKit;
using CoreAnimation;
using CoreFoundation;

namespace OCR {
	[Register("YMIDCardEngine", true)]
	public unsafe partial class YMIDCardEngine : NSObject {
		
		[CompilerGenerated]
		static readonly IntPtr class_ptr = Class.GetHandle ("YMIDCardEngine");
		
		public override IntPtr ClassHandle { get { return class_ptr; } }
		
		[CompilerGenerated]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("init")]
		public YMIDCardEngine () : base (NSObjectFlag.Empty)
		{
			IsDirectBinding = GetType ().Assembly == global::ApiDefinition.Messaging.this_assembly;
			if (IsDirectBinding) {
				InitializeHandle (global::ApiDefinition.Messaging.IntPtr_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle ("init")), "init");
			} else {
				InitializeHandle (global::ApiDefinition.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle ("init")), "init");
			}
		}

		[CompilerGenerated]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		protected YMIDCardEngine (NSObjectFlag t) : base (t)
		{
			IsDirectBinding = GetType ().Assembly == global::ApiDefinition.Messaging.this_assembly;
		}

		[CompilerGenerated]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		protected internal YMIDCardEngine (IntPtr handle) : base (handle)
		{
			IsDirectBinding = GetType ().Assembly == global::ApiDefinition.Messaging.this_assembly;
		}

		[Export ("initWithLanguage:andChannelNumber:")]
		[CompilerGenerated]
		public YMIDCardEngine (global::System.nint language, string channelNumberStr)
			: base (NSObjectFlag.Empty)
		{
			if (channelNumberStr == null)
				throw new ArgumentNullException ("channelNumberStr");
			var nschannelNumberStr = NSString.CreateNative (channelNumberStr);
			
			IsDirectBinding = GetType ().Assembly == global::ApiDefinition.Messaging.this_assembly;
			if (IsDirectBinding) {
				InitializeHandle (global::ApiDefinition.Messaging.IntPtr_objc_msgSend_nint_IntPtr (this.Handle, Selector.GetHandle ("initWithLanguage:andChannelNumber:"), language, nschannelNumberStr), "initWithLanguage:andChannelNumber:");
			} else {
				InitializeHandle (global::ApiDefinition.Messaging.IntPtr_objc_msgSendSuper_nint_IntPtr (this.SuperHandle, Selector.GetHandle ("initWithLanguage:andChannelNumber:"), language, nschannelNumberStr), "initWithLanguage:andChannelNumber:");
			}
			NSString.ReleaseNative (nschannelNumberStr);
			
		}
		
		[Export ("allocBImage:")]
		[CompilerGenerated]
		public virtual bool AllocBImage (global::UIKit.UIImage image)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (IsDirectBinding) {
				return global::ApiDefinition.Messaging.bool_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle ("allocBImage:"), image.Handle);
			} else {
				return global::ApiDefinition.Messaging.bool_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.GetHandle ("allocBImage:"), image.Handle);
			}
		}
		
		[Export ("allocVideoBImage:")]
		[CompilerGenerated]
		public virtual bool AllocVideoBImage (global::UIKit.UIImage image)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (IsDirectBinding) {
				return global::ApiDefinition.Messaging.bool_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle ("allocVideoBImage:"), image.Handle);
			} else {
				return global::ApiDefinition.Messaging.bool_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.GetHandle ("allocVideoBImage:"), image.Handle);
			}
		}
		
		[Export ("charDetection:lastPoint:")]
		[CompilerGenerated]
		public virtual CGRect CharDetection (CGPoint firstPoint, CGPoint lastPoint)
		{
			CGRect ret;
			if (IsDirectBinding) {
				if (Runtime.Arch == Arch.DEVICE) {
					if (IntPtr.Size == 8) {
						ret = global::ApiDefinition.Messaging.CGRect_objc_msgSend_CGPoint_CGPoint (this.Handle, Selector.GetHandle ("charDetection:lastPoint:"), firstPoint, lastPoint);
					} else {
						global::ApiDefinition.Messaging.CGRect_objc_msgSend_stret_CGPoint_CGPoint (out ret, this.Handle, Selector.GetHandle ("charDetection:lastPoint:"), firstPoint, lastPoint);
					}
				} else if (IntPtr.Size == 8) {
					global::ApiDefinition.Messaging.CGRect_objc_msgSend_stret_CGPoint_CGPoint (out ret, this.Handle, Selector.GetHandle ("charDetection:lastPoint:"), firstPoint, lastPoint);
				} else {
					global::ApiDefinition.Messaging.CGRect_objc_msgSend_stret_CGPoint_CGPoint (out ret, this.Handle, Selector.GetHandle ("charDetection:lastPoint:"), firstPoint, lastPoint);
				}
			} else {
				if (Runtime.Arch == Arch.DEVICE) {
					if (IntPtr.Size == 8) {
						ret = global::ApiDefinition.Messaging.CGRect_objc_msgSendSuper_CGPoint_CGPoint (this.SuperHandle, Selector.GetHandle ("charDetection:lastPoint:"), firstPoint, lastPoint);
					} else {
						global::ApiDefinition.Messaging.CGRect_objc_msgSendSuper_stret_CGPoint_CGPoint (out ret, this.SuperHandle, Selector.GetHandle ("charDetection:lastPoint:"), firstPoint, lastPoint);
					}
				} else if (IntPtr.Size == 8) {
					global::ApiDefinition.Messaging.CGRect_objc_msgSendSuper_stret_CGPoint_CGPoint (out ret, this.SuperHandle, Selector.GetHandle ("charDetection:lastPoint:"), firstPoint, lastPoint);
				} else {
					global::ApiDefinition.Messaging.CGRect_objc_msgSendSuper_stret_CGPoint_CGPoint (out ret, this.SuperHandle, Selector.GetHandle ("charDetection:lastPoint:"), firstPoint, lastPoint);
				}
			}
			return ret;
		}
		
		[Export ("doBcrRecognizeVedioWith:andHeight:andRect:andChannelNumberStr:")]
		[CompilerGenerated]
		public virtual int DoBcrRecognizeVedioWith (int width, int height, BRect pRect, string channelNumberStr)
		{
			if (channelNumberStr == null)
				throw new ArgumentNullException ("channelNumberStr");
			var nschannelNumberStr = NSString.CreateNative (channelNumberStr);
			
			int ret;
			if (IsDirectBinding) {
				ret = global::ApiDefinition.Messaging.int_objc_msgSend_int_int_BRect_IntPtr (this.Handle, Selector.GetHandle ("doBcrRecognizeVedioWith:andHeight:andRect:andChannelNumberStr:"), width, height, pRect, nschannelNumberStr);
			} else {
				ret = global::ApiDefinition.Messaging.int_objc_msgSendSuper_int_int_BRect_IntPtr (this.SuperHandle, Selector.GetHandle ("doBcrRecognizeVedioWith:andHeight:andRect:andChannelNumberStr:"), width, height, pRect, nschannelNumberStr);
			}
			NSString.ReleaseNative (nschannelNumberStr);
			
			return ret;
		}
		
		[Export ("doBCRWithRect:")]
		[CompilerGenerated]
		public virtual NSDictionary DoBCRWithRect (CGRect rect)
		{
			if (IsDirectBinding) {
				return  Runtime.GetNSObject<NSDictionary> (global::ApiDefinition.Messaging.IntPtr_objc_msgSend_CGRect (this.Handle, Selector.GetHandle ("doBCRWithRect:"), rect));
			} else {
				return  Runtime.GetNSObject<NSDictionary> (global::ApiDefinition.Messaging.IntPtr_objc_msgSendSuper_CGRect (this.SuperHandle, Selector.GetHandle ("doBCRWithRect:"), rect));
			}
		}
		
		[Export ("freeBImage")]
		[CompilerGenerated]
		public virtual void FreeBImage ()
		{
			if (IsDirectBinding) {
				global::ApiDefinition.Messaging.void_objc_msgSend (this.Handle, Selector.GetHandle ("freeBImage"));
			} else {
				global::ApiDefinition.Messaging.void_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("freeBImage"));
			}
		}
		
		[Export ("progressCancel")]
		[CompilerGenerated]
		public virtual void ProgressCancel ()
		{
			if (IsDirectBinding) {
				global::ApiDefinition.Messaging.void_objc_msgSend (this.Handle, Selector.GetHandle ("progressCancel"));
			} else {
				global::ApiDefinition.Messaging.void_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("progressCancel"));
			}
		}
		
		[Export ("setBcrResultCallbackDelegate:")]
		[CompilerGenerated]
		public virtual void SetBcrResultCallbackDelegate (NSObject @delegate)
		{
			if (@delegate == null)
				throw new ArgumentNullException ("@delegate");
			if (IsDirectBinding) {
				global::ApiDefinition.Messaging.void_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle ("setBcrResultCallbackDelegate:"), @delegate.Handle);
			} else {
				global::ApiDefinition.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.GetHandle ("setBcrResultCallbackDelegate:"), @delegate.Handle);
			}
		}
		
		[Export ("setProgressCallbackDelegate:")]
		[CompilerGenerated]
		public virtual void SetProgressCallbackDelegate (BCRProgressCallBackDelegate @delegate)
		{
			if (@delegate == null)
				throw new ArgumentNullException ("@delegate");
			if (IsDirectBinding) {
				global::ApiDefinition.Messaging.void_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle ("setProgressCallbackDelegate:"), @delegate.Handle);
			} else {
				global::ApiDefinition.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.GetHandle ("setProgressCallbackDelegate:"), @delegate.Handle);
			}
		}
		
		[CompilerGenerated]
		public virtual string ChNumberStr {
			[Export ("chNumberStr")]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (global::ApiDefinition.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.GetHandle ("chNumberStr")));
				} else {
					return NSString.FromHandle (global::ApiDefinition.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("chNumberStr")));
				}
			}
			
			[Export ("setChNumberStr:")]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				var nsvalue = NSString.CreateNative (value);
				
				if (IsDirectBinding) {
					global::ApiDefinition.Messaging.void_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle ("setChNumberStr:"), nsvalue);
				} else {
					global::ApiDefinition.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.GetHandle ("setChNumberStr:"), nsvalue);
				}
				NSString.ReleaseNative (nsvalue);
				
			}
		}
		
		[CompilerGenerated]
		public virtual NSObject[] DoBCR {
			[Export ("doBCR")]
			get {
				NSObject[] ret;
				if (IsDirectBinding) {
					ret = NSArray.ArrayFromHandle<NSObject>(global::ApiDefinition.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.GetHandle ("doBCR")));
				} else {
					ret = NSArray.ArrayFromHandle<NSObject>(global::ApiDefinition.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("doBCR")));
				}
				return ret;
			}
			
		}
		
		[CompilerGenerated]
		public virtual global::UIKit.UIImage HeadImage {
			[Export ("getHeadImage")]
			get {
				global::UIKit.UIImage ret;
				if (IsDirectBinding) {
					ret =  Runtime.GetNSObject<global::UIKit.UIImage> (global::ApiDefinition.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.GetHandle ("getHeadImage")));
				} else {
					ret =  Runtime.GetNSObject<global::UIKit.UIImage> (global::ApiDefinition.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("getHeadImage")));
				}
				return ret;
			}
			
		}
		
		[CompilerGenerated]
		public virtual global::UIKit.UIImage IDCardImage {
			[Export ("getIDCardImage")]
			get {
				global::UIKit.UIImage ret;
				if (IsDirectBinding) {
					ret =  Runtime.GetNSObject<global::UIKit.UIImage> (global::ApiDefinition.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.GetHandle ("getIDCardImage")));
				} else {
					ret =  Runtime.GetNSObject<global::UIKit.UIImage> (global::ApiDefinition.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("getIDCardImage")));
				}
				return ret;
			}
			
		}
		
		[CompilerGenerated]
		public virtual bool InitSuccess {
			[Export ("initSuccess")]
			get {
				if (IsDirectBinding) {
					return global::ApiDefinition.Messaging.bool_objc_msgSend (this.Handle, Selector.GetHandle ("initSuccess"));
				} else {
					return global::ApiDefinition.Messaging.bool_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("initSuccess"));
				}
			}
			
			[Export ("setInitSuccess:")]
			set {
				if (IsDirectBinding) {
					global::ApiDefinition.Messaging.void_objc_msgSend_bool (this.Handle, Selector.GetHandle ("setInitSuccess:"), value);
				} else {
					global::ApiDefinition.Messaging.void_objc_msgSendSuper_bool (this.SuperHandle, Selector.GetHandle ("setInitSuccess:"), value);
				}
			}
		}
		
		[CompilerGenerated]
		public virtual global::System.nint OcrLanguage {
			[Export ("ocrLanguage")]
			get {
				if (IsDirectBinding) {
					return global::ApiDefinition.Messaging.nint_objc_msgSend (this.Handle, Selector.GetHandle ("ocrLanguage"));
				} else {
					return global::ApiDefinition.Messaging.nint_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("ocrLanguage"));
				}
			}
			
			[Export ("setOcrLanguage:")]
			set {
				if (IsDirectBinding) {
					global::ApiDefinition.Messaging.void_objc_msgSend_nint (this.Handle, Selector.GetHandle ("setOcrLanguage:"), value);
				} else {
					global::ApiDefinition.Messaging.void_objc_msgSendSuper_nint (this.SuperHandle, Selector.GetHandle ("setOcrLanguage:"), value);
				}
			}
		}
		
		[CompilerGenerated]
		public virtual bool RotateBImage {
			[Export ("rotateBImage")]
			get {
				if (IsDirectBinding) {
					return global::ApiDefinition.Messaging.bool_objc_msgSend (this.Handle, Selector.GetHandle ("rotateBImage"));
				} else {
					return global::ApiDefinition.Messaging.bool_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("rotateBImage"));
				}
			}
			
		}
		
	} /* class YMIDCardEngine */
}
