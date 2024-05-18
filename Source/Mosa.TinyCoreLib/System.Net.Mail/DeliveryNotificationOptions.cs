namespace System.Net.Mail;

[Flags]
public enum DeliveryNotificationOptions
{
	None = 0,
	OnSuccess = 1,
	OnFailure = 2,
	Delay = 4,
	Never = 0x8000000
}
