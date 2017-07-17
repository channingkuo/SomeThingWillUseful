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
	[Register("Utility", true)]
	public unsafe partial class Utility : NSObject {
		
		[CompilerGenerated]
		static readonly IntPtr class_ptr = Class.GetHandle ("Utility");
		
		public override IntPtr ClassHandle { get { return class_ptr; } }
		
		[CompilerGenerated]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("init")]
		public Utility () : base (NSObjectFlag.Empty)
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
		protected Utility (NSObjectFlag t) : base (t)
		{
			IsDirectBinding = GetType ().Assembly == global::ApiDefinition.Messaging.this_assembly;
		}

		[CompilerGenerated]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		protected internal Utility (IntPtr handle) : base (handle)
		{
			IsDirectBinding = GetType ().Assembly == global::ApiDefinition.Messaging.this_assembly;
		}

		[Export ("createRoundedRectImage:size:")]
		[CompilerGenerated]
		public static global::UIKit.UIImage CreateRoundedRectImage (global::UIKit.UIImage image, CGSize size)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			return  Runtime.GetNSObject<global::UIKit.UIImage> (global::ApiDefinition.Messaging.IntPtr_objc_msgSend_IntPtr_CGSize (class_ptr, Selector.GetHandle ("createRoundedRectImage:size:"), image.Handle, size));
		}
		
		[Export ("distanceBetweenPoints:toPoint:")]
		[CompilerGenerated]
		public static global::System.nfloat DistanceBetweenPoints (CGPoint first, CGPoint second)
		{
			return global::ApiDefinition.Messaging.nfloat_objc_msgSend_CGPoint_CGPoint (class_ptr, Selector.GetHandle ("distanceBetweenPoints:toPoint:"), first, second);
		}
		
		[Export ("informationAlertWithMessage:")]
		[CompilerGenerated]
		public static void InformationAlertWithMessage (string mes)
		{
			if (mes == null)
				throw new ArgumentNullException ("mes");
			var nsmes = NSString.CreateNative (mes);
			
			global::ApiDefinition.Messaging.void_objc_msgSend_IntPtr (class_ptr, Selector.GetHandle ("informationAlertWithMessage:"), nsmes);
			NSString.ReleaseNative (nsmes);
			
		}
		
		[Export ("warningAlertWithMessage:")]
		[CompilerGenerated]
		public static void WarningAlertWithMessage (string mes)
		{
			if (mes == null)
				throw new ArgumentNullException ("mes");
			var nsmes = NSString.CreateNative (mes);
			
			global::ApiDefinition.Messaging.void_objc_msgSend_IntPtr (class_ptr, Selector.GetHandle ("warningAlertWithMessage:"), nsmes);
			NSString.ReleaseNative (nsmes);
			
		}
		
	} /* class Utility */
}
