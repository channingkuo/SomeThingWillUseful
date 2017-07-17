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
	[Protocol (Name = "BcrResultCallbackDelegate", WrapperType = typeof (BcrResultCallbackDelegateWrapper))]
	[ProtocolMember (IsRequired = true, IsProperty = false, IsStatic = false, Name = "BcrResultCallbackWithValue", Selector = "bcrResultCallbackWithValue:", ParameterType = new Type [] { typeof (nint) }, ParameterByRef = new bool [] { false })]
	public interface IBcrResultCallbackDelegate : INativeObject, IDisposable
	{
		[CompilerGenerated]
		[Export ("bcrResultCallbackWithValue:")]
		[Preserve (Conditional = true)]
		void BcrResultCallbackWithValue (global::System.nint value);
		
	}
	
	internal sealed class BcrResultCallbackDelegateWrapper : BaseWrapper, IBcrResultCallbackDelegate {
		public BcrResultCallbackDelegateWrapper (IntPtr handle)
			: base (handle, false)
		{
		}
		
		[Preserve (Conditional = true)]
		public BcrResultCallbackDelegateWrapper (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}
		
		[Export ("bcrResultCallbackWithValue:")]
		[CompilerGenerated]
		public void BcrResultCallbackWithValue (global::System.nint value)
		{
			global::ApiDefinition.Messaging.void_objc_msgSend_nint (this.Handle, Selector.GetHandle ("bcrResultCallbackWithValue:"), value);
		}
		
	}
}
namespace OCR {
	[Protocol]
	[Register("BcrResultCallbackDelegate", false)]
	[Model]
	public unsafe abstract partial class BcrResultCallbackDelegate : NSObject, IBcrResultCallbackDelegate {
		
		[CompilerGenerated]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("init")]
		protected BcrResultCallbackDelegate () : base (NSObjectFlag.Empty)
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
		protected BcrResultCallbackDelegate (NSObjectFlag t) : base (t)
		{
			IsDirectBinding = GetType ().Assembly == global::ApiDefinition.Messaging.this_assembly;
		}

		[CompilerGenerated]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		protected internal BcrResultCallbackDelegate (IntPtr handle) : base (handle)
		{
			IsDirectBinding = GetType ().Assembly == global::ApiDefinition.Messaging.this_assembly;
		}

		[Export ("bcrResultCallbackWithValue:")]
		[CompilerGenerated]
		public abstract void BcrResultCallbackWithValue (global::System.nint value);
	} /* class BcrResultCallbackDelegate */
}
