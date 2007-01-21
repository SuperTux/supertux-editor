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

		public static bool operator ==(SizeF s1, SizeF s2) {
			return s1.Width == s2.Width && s1.Height == s2.Height;
		}

		public static bool operator !=(SizeF s1, SizeF s2) {
			return s1.Width != s2.Width || s1.Height != s2.Height;
		}

		public override bool Equals(object obj) {
			if (!(obj is SizeF))
				return false;
			SizeF size = (SizeF) obj;
			return this == size;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

	}

}
