using System;
using ObjCRuntime;

[assembly: LinkWith(
	"libPushSDK-1.8.3.a",
	LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator,
	SmartLink = true,
	ForceLoad = true,
	Frameworks = "CoreTelephony Security"
)]
