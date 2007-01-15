using Sdl;

public class Controller {
    private bool[] ControlState = new bool[(int) Control.CONTROLCOUNT];
    private bool[] LastState = new bool[(int) Control.CONTROLCOUNT];

	/** returns true if the specified controll is pressed */
    public bool IsDown(Control Control) {
        return ControlState[(int) Control];
    }
	/** returns true if the specified controll has just been pressed */
    public bool WasPressed(Control Control) {
        return LastState[(int) Control] == false && ControlState[(int) Control] == true;
    }

    public void NextFrame() {
        ControlState.CopyTo(LastState, 0);
    }

    public void HandleEvent(Event Event) {
        if(Event.type == SDL.KEYUP ||
           Event.type == SDL.KEYDOWN) {
            bool newstate = Event.type == SDL.KEYDOWN;
            switch(Event.key.keysym.sym) {
                case Key.UP:
                    ControlState[(int) Control.UP] = newstate;
                    break;
                case Key.DOWN:
                    ControlState[(int) Control.DOWN] = newstate;
                    break;
                case Key.LEFT:
                    ControlState[(int) Control.LEFT] = newstate;
                    break;
                case Key.RIGHT:
                    ControlState[(int) Control.RIGHT] = newstate;
                    break;
                case Key.SPACE:
                    ControlState[(int) Control.JUMP] = newstate;
                    break;
                case Key.LCTRL:
                case Key.RCTRL:
                    ControlState[(int) Control.SPECIAL] = newstate;
                    break;

                case Key.p:
                    ControlState[(int) Control.PAUSE] = newstate;
                    break;
                case Key.ESCAPE:
                    ControlState[(int) Control.MENU] = newstate;
                    break;
            }
        }
    }
}
