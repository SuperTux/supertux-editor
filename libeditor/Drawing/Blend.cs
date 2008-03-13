using OpenGl;

namespace Drawing
{
	
	public struct Blend
	{
		public uint SFactor;
		public uint DFactor;

		public Blend(uint sfactor, uint dfactor)
		{
			this.SFactor = sfactor;
			this.DFactor = dfactor;
		}
	}

}
