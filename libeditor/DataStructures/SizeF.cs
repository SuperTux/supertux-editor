//  $Id$
namespace DataStructures
{

	public struct SizeF
	{
		public float Width;
		public float Height;
		public bool IsEmpty
		{
			get { return Width <= 0 || Height <= 0; }
		}

		public SizeF(float Width, float Height)
		{
			this.Width = Width;
			this.Height = Height;
		}
	}

}
