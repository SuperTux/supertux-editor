namespace DataStructures
{

	public struct FieldPos
	{
		public int X;
		public int Y;

		public FieldPos(int X, int Y)
		{
			this.X = X;
			this.Y = Y;
		}

		public static bool operator ==(FieldPos p1, FieldPos p2)
		{
			return p1.X == p2.X && p1.Y == p2.Y;
		}

		public static bool operator !=(FieldPos p1, FieldPos p2)
		{
			return p1.X != p2.X || p1.Y != p2.Y;
		}

		public override bool Equals(object other)
		{
			if(!(other is FieldPos))
				return false;
			FieldPos pos = (FieldPos) other;
			return this == pos;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public FieldPos Up
		{
			get {
				return new FieldPos(X, Y-1);
			}
		}

		public FieldPos Down
		{
			get {
				return new FieldPos(X, Y+1);
			}
		}

		public FieldPos Left
		{
			get {
				return new FieldPos(X-1, Y);
			}
		}

		public FieldPos Right
		{
			get {
				return new FieldPos(X+1, Y);
			}
		}

	}

}
