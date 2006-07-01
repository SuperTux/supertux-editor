namespace Drawing
{

	public struct Color
	{
		/// <summary>red part of the color</summary>
		public float Red;
		/// <summary>green part of the color</summary>
		public float Green;
		/// <summary>blue part of the color</summary>
		public float Blue;
		/// <summary>alpha part of the color</summary>
		/// <remarks>1.0 = full opaque, 0.0 = invisible</remarks>
		public float Alpha;

		public Color(float Red, float Green, float Blue, float Alpha)
		{
			this.Red = Red;
			this.Green = Green;
			this.Blue = Blue;
			this.Alpha = Alpha;
		}

		public Color(float Red, float Green, float Blue)
		{
			this.Red = Red;
			this.Green = Green;
			this.Blue = Blue;
			this.Alpha = 1.0f;
		}
	}

}
