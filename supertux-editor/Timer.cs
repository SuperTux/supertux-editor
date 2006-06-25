using Sdl;

public class Timer {
	private static float Time = 0;
	
	public static float CurrentTime {
		get {
			return Time;
		}
	}

	public static void Update(float ElapsedTime) {
		Time += ElapsedTime;
	}
}
