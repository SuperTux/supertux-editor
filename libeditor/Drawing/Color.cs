namespace Drawing
{
	
	public struct Color
	{
		/// red part of the color
		public float Red;
		/// green part of the color
		public float Green;
		/// blue part of the color
		public float Blue;
		/// alpha part of the color 1.0 = full opaque, 0.0 = invisible
		public float Alpha;
		
		public Color(float Red, float Green, float Blue, float Alpha) {
			this.Red = Red;
			this.Green = Green;
			this.Blue = Blue;
			this.Alpha = Alpha;
		}
		
		public Color(float Red, float Green, float Blue) {
			this.Red = Red;
			this.Green = Green;
			this.Blue = Blue;
			this.Alpha = 1.0f;
		}		
	}
	
}
