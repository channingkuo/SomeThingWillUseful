using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace RekTec.Application.App
{
	public class XrmNavigationController : UINavigationController
	{
		public XrmNavigationController (UIViewController vc) : base (vc)
		{
		}

		public override bool ShouldAutorotate ()
		{
			if (VisibleViewController is UIAlertController || VisibleViewController == null) {
				return base.ShouldAutorotate ();
			}
			return VisibleViewController.ShouldAutorotate ();
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			if (VisibleViewController is UIAlertController || VisibleViewController == null) {
				return base.GetSupportedInterfaceOrientations ();
			}
			return VisibleViewController.GetSupportedInterfaceOrientations ();
		}

		public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation ()
		{
			if (VisibleViewController is UIAlertController || VisibleViewController == null) {
				return base.PreferredInterfaceOrientationForPresentation ();
			}
			return VisibleViewController.PreferredInterfaceOrientationForPresentation ();
		}
	}
}
