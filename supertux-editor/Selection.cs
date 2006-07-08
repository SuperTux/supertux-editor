
public sealed class Selection : TileBlock {
	public delegate void ChangedEventHandler();
	public event ChangedEventHandler Changed;

	public void FireChangedEvent() {
		Changed();
	}
}
