using System;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using Sdl;
using Sdl.Image;
using OpenGl;
using Drawing;
using DataStructures;
using Resources;

public class Application {
    private CommandLine CommandLine = new CommandLine();
    public static Controller Controller = new Controller();

    private Application() {
    }

    public void Init(string[] Arguments) {
        if(!CommandLine.Parse(Arguments))
            return;

        InitSdl();
        InitVideo();
    }

    private void InitSdl() {
        if(SDL.Init(SDL.INIT_EVERYTHING | SDL.INIT_NOPARACHUTE) < 0) {
            throw new Exception("Couldn't initialize SDL: " + SDL.GetError());
        }
        SDL.EnableUNICODE(true);
    }

    private void InitVideo() {
        SDL.GL_SetAttribute(GLattr.DOUBLEBUFFER, 1);
        SDL.GL_SetAttribute(GLattr.RED_SIZE, 5);
        SDL.GL_SetAttribute(GLattr.GREEN_SIZE, 5);
        SDL.GL_SetAttribute(GLattr.BLUE_SIZE, 5);

        uint flags = SDL.OPENGL;
        Settings settings = Settings.Instance;
        if(settings.UseFullscreen)
            flags |= SDL.FULLSCREEN;
        int bpp = 0;

        IntPtr screen = SDL.SetVideoMode((int) settings.ScreenWidth,
                (int) settings.ScreenHeight,
                0, flags);
        if(screen == IntPtr.Zero)
            throw new Exception("Couldn't set video mode (" +
                    settings.ScreenWidth + "x" + settings.ScreenHeight +
                    "-" + bpp + "): " + SDL.GetError());

        SDL.WM_SetCaption(Constants.PACKAGE_NAME + " " +
                Constants.PACKAGE_VERSION, "");

        // set icon
        IntPtr icon = IMG.Load(
                ResourceManager.Instance.GetFilename("images/engine/icons/supertux.xpm"));
        if(icon != IntPtr.Zero) {
            SDL.WM_SetIcon(icon, IntPtr.Zero);
            SDL.FreeSurface(icon);
        }

        GlUtil.Assert("before setting opengl transforms");

        // setup opengl sstate and transform
        gl.Disable(gl.DEPTH_TEST);
        gl.Disable(gl.CULL_FACE);
        gl.Enable(gl.TEXTURE_2D);
        gl.Enable(gl.BLEND);
        gl.BlendFunc(gl.SRC_ALPHA, gl.ONE_MINUS_SRC_ALPHA);
        gl.Viewport(0, 0, (int) settings.ScreenWidth, (int) settings.ScreenHeight);
        gl.MatrixMode(gl.PROJECTION);
        gl.LoadIdentity();
        gl.Ortho(0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, 0,
                -1.0f, 1.0f);
        gl.MatrixMode(gl.MODELVIEW);
        gl.LoadIdentity();

        GlUtil.Assert("After setting opengl transforms");
    }

    public void Deinit() {
        Settings.Instance.Save();
        SDL.Quit();
    }

    public void Run() {
        WorldmapSector Sector = new WorldmapSector("worldmap.stwm");

        bool running = true;
        float frames = 0;
        uint frameticks = SDL.GetTicks();
        while(running) {
            Sdl.Event Event;
            while(SDL.PollEvent(out Event) > 0) {
                switch(Event.type) {
                    case SDL.QUIT:
                        running = false;
                        break;
                    case SDL.KEYDOWN:
                    case SDL.KEYUP:
                        Controller.HandleEvent(Event);
                        if(Controller.WasPressed(Control.MENU))
                            running = false;
                        break;
                }
            }

            Sector.Draw();
            SDL.GL_SwapBuffers();

            //SDL.Delay(10);
			Timer.Update(.01f);
            Sector.Update(.01f);

            frames++;
            uint ticks = SDL.GetTicks();
            if(ticks - frameticks > 1000) {
                Console.WriteLine("FPS: " + frames);
                frames = 0;
                frameticks = ticks;
            }
        }
   }

    public static void Main(string[] Arguments) {
        Application app = new Application();
        app.Init(Arguments);
        try {
            app.Run();
        } catch(Exception e) {
            Console.WriteLine("Exception: " + e.Message);
            Console.WriteLine(e.StackTrace);
        }
        app.Deinit();
    }
}

