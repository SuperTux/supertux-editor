namespace DataStructures
{

public struct Vector {
    public float X;
    public float Y;

    public Vector(float X, float Y) {
        this.X = X;
        this.Y = Y;
    }

    public static Vector operator +(Vector v1, Vector v2) {
        return new Vector(v1.X + v2.X, v1.Y + v2.Y);
    }
    
    public static Vector operator -(Vector v1, Vector v2) {
        return new Vector(v1.X - v2.X, v1.Y - v2.Y);
    }
    
	/// cross product
    public static Vector operator *(Vector v1, float s) {
        return new Vector(v1.X * s, v1.Y * s);
    }

	/// s-multiplication
	public static float operator *(Vector v1, Vector v2) {
		return v1.X * v2.X + v1.Y * v2.Y;
	}

	public static Vector operator /(Vector v1, float s) {
		return new Vector(v1.X / s, v1.Y / s);
	}

	public static bool operator ==(Vector v1, Vector v2) {
		return v1.X == v2.X && v1.Y == v2.Y;
	}

	public static bool operator !=(Vector v1, Vector v2) {
		return v1.X != v2.X || v1.Y != v2.Y;
	}

	public override bool Equals(object other) {
		if(! (other is Vector))
			return false;
		Vector ov = (Vector) other;
		return this == ov;
	}

	public override int GetHashCode() {
		return base.GetHashCode();
	}

	public override string ToString() {
		return "[" + X + ";" + Y + "]";
	}
}

}
