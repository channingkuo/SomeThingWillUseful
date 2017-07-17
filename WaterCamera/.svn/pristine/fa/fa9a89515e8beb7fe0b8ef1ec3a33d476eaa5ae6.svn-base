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
	[Protocol (Name = "BCRProgressCallBackDelegate", WrapperType = typeof (BCRProgressCallBackDelegateWrapper))]
	[ProtocolMember (IsRequired = true, IsProperty = false, IsStatic = false, Name = "ProgressCallbackWithValue", Selector = "progressCallbackWithValue:", ParameterType = new Type [] { typeof (nint) }, ParameterByRef = new bool [] { false })]
	[ProtocolMember (IsRequired = true, IsProperty = false, IsStatic = false, Name = "ProgressStop", Selector = "progressStop")]
	public interface IBCRProgressCallBackDelegate : INativeObject, IDisposable
	{
		[CompilerGenerated]
		[Export ("progressCallbackWithValue:")]
		[Preserve (Conditional = true)]
		void ProgressCallbackWithValue (global::System.nint value);
		
		[CompilerGenerated]
		[Export ("progressStop")]
		[Preserve (Conditional = true)]
		void ProgressStop ();
		
	}
	
	internal sealed class BCRProgressCallBackDelegateWrapper : BaseWrapper, IBCRProgressCallBackDelegate {
		public BCRProgressCallBackDelegateWrapper (IntPtr handle)
			: base (handle, false)
		{
		}
		
		[Preserve (Conditional = true)]
		public BCRProgressCallBackDelegateWrapper (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}
		
		[Export ("progressCallbackWithValue:")]
		[CompilerGenerated]
		public void ProgressCallbackWithValue (global::System.nint value)
		{
			global::ApiDefinition.Messaging.void_objc_msgSend_nint (this.Handle, Selector.GetHandle ("progressCallbackWithValue:"), value);
		}
		
		[Export ("progressStop")]
		[CompilerGenerated]
		public void ProgressStop ()
		{
			global::ApiDefinition.Messaging.void_objc_msgSend (this.Handle, Selector.GetHandle ("progressStop"));
		}
		
	}
}
namespace OCR {
	[Protocol]
	[Register("BCRProgressCallBackDelegate", false)]
	[Model]
	public unsafe abstract partial class BCRProgressCallBackDelegate : NSObject, IBCRProgressCallBackDelegate {
		
		[CompilerGenerated]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("init")]
		protected BCRProgressCallBackDelegate () : base (NSObjectFlag.Empty)
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
		protected BCRProgressCallBackDelegate (NSObjectFlag t) : base (t)
		{
			IsDirectBinding = GetType ().Assembly == global::ApiDefinition.Messaging.this_assembly;
		}

		[CompilerGenerated]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		protected internal BCRProgressCallBackDelegate (IntPtr handle) : base (handle)
		{
			IsDirectBinding = GetType ().Assembly == global::ApiDefinition.Messaging.this_assembly;
		}

		[Export ("progressCallbackWithValue:")]
		[CompilerGenerated]
		public abstract void ProgressCallbackWithValue (global::System.nint value);
		[Export ("progressStop")]
		[CompilerGenerated]
		public abstract void ProgressStop ();
	} /* class BCRProgressCallBackDelegate */
}
