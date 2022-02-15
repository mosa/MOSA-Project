namespace Mosa.DeviceSystem
{
	public interface ITimer
	{
		/// <summary>
		/// Waits for a specific time, in milliseconds.
		/// </summary>
		void Wait(uint ms);
	}
}
