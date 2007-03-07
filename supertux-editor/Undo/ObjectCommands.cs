// This file contains objectcommand classes used in several places.

namespace Undo {

	public abstract class ObjectCommand : Command {
		/// <summary>
		/// The object this action was on
		/// </summary>
		protected IGameObject changedObject;
		/// <summary>
		/// The object the object was/is in.
		/// </summary>
		protected Sector sector;

		protected ObjectCommand(string title, IGameObject changedObject, Sector sector)
			: base(title) {
			this.changedObject = changedObject;
			this.sector = sector;
		}
	}

	// TODO: Avoid code duplication from ObjectRemoveCommand
	// FIXME: Undoing this doesn't work, why?
	internal sealed class ObjectAddCommand : ObjectCommand {
		public override void Do() {
			sector.Add(changedObject, true);
		}

		public override void Undo() {
			sector.Remove(changedObject, true);
		}

		public ObjectAddCommand(string title, IGameObject changedObject, Sector sector)
			: base(title, changedObject, sector) { }
	}

	// TODO: Possible mem leak with objects hanging around forever?
	// TODO: Avoid code duplication from ObjectAddCommand
	// FIXME: Redoing this doesn't work, why?
	internal sealed class ObjectRemoveCommand : ObjectCommand {
		public override void Do() {
			sector.Remove(changedObject, true);
		}

		public override void Undo() {
			sector.Add(changedObject, true);
		}

		public ObjectRemoveCommand(string title, IGameObject changedObject, Sector sector)
			: base(title, changedObject, sector) { }
	}

}
