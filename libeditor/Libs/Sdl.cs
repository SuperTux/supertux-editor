//  $Id$
/* WARNING: Automatically generated file */
using System;
using System.Runtime.InteropServices;
using System.Security;


namespace Sdl
{
	[StructLayout(LayoutKind.Sequential)]
	public struct RWops
	{
		public IntPtr /* funcptr */ seek;
		public IntPtr /* funcptr */ read;
		public IntPtr /* funcptr */ write;
		public IntPtr /* funcptr */ close;
		public uint type;
	}





	[StructLayout(LayoutKind.Sequential)]
	public struct _SDL_TimerID
	{
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AudioSpec
	{
		public int freq;
		public ushort format;
		public byte channels;
		public byte silence;
		public ushort samples;
		public ushort padding;
		public uint size;
		public IntPtr /* funcptr */ callback;
		public IntPtr /*void*/ userdata;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AudioCVT
	{
		public int needed;
		public ushort src_format;
		public ushort dst_format;
		public double rate_incr;
		public IntPtr /*byte*/ buf;
		public int len;
		public int len_cvt;
		public int len_mult;
		public double len_ratio;
		public IntPtr /* funcptr */ filters;
		public int filter_index;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CDtrack
	{
		public byte id;
		public byte type;
		public ushort unused;
		public uint length;
		public uint offset;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CD
	{
		public int id;
		public CDstatus status;
		public int numtracks;
		public int cur_track;
		public int cur_frame;
		public CDtrack track;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Joystick
	{
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct keysym
	{
		public byte scancode;
		public Key sym;
		public KMod mod;
		public ushort unicode;
	}




	[StructLayout(LayoutKind.Sequential)]
	public struct Rect
	{
		public short x;
		public short y;
		public ushort w;
		public ushort h;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Color
	{
		public byte r;
		public byte g;
		public byte b;
		public byte unused;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Palette
	{
		public int ncolors;
		public IntPtr /*Color*/ colors;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PixelFormat
	{
		public IntPtr /*Palette*/ palette;
		public byte BitsPerPixel;
		public byte BytesPerPixel;
		public byte Rloss;
		public byte Gloss;
		public byte Bloss;
		public byte Aloss;
		public byte Rshift;
		public byte Gshift;
		public byte Bshift;
		public byte Ashift;
		public uint Rmask;
		public uint Gmask;
		public uint Bmask;
		public uint Amask;
		public uint colorkey;
		public byte alpha;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Surface
	{
		public uint flags;
		public IntPtr /*PixelFormat*/ format;
		public int w;
		public int h;
		public ushort pitch;
		public IntPtr /*void*/ pixels;
		public int offset;
		public IntPtr /*private_hwdata*/ hwdata;
		public Rect clip_rect;
		public uint unused1;
		public uint locked;
		public IntPtr /*BlitMap*/ map;
		public uint format_version;
		public int refcount;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct private_hwdata
	{
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct BlitMap
	{
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VideoInfo
	{
		public uint hw_available;
		public uint wm_available;
		public uint UnusedBits1;
		public uint UnusedBits2;
		public uint blit_hw;
		public uint blit_hw_CC;
		public uint blit_hw_A;
		public uint blit_sw;
		public uint blit_sw_CC;
		public uint blit_sw_A;
		public uint blit_fill;
		public uint UnusedBits3;
		public uint video_mem;
		public IntPtr /*PixelFormat*/ vfmt;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Overlay
	{
		public uint format;
		public int w;
		public int h;
		public int planes;
		public IntPtr /*ushort*/ pitches;
		public IntPtr /*IntPtr byte*/ pixels;
		public IntPtr /*private_yuvhwfuncs*/ hwfuncs;
		public IntPtr /*private_yuvhwdata*/ hwdata;
		public uint hw_overlay;
		public uint UnusedBits;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct private_yuvhwfuncs
	{
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct private_yuvhwdata
	{
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WMcursor
	{
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Cursor
	{
		public Rect area;
		public short hot_x;
		public short hot_y;
		public IntPtr /*byte*/ data;
		public IntPtr /*byte*/ mask;
		public IntPtr /*byte*/ save;
		public IntPtr /*WMcursor*/ wm_cursor;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ActiveEvent
	{
		public byte type;
		public byte gain;
		public byte state;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct KeyboardEvent
	{
		public byte type;
		public byte which;
		public byte state;
		public keysym keysym;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MouseMotionEvent
	{
		public byte type;
		public byte which;
		public byte state;
		public ushort x;
		public ushort y;
		public short xrel;
		public short yrel;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MouseButtonEvent
	{
		public byte type;
		public byte which;
		public byte button;
		public byte state;
		public ushort x;
		public ushort y;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct JoyAxisEvent
	{
		public byte type;
		public byte which;
		public byte axis;
		public short _value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct JoyBallEvent
	{
		public byte type;
		public byte which;
		public byte ball;
		public short xrel;
		public short yrel;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct JoyHatEvent
	{
		public byte type;
		public byte which;
		public byte hat;
		public byte _value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct JoyButtonEvent
	{
		public byte type;
		public byte which;
		public byte button;
		public byte state;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ResizeEvent
	{
		public byte type;
		public int w;
		public int h;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ExposeEvent
	{
		public byte type;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct QuitEvent
	{
		public byte type;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct UserEvent
	{
		public byte type;
		public int code;
		public IntPtr /*void*/ data1;
		public IntPtr /*void*/ data2;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SysWMmsg
	{
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SysWMEvent
	{
		public byte type;
		public IntPtr /*SysWMmsg*/ msg;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct Event
	{
		[FieldOffset(0)]
		public byte type;
		[FieldOffset(0)]
		public ActiveEvent active;
		[FieldOffset(0)]
		public KeyboardEvent key;
		[FieldOffset(0)]
		public MouseMotionEvent motion;
		[FieldOffset(0)]
		public MouseButtonEvent button;
		[FieldOffset(0)]
		public JoyAxisEvent jaxis;
		[FieldOffset(0)]
		public JoyBallEvent jball;
		[FieldOffset(0)]
		public JoyHatEvent jhat;
		[FieldOffset(0)]
		public JoyButtonEvent jbutton;
		[FieldOffset(0)]
		public ResizeEvent resize;
		[FieldOffset(0)]
		public ExposeEvent expose;
		[FieldOffset(0)]
		public QuitEvent quit;
		[FieldOffset(0)]
		public UserEvent user;
		[FieldOffset(0)]
		public SysWMEvent syswm;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Version
	{
		public byte major;
		public byte minor;
		public byte patch;
	}




	public enum ErrorCode
	{
		ENOMEM,
		EFREAD,
		EFWRITE,
		EFSEEK,
		LASTERROR,
	}

	public enum AudioStatus
	{
		STOPPED = 0,
		PLAYING,
		PAUSED,
	}

	public enum CDstatus
	{
		TRAYEMPTY,
		STOPPED,
		PLAYING,
		PAUSED,
		ERROR = (int) (-1),
	}

	public enum Key
	{
		UNKNOWN = 0,
		FIRST = 0,
		BACKSPACE = 8,
		TAB = 9,
		CLEAR = 12,
		RETURN = 13,
		PAUSE = 19,
		ESCAPE = 27,
		SPACE = 32,
		EXCLAIM = 33,
		QUOTEDBL = 34,
		HASH = 35,
		DOLLAR = 36,
		AMPERSAND = 38,
		QUOTE = 39,
		LEFTPAREN = 40,
		RIGHTPAREN = 41,
		ASTERISK = 42,
		PLUS = 43,
		COMMA = 44,
		MINUS = 45,
		PERIOD = 46,
		SLASH = 47,
		_0 = 48,
		_1 = 49,
		_2 = 50,
		_3 = 51,
		_4 = 52,
		_5 = 53,
		_6 = 54,
		_7 = 55,
		_8 = 56,
		_9 = 57,
		COLON = 58,
		SEMICOLON = 59,
		LESS = 60,
		EQUALS = 61,
		GREATER = 62,
		QUESTION = 63,
		AT = 64,
		LEFTBRACKET = 91,
		BACKSLASH = 92,
		RIGHTBRACKET = 93,
		CARET = 94,
		UNDERSCORE = 95,
		BACKQUOTE = 96,
		a = 97,
		b = 98,
		c = 99,
		d = 100,
		e = 101,
		f = 102,
		g = 103,
		h = 104,
		i = 105,
		j = 106,
		k = 107,
		l = 108,
		m = 109,
		n = 110,
		o = 111,
		p = 112,
		q = 113,
		r = 114,
		s = 115,
		t = 116,
		u = 117,
		v = 118,
		w = 119,
		x = 120,
		y = 121,
		z = 122,
		DELETE = 127,
		WORLD_0 = 160,
		WORLD_1 = 161,
		WORLD_2 = 162,
		WORLD_3 = 163,
		WORLD_4 = 164,
		WORLD_5 = 165,
		WORLD_6 = 166,
		WORLD_7 = 167,
		WORLD_8 = 168,
		WORLD_9 = 169,
		WORLD_10 = 170,
		WORLD_11 = 171,
		WORLD_12 = 172,
		WORLD_13 = 173,
		WORLD_14 = 174,
		WORLD_15 = 175,
		WORLD_16 = 176,
		WORLD_17 = 177,
		WORLD_18 = 178,
		WORLD_19 = 179,
		WORLD_20 = 180,
		WORLD_21 = 181,
		WORLD_22 = 182,
		WORLD_23 = 183,
		WORLD_24 = 184,
		WORLD_25 = 185,
		WORLD_26 = 186,
		WORLD_27 = 187,
		WORLD_28 = 188,
		WORLD_29 = 189,
		WORLD_30 = 190,
		WORLD_31 = 191,
		WORLD_32 = 192,
		WORLD_33 = 193,
		WORLD_34 = 194,
		WORLD_35 = 195,
		WORLD_36 = 196,
		WORLD_37 = 197,
		WORLD_38 = 198,
		WORLD_39 = 199,
		WORLD_40 = 200,
		WORLD_41 = 201,
		WORLD_42 = 202,
		WORLD_43 = 203,
		WORLD_44 = 204,
		WORLD_45 = 205,
		WORLD_46 = 206,
		WORLD_47 = 207,
		WORLD_48 = 208,
		WORLD_49 = 209,
		WORLD_50 = 210,
		WORLD_51 = 211,
		WORLD_52 = 212,
		WORLD_53 = 213,
		WORLD_54 = 214,
		WORLD_55 = 215,
		WORLD_56 = 216,
		WORLD_57 = 217,
		WORLD_58 = 218,
		WORLD_59 = 219,
		WORLD_60 = 220,
		WORLD_61 = 221,
		WORLD_62 = 222,
		WORLD_63 = 223,
		WORLD_64 = 224,
		WORLD_65 = 225,
		WORLD_66 = 226,
		WORLD_67 = 227,
		WORLD_68 = 228,
		WORLD_69 = 229,
		WORLD_70 = 230,
		WORLD_71 = 231,
		WORLD_72 = 232,
		WORLD_73 = 233,
		WORLD_74 = 234,
		WORLD_75 = 235,
		WORLD_76 = 236,
		WORLD_77 = 237,
		WORLD_78 = 238,
		WORLD_79 = 239,
		WORLD_80 = 240,
		WORLD_81 = 241,
		WORLD_82 = 242,
		WORLD_83 = 243,
		WORLD_84 = 244,
		WORLD_85 = 245,
		WORLD_86 = 246,
		WORLD_87 = 247,
		WORLD_88 = 248,
		WORLD_89 = 249,
		WORLD_90 = 250,
		WORLD_91 = 251,
		WORLD_92 = 252,
		WORLD_93 = 253,
		WORLD_94 = 254,
		WORLD_95 = 255,
		KP0 = 256,
		KP1 = 257,
		KP2 = 258,
		KP3 = 259,
		KP4 = 260,
		KP5 = 261,
		KP6 = 262,
		KP7 = 263,
		KP8 = 264,
		KP9 = 265,
		KP_PERIOD = 266,
		KP_DIVIDE = 267,
		KP_MULTIPLY = 268,
		KP_MINUS = 269,
		KP_PLUS = 270,
		KP_ENTER = 271,
		KP_EQUALS = 272,
		UP = 273,
		DOWN = 274,
		RIGHT = 275,
		LEFT = 276,
		INSERT = 277,
		HOME = 278,
		END = 279,
		PAGEUP = 280,
		PAGEDOWN = 281,
		F1 = 282,
		F2 = 283,
		F3 = 284,
		F4 = 285,
		F5 = 286,
		F6 = 287,
		F7 = 288,
		F8 = 289,
		F9 = 290,
		F10 = 291,
		F11 = 292,
		F12 = 293,
		F13 = 294,
		F14 = 295,
		F15 = 296,
		NUMLOCK = 300,
		CAPSLOCK = 301,
		SCROLLOCK = 302,
		RSHIFT = 303,
		LSHIFT = 304,
		RCTRL = 305,
		LCTRL = 306,
		RALT = 307,
		LALT = 308,
		RMETA = 309,
		LMETA = 310,
		LSUPER = 311,
		RSUPER = 312,
		MODE = 313,
		COMPOSE = 314,
		HELP = 315,
		PRINT = 316,
		SYSREQ = 317,
		BREAK = 318,
		MENU = 319,
		POWER = 320,
		EURO = 321,
		UNDO = 322,
		LAST,
	}

	public enum KMod
	{
		NONE = 0x0000,
		LSHIFT = 0x0001,
		RSHIFT = 0x0002,
		LCTRL = 0x0040,
		RCTRL = 0x0080,
		LALT = 0x0100,
		RALT = 0x0200,
		LMETA = 0x0400,
		RMETA = 0x0800,
		NUM = 0x1000,
		CAPS = 0x2000,
		MODE = 0x4000,
		RESERVED = 0x8000,
	}

	public enum GLattr
	{
		RED_SIZE,
		GREEN_SIZE,
		BLUE_SIZE,
		ALPHA_SIZE,
		BUFFER_SIZE,
		DOUBLEBUFFER,
		DEPTH_SIZE,
		STENCIL_SIZE,
		ACCUM_RED_SIZE,
		ACCUM_GREEN_SIZE,
		ACCUM_BLUE_SIZE,
		ACCUM_ALPHA_SIZE,
		STEREO,
		MULTISAMPLEBUFFERS,
		MULTISAMPLESAMPLES,
	}

	public enum GrabMode
	{
		QUERY = (int) (-1),
		OFF = 0,
		ON = 1,
		FULLSCREEN,
	}



	public enum EventAction
	{
		ADDEVENT,
		PEEKEVENT,
		GETEVENT,
	}

	public class SDL
	{

		private const string SDL_DLL = "sdl.dll";

		public const uint AUDIO_S16 = 0x8010;
		public const uint AUDIO_S16LSB = 0x8010;
		public const uint AUDIO_S16MSB = 0x9010;
		public const uint AUDIO_S8 = 0x8008;
		public const uint AUDIO_U16 = 0x0010;
		public const uint AUDIO_U16LSB = 0x0010;
		public const uint AUDIO_U16MSB = 0x1010;
		public const uint AUDIO_U8 = 0x0008;
		public const int CD_FPS = 75;
		public const uint ALL_HOTKEYS = 0xFFFFFFFF;
		public const uint ALLEVENTS = 0xFFFFFFFF;
		public const int ALPHA_OPAQUE = 255;
		public const int ALPHA_TRANSPARENT = 0;
		public const uint ANYFORMAT = 0x10000000;
		public const uint APPACTIVE = 0x04;
		public const uint APPINPUTFOCUS = 0x02;
		public const uint APPMOUSEFOCUS = 0x01;
		public const uint ASYNCBLIT = 0x00000004;
		public const uint AUDIO_TRACK = 0x00;
		public const int BIG_ENDIAN = 4321;
		public const int BUTTON_LEFT = 1;
		public const int BUTTON_MIDDLE = 2;
		public const int BUTTON_RIGHT = 3;
		public const int BUTTON_WHEELDOWN = 5;
		public const int BUTTON_WHEELUP = 4;
		public const uint DATA_TRACK = 0x04;
		public const int DEFAULT_REPEAT_DELAY = 500;
		public const int DEFAULT_REPEAT_INTERVAL = 30;
		public const int DISABLE = 0;
		public const uint DOUBLEBUF = 0x40000000;
		public const int ENABLE = 1;
		public const uint FULLSCREEN = 0x80000000;
		public const uint HAT_CENTERED = 0x00;
		public const uint HAT_DOWN = 0x04;
		public const uint HAT_LEFT = 0x08;
		public const uint HAT_RIGHT = 0x02;
		public const uint HAT_UP = 0x01;
		public const uint HWACCEL = 0x00000100;
		public const uint HWPALETTE = 0x20000000;
		public const uint HWSURFACE = 0x00000001;
		public const int IGNORE = 0;
		public const uint INIT_AUDIO = 0x00000010;
		public const uint INIT_CDROM = 0x00000100;
		public const uint INIT_EVENTTHREAD = 0x01000000;
		public const uint INIT_EVERYTHING = 0x0000FFFF;
		public const uint INIT_JOYSTICK = 0x00000200;
		public const uint INIT_NOPARACHUTE = 0x00100000;
		public const uint INIT_TIMER = 0x00000001;
		public const uint INIT_VIDEO = 0x00000020;
		public const uint IYUV_OVERLAY = 0x56555949;
		public const int LIL_ENDIAN = 1234;
		public const uint LOGPAL = 0x01;
		public const int MAJOR_VERSION = 1;
		public const int MAX_TRACKS = 99;
		public const int MINOR_VERSION = 2;
		public const int MIX_MAXVOLUME = 128;
		public const int MUTEX_TIMEDOUT = 1;
		public const uint NOFRAME = 0x00000020;
		public const uint OPENGL = 0x00000002;
		public const uint OPENGLBLIT = 0x0000000A;
		public const int PATCHLEVEL = 8;
		public const uint PHYSPAL = 0x02;
		public const uint PREALLOC = 0x01000000;
		public const int QUERY = -1;
		public const uint RESIZABLE = 0x00000010;
		public const uint RLEACCEL = 0x00004000;
		public const uint RLEACCELOK = 0x00002000;
		public const uint SRCALPHA = 0x00010000;
		public const uint SRCCOLORKEY = 0x00001000;
		public const uint SWSURFACE = 0x00000000;
		public const int TIMESLICE = 10;
		public const uint UYVY_OVERLAY = 0x59565955;
		public const uint YUY2_OVERLAY = 0x32595559;
		public const uint YV12_OVERLAY = 0x32315659;
		public const uint YVYU_OVERLAY = 0x55595659;
		public const int TIMER_RESOLUTION = 10;
		public const int PRESSED = 0x01;
		public const int RELEASED = 0x00;
		public const int NOEVENT = 0;
		public const int ACTIVEEVENT = 1;
		public const int KEYDOWN = 2;
		public const int KEYUP = 3;
		public const int MOUSEMOTION = 4;
		public const int MOUSEBUTTONDOWN = 5;
		public const int MOUSEBUTTONUP = 6;
		public const int JOYAXISMOTION = 7;
		public const int JOYBALLMOTION = 8;
		public const int JOYHATMOTION = 9;
		public const int JOYBUTTONDOWN = 10;
		public const int JOYBUTTONUP = 11;
		public const int QUIT = 12;
		public const int SYSWMEVENT = 13;
		public const int EVENT_RESERVEDA = 14;
		public const int EVENT_RESERVEDB = 15;
		public const int VIDEORESIZE = 16;
		public const int VIDEOEXPOSE = 17;
		public const int EVENT_RESERVED2 = 18;
		public const int EVENT_RESERVED3 = 19;
		public const int EVENT_RESERVED4 = 20;
		public const int EVENT_RESERVED5 = 21;
		public const int EVENT_RESERVED6 = 22;
		public const int EVENT_RESERVED7 = 23;
		public const int USEREVENT = 24;
		public const int NUMEVENTS = 32;
		public const int ACTIVEEVENTMASK = (int) (1 << ACTIVEEVENT);
		public const int KEYDOWNMASK = (int) (1 << KEYDOWN);
		public const int KEYUPMASK = (int) (1 << KEYUP);
		public const int MOUSEMOTIONMASK = (int) (1 << MOUSEMOTION);
		public const int MOUSEBUTTONDOWNMASK = (int) (1 << MOUSEBUTTONDOWN);
		public const int MOUSEBUTTONUPMASK = (int) (1 << MOUSEBUTTONUP);
		public const int MOUSEEVENTMASK = (int) (1 << MOUSEMOTION | 1 << MOUSEBUTTONDOWN | 1 << MOUSEBUTTONUP);
		public const int JOYAXISMOTIONMASK = (int) (1 << JOYAXISMOTION);
		public const int JOYBALLMOTIONMASK = (int) (1 << JOYBALLMOTION);
		public const int JOYHATMOTIONMASK = (int) (1 << JOYHATMOTION);
		public const int JOYBUTTONDOWNMASK = (int) (1 << JOYBUTTONDOWN);
		public const int JOYBUTTONUPMASK = (int) (1 << JOYBUTTONUP);
		public const int JOYEVENTMASK = (int) (1 << JOYAXISMOTION | 1 << JOYBALLMOTION | 1 << JOYHATMOTION | 1 << JOYBUTTONDOWN | 1 << JOYBUTTONUP);
		public const int VIDEORESIZEMASK = (int) (1 << VIDEORESIZE);
		public const int VIDEOEXPOSEMASK = (int) (1 << VIDEOEXPOSE);
		public const int QUITMASK = (int) (1 << QUIT);
		public const int SYSWMEVENTMASK = (int) (1 << SYSWMEVENT);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetError"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*sbyte*/ _GetError();

		[DllImport(SDL_DLL, EntryPoint = "SDL_ClearError"), SuppressUnmanagedCodeSecurity]
		public static extern void ClearError();

		[DllImport(SDL_DLL, EntryPoint = "SDL_Error"), SuppressUnmanagedCodeSecurity]
		public static extern void Error(ErrorCode code);

		[DllImport(SDL_DLL, EntryPoint = "SDL_RWFromFile"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*RWops*/ RWFromFile(string file, string mode);

		[DllImport(SDL_DLL, EntryPoint = "SDL_RWFromMem"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*RWops*/ RWFromMem(IntPtr mem, int size);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetTicks"), SuppressUnmanagedCodeSecurity]
		public static extern uint GetTicks();

		[DllImport(SDL_DLL, EntryPoint = "SDL_Delay"), SuppressUnmanagedCodeSecurity]
		public static extern void Delay(uint ms);

		[DllImport(SDL_DLL, EntryPoint = "SDL_AudioInit"), SuppressUnmanagedCodeSecurity]
		public static extern int AudioInit(string driver_name);

		[DllImport(SDL_DLL, EntryPoint = "SDL_AudioQuit"), SuppressUnmanagedCodeSecurity]
		public static extern void AudioQuit();

		[DllImport(SDL_DLL, EntryPoint = "SDL_AudioDriverName"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*sbyte*/ AudioDriverName([In, Out] sbyte[] namebuf, int maxlen);

		[DllImport(SDL_DLL, EntryPoint = "SDL_OpenAudio"), SuppressUnmanagedCodeSecurity]
		public static extern int OpenAudio(ref AudioSpec desired, ref AudioSpec obtained);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetAudioStatus"), SuppressUnmanagedCodeSecurity]
		public static extern AudioStatus GetAudioStatus();

		[DllImport(SDL_DLL, EntryPoint = "SDL_PauseAudio"), SuppressUnmanagedCodeSecurity]
		public static extern void PauseAudio(int pause_on);

		[DllImport(SDL_DLL, EntryPoint = "SDL_FreeWAV"), SuppressUnmanagedCodeSecurity]
		public static extern void FreeWAV([In, Out] byte[] audio_buf);

		[DllImport(SDL_DLL, EntryPoint = "SDL_BuildAudioCVT"), SuppressUnmanagedCodeSecurity]
		public static extern int BuildAudioCVT(ref AudioCVT cvt, ushort src_format, byte src_channels, int src_rate, ushort dst_format, byte dst_channels, int dst_rate);

		[DllImport(SDL_DLL, EntryPoint = "SDL_ConvertAudio"), SuppressUnmanagedCodeSecurity]
		public static extern int ConvertAudio(ref AudioCVT cvt);

		[DllImport(SDL_DLL, EntryPoint = "SDL_MixAudio"), SuppressUnmanagedCodeSecurity]
		public static extern void MixAudio([In, Out] byte[] dst, [In] byte[] src, uint len, int volume);

		[DllImport(SDL_DLL, EntryPoint = "SDL_LockAudio"), SuppressUnmanagedCodeSecurity]
		public static extern void LockAudio();

		[DllImport(SDL_DLL, EntryPoint = "SDL_UnlockAudio"), SuppressUnmanagedCodeSecurity]
		public static extern void UnlockAudio();

		[DllImport(SDL_DLL, EntryPoint = "SDL_CloseAudio"), SuppressUnmanagedCodeSecurity]
		public static extern void CloseAudio();

		[DllImport(SDL_DLL, EntryPoint = "SDL_CDNumDrives"), SuppressUnmanagedCodeSecurity]
		public static extern int CDNumDrives();

		[DllImport(SDL_DLL, EntryPoint = "SDL_CDName"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*sbyte*/ CDName(int drive);

		[DllImport(SDL_DLL, EntryPoint = "SDL_CDOpen"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*CD*/ CDOpen(int drive);

		[DllImport(SDL_DLL, EntryPoint = "SDL_CDStatus"), SuppressUnmanagedCodeSecurity]
		public static extern CDstatus CDStatus(IntPtr /*CD*/ cdrom);

		[DllImport(SDL_DLL, EntryPoint = "SDL_CDPlayTracks"), SuppressUnmanagedCodeSecurity]
		public static extern int CDPlayTracks(IntPtr /*CD*/ cdrom, int start_track, int start_frame, int ntracks, int nframes);

		[DllImport(SDL_DLL, EntryPoint = "SDL_CDPlay"), SuppressUnmanagedCodeSecurity]
		public static extern int CDPlay(IntPtr /*CD*/ cdrom, int start, int length);

		[DllImport(SDL_DLL, EntryPoint = "SDL_CDPause"), SuppressUnmanagedCodeSecurity]
		public static extern int CDPause(IntPtr /*CD*/ cdrom);

		[DllImport(SDL_DLL, EntryPoint = "SDL_CDResume"), SuppressUnmanagedCodeSecurity]
		public static extern int CDResume(IntPtr /*CD*/ cdrom);

		[DllImport(SDL_DLL, EntryPoint = "SDL_CDStop"), SuppressUnmanagedCodeSecurity]
		public static extern int CDStop(IntPtr /*CD*/ cdrom);

		[DllImport(SDL_DLL, EntryPoint = "SDL_CDEject"), SuppressUnmanagedCodeSecurity]
		public static extern int CDEject(IntPtr /*CD*/ cdrom);

		[DllImport(SDL_DLL, EntryPoint = "SDL_CDClose"), SuppressUnmanagedCodeSecurity]
		public static extern void CDClose(IntPtr /*CD*/ cdrom);

		[DllImport(SDL_DLL, EntryPoint = "SDL_NumJoysticks"), SuppressUnmanagedCodeSecurity]
		public static extern int NumJoysticks();

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickName"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*sbyte*/ JoystickName(int device_index);

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickOpen"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*Joystick*/ JoystickOpen(int device_index);

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickOpened"), SuppressUnmanagedCodeSecurity]
		public static extern int JoystickOpened(int device_index);

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickIndex"), SuppressUnmanagedCodeSecurity]
		public static extern int JoystickIndex(IntPtr /*Joystick*/ joystick);

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickNumAxes"), SuppressUnmanagedCodeSecurity]
		public static extern int JoystickNumAxes(IntPtr /*Joystick*/ joystick);

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickNumBalls"), SuppressUnmanagedCodeSecurity]
		public static extern int JoystickNumBalls(IntPtr /*Joystick*/ joystick);

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickNumHats"), SuppressUnmanagedCodeSecurity]
		public static extern int JoystickNumHats(IntPtr /*Joystick*/ joystick);

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickNumButtons"), SuppressUnmanagedCodeSecurity]
		public static extern int JoystickNumButtons(IntPtr /*Joystick*/ joystick);

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickUpdate"), SuppressUnmanagedCodeSecurity]
		public static extern void JoystickUpdate();

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickEventState"), SuppressUnmanagedCodeSecurity]
		public static extern int JoystickEventState(int state);

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickGetAxis"), SuppressUnmanagedCodeSecurity]
		public static extern short JoystickGetAxis(IntPtr /*Joystick*/ joystick, int axis);

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickGetHat"), SuppressUnmanagedCodeSecurity]
		public static extern byte JoystickGetHat(IntPtr /*Joystick*/ joystick, int hat);

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickGetBall"), SuppressUnmanagedCodeSecurity]
		public static extern int JoystickGetBall(IntPtr /*Joystick*/ joystick, int ball, [In, Out] int[] dx, [In, Out] int[] dy);

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickGetButton"), SuppressUnmanagedCodeSecurity]
		public static extern byte JoystickGetButton(IntPtr /*Joystick*/ joystick, int button);

		[DllImport(SDL_DLL, EntryPoint = "SDL_JoystickClose"), SuppressUnmanagedCodeSecurity]
		public static extern void JoystickClose(IntPtr /*Joystick*/ joystick);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetAppState"), SuppressUnmanagedCodeSecurity]
		public static extern byte GetAppState();

		[DllImport(SDL_DLL, EntryPoint = "SDL_EnableUNICODE"), SuppressUnmanagedCodeSecurity]
		public static extern int EnableUNICODE(bool enable);

		[DllImport(SDL_DLL, EntryPoint = "SDL_EnableKeyRepeat"), SuppressUnmanagedCodeSecurity]
		public static extern int EnableKeyRepeat(int delay, int interval);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetKeyState"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*byte*/ GetKeyState([In, Out] int[] numkeys);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetModState"), SuppressUnmanagedCodeSecurity]
		public static extern KMod GetModState();

		[DllImport(SDL_DLL, EntryPoint = "SDL_SetModState"), SuppressUnmanagedCodeSecurity]
		public static extern void SetModState(KMod modstate);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetKeyName"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*sbyte*/ GetKeyName(Key key);

		[DllImport(SDL_DLL, EntryPoint = "SDL_VideoInit"), SuppressUnmanagedCodeSecurity]
		public static extern int VideoInit(string driver_name, uint flags);

		[DllImport(SDL_DLL, EntryPoint = "SDL_VideoQuit"), SuppressUnmanagedCodeSecurity]
		public static extern void VideoQuit();

		[DllImport(SDL_DLL, EntryPoint = "SDL_VideoDriverName"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*sbyte*/ VideoDriverName([In, Out] sbyte[] namebuf, int maxlen);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetVideoSurface"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*Surface*/ GetVideoSurface();

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetVideoInfo"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*VideoInfo*/ GetVideoInfo();

		[DllImport(SDL_DLL, EntryPoint = "SDL_VideoModeOK"), SuppressUnmanagedCodeSecurity]
		public static extern int VideoModeOK(int width, int height, int bpp, uint flags);

		[DllImport(SDL_DLL, EntryPoint = "SDL_ListModes"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*IntPtr Rect*/ ListModes(ref PixelFormat format, uint flags);

		[DllImport(SDL_DLL, EntryPoint = "SDL_SetVideoMode"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*Surface*/ SetVideoMode(int width, int height, int bpp, uint flags);

		[DllImport(SDL_DLL, EntryPoint = "SDL_UpdateRects"), SuppressUnmanagedCodeSecurity]
		public static extern void UpdateRects(IntPtr /*Surface*/ screen, int numrects, ref Rect rects);

		[DllImport(SDL_DLL, EntryPoint = "SDL_UpdateRect"), SuppressUnmanagedCodeSecurity]
		public static extern void UpdateRect(IntPtr /*Surface*/ screen, int x, int y, uint w, uint h);

		[DllImport(SDL_DLL, EntryPoint = "SDL_Flip"), SuppressUnmanagedCodeSecurity]
		public static extern int Flip(IntPtr /*Surface*/ screen);

		[DllImport(SDL_DLL, EntryPoint = "SDL_SetGamma"), SuppressUnmanagedCodeSecurity]
		public static extern int SetGamma(float red, float green, float blue);

		[DllImport(SDL_DLL, EntryPoint = "SDL_SetGammaRamp"), SuppressUnmanagedCodeSecurity]
		public static extern int SetGammaRamp([In] ushort[] red, [In] ushort[] green, [In] ushort[] blue);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetGammaRamp"), SuppressUnmanagedCodeSecurity]
		public static extern int GetGammaRamp([In, Out] ushort[] red, [In, Out] ushort[] green, [In, Out] ushort[] blue);

		[DllImport(SDL_DLL, EntryPoint = "SDL_SetColors"), SuppressUnmanagedCodeSecurity]
		public static extern int SetColors(IntPtr /*Surface*/ surface, ref Color colors, int firstcolor, int ncolors);

		[DllImport(SDL_DLL, EntryPoint = "SDL_SetPalette"), SuppressUnmanagedCodeSecurity]
		public static extern int SetPalette(IntPtr /*Surface*/ surface, int flags, ref Color colors, int firstcolor, int ncolors);

		[DllImport(SDL_DLL, EntryPoint = "SDL_MapRGB"), SuppressUnmanagedCodeSecurity]
		public static extern uint MapRGB(ref PixelFormat format, byte r, byte g, byte b);

		[DllImport(SDL_DLL, EntryPoint = "SDL_MapRGBA"), SuppressUnmanagedCodeSecurity]
		public static extern uint MapRGBA(ref PixelFormat format, byte r, byte g, byte b, byte a);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetRGB"), SuppressUnmanagedCodeSecurity]
		public static extern void GetRGB(uint pixel, ref PixelFormat fmt, [In, Out] byte[] r, [In, Out] byte[] g, [In, Out] byte[] b);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetRGBA"), SuppressUnmanagedCodeSecurity]
		public static extern void GetRGBA(uint pixel, ref PixelFormat fmt, [In, Out] byte[] r, [In, Out] byte[] g, [In, Out] byte[] b, [In, Out] byte[] a);

		[DllImport(SDL_DLL, EntryPoint = "SDL_CreateRGBSurface"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*Surface*/ CreateRGBSurface(uint flags, int width, int height, int depth, uint Rmask, uint Gmask, uint Bmask, uint Amask);

		[DllImport(SDL_DLL, EntryPoint = "SDL_CreateRGBSurfaceFrom"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*Surface*/ CreateRGBSurfaceFrom(IntPtr pixels, int width, int height, int depth, int pitch, uint Rmask, uint Gmask, uint Bmask, uint Amask);

		[DllImport(SDL_DLL, EntryPoint = "SDL_FreeSurface"), SuppressUnmanagedCodeSecurity]
		public static extern void FreeSurface(IntPtr /*Surface*/ surface);

		[DllImport(SDL_DLL, EntryPoint = "SDL_LockSurface"), SuppressUnmanagedCodeSecurity]
		public static extern int LockSurface(IntPtr /*Surface*/ surface);

		[DllImport(SDL_DLL, EntryPoint = "SDL_UnlockSurface"), SuppressUnmanagedCodeSecurity]
		public static extern void UnlockSurface(IntPtr /*Surface*/ surface);

		[DllImport(SDL_DLL, EntryPoint = "SDL_LoadBMP_RW"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*Surface*/ LoadBMP_RW(ref RWops src, int freesrc);

		[DllImport(SDL_DLL, EntryPoint = "SDL_SaveBMP_RW"), SuppressUnmanagedCodeSecurity]
		public static extern int SaveBMP_RW(IntPtr /*Surface*/ surface, ref RWops dst, int freedst);

		[DllImport(SDL_DLL, EntryPoint = "SDL_SetColorKey"), SuppressUnmanagedCodeSecurity]
		public static extern int SetColorKey(IntPtr /*Surface*/ surface, uint flag, uint key);

		[DllImport(SDL_DLL, EntryPoint = "SDL_SetAlpha"), SuppressUnmanagedCodeSecurity]
		public static extern int SetAlpha(IntPtr /*Surface*/ surface, uint flag, byte alpha);

		[DllImport(SDL_DLL, EntryPoint = "SDL_SetClipRect"), SuppressUnmanagedCodeSecurity]
		public static extern bool SetClipRect(IntPtr /*Surface*/ surface, Rect rect);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetClipRect"), SuppressUnmanagedCodeSecurity]
		public static extern void GetClipRect(IntPtr /*Surface*/ surface, out Rect rect);

		[DllImport(SDL_DLL, EntryPoint = "SDL_ConvertSurface"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*Surface*/ ConvertSurface(IntPtr /*Surface*/ src, ref PixelFormat fmt, uint flags);

		[DllImport(SDL_DLL, EntryPoint = "SDL_UpperBlit"), SuppressUnmanagedCodeSecurity]
		public static extern int BlitSurface(IntPtr /*Surface*/ src, IntPtr /*Rect*/ srcrect, IntPtr /*Surface*/ dst, IntPtr /*Rect*/ dstrect);

		[DllImport(SDL_DLL, EntryPoint = "SDL_LowerBlit"), SuppressUnmanagedCodeSecurity]
		public static extern int LowerBlit(IntPtr /*Surface*/ src, IntPtr /*Rect*/ srcrect, IntPtr /*Surface*/ dst, IntPtr /*Rect*/ dstrect);

		[DllImport(SDL_DLL, EntryPoint = "SDL_FillRect"), SuppressUnmanagedCodeSecurity]
		public static extern int FillRect(IntPtr /*Surface*/ dst, Rect dstrect, uint color);

		[DllImport(SDL_DLL, EntryPoint = "SDL_DisplayFormat"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*Surface*/ DisplayFormat(IntPtr /*Surface*/ surface);

		[DllImport(SDL_DLL, EntryPoint = "SDL_DisplayFormatAlpha"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*Surface*/ DisplayFormatAlpha(IntPtr /*Surface*/ surface);

		[DllImport(SDL_DLL, EntryPoint = "SDL_CreateYUVOverlay"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*Overlay*/ CreateYUVOverlay(int width, int height, uint format, IntPtr /*Surface*/ display);

		[DllImport(SDL_DLL, EntryPoint = "SDL_LockYUVOverlay"), SuppressUnmanagedCodeSecurity]
		public static extern int LockYUVOverlay(ref Overlay overlay);

		[DllImport(SDL_DLL, EntryPoint = "SDL_UnlockYUVOverlay"), SuppressUnmanagedCodeSecurity]
		public static extern void UnlockYUVOverlay(ref Overlay overlay);

		[DllImport(SDL_DLL, EntryPoint = "SDL_DisplayYUVOverlay"), SuppressUnmanagedCodeSecurity]
		public static extern int DisplayYUVOverlay(ref Overlay overlay, Rect dstrect);

		[DllImport(SDL_DLL, EntryPoint = "SDL_FreeYUVOverlay"), SuppressUnmanagedCodeSecurity]
		public static extern void FreeYUVOverlay(ref Overlay overlay);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GL_LoadLibrary"), SuppressUnmanagedCodeSecurity]
		public static extern int GL_LoadLibrary(string path);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GL_SetAttribute"), SuppressUnmanagedCodeSecurity]
		public static extern int GL_SetAttribute(GLattr attr, int _value);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GL_GetAttribute"), SuppressUnmanagedCodeSecurity]
		public static extern int GL_GetAttribute(GLattr attr, [In, Out] int[] _value);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GL_SwapBuffers"), SuppressUnmanagedCodeSecurity]
		public static extern void GL_SwapBuffers();

		[DllImport(SDL_DLL, EntryPoint = "SDL_WM_SetCaption"), SuppressUnmanagedCodeSecurity]
		public static extern void WM_SetCaption(string title, string icon);

		[DllImport(SDL_DLL, EntryPoint = "SDL_WM_GetCaption"), SuppressUnmanagedCodeSecurity]
		public static extern void WM_GetCaption(IntPtr /*IntPtr sbyte*/ title, IntPtr /*IntPtr sbyte*/ icon);

		[DllImport(SDL_DLL, EntryPoint = "SDL_WM_IconifyWindow"), SuppressUnmanagedCodeSecurity]
		public static extern int WM_IconifyWindow();

		[DllImport(SDL_DLL, EntryPoint = "SDL_WM_ToggleFullScreen"), SuppressUnmanagedCodeSecurity]
		public static extern int WM_ToggleFullScreen(IntPtr /*Surface*/ surface);

		[DllImport(SDL_DLL, EntryPoint = "SDL_WM_GrabInput"), SuppressUnmanagedCodeSecurity]
		public static extern GrabMode WM_GrabInput(GrabMode mode);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetMouseState"), SuppressUnmanagedCodeSecurity]
		public static extern byte GetMouseState([In, Out] int[] x, [In, Out] int[] y);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetRelativeMouseState"), SuppressUnmanagedCodeSecurity]
		public static extern byte GetRelativeMouseState([In, Out] int[] x, [In, Out] int[] y);

		[DllImport(SDL_DLL, EntryPoint = "SDL_WarpMouse"), SuppressUnmanagedCodeSecurity]
		public static extern void WarpMouse(ushort x, ushort y);

		[DllImport(SDL_DLL, EntryPoint = "SDL_CreateCursor"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*Cursor*/ CreateCursor([In, Out] byte[] data, [In, Out] byte[] mask, int w, int h, int hot_x, int hot_y);

		[DllImport(SDL_DLL, EntryPoint = "SDL_SetCursor"), SuppressUnmanagedCodeSecurity]
		public static extern void SetCursor(IntPtr /*Cursor*/ cursor);

		[DllImport(SDL_DLL, EntryPoint = "SDL_GetCursor"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*Cursor*/ GetCursor();

		[DllImport(SDL_DLL, EntryPoint = "SDL_FreeCursor"), SuppressUnmanagedCodeSecurity]
		public static extern void FreeCursor(IntPtr /*Cursor*/ cursor);

		[DllImport(SDL_DLL, EntryPoint = "SDL_ShowCursor"), SuppressUnmanagedCodeSecurity]
		public static extern int ShowCursor(int toggle);

		[DllImport(SDL_DLL, EntryPoint = "SDL_PumpEvents"), SuppressUnmanagedCodeSecurity]
		public static extern void PumpEvents();

		[DllImport(SDL_DLL, EntryPoint = "SDL_PeepEvents"), SuppressUnmanagedCodeSecurity]
		public static extern int PeepEvents([Out] Event[] events, int numevents, EventAction action, uint mask);

		[DllImport(SDL_DLL, EntryPoint = "SDL_PollEvent"), SuppressUnmanagedCodeSecurity]
		public static extern int PollEvent(out Event _event);

		[DllImport(SDL_DLL, EntryPoint = "SDL_WaitEvent"), SuppressUnmanagedCodeSecurity]
		public static extern int WaitEvent(out Event _event);

		[DllImport(SDL_DLL, EntryPoint = "SDL_PushEvent"), SuppressUnmanagedCodeSecurity]
		public static extern int PushEvent(Event _event);

		[DllImport(SDL_DLL, EntryPoint = "SDL_EventState"), SuppressUnmanagedCodeSecurity]
		public static extern byte EventState(byte type, int state);

		[DllImport(SDL_DLL, EntryPoint = "SDL_Linked_Version"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*Version*/ Linked_Version();

		[DllImport(SDL_DLL, EntryPoint = "SDL_Init"), SuppressUnmanagedCodeSecurity]
		public static extern int Init(uint flags);

		[DllImport(SDL_DLL, EntryPoint = "SDL_InitSubSystem"), SuppressUnmanagedCodeSecurity]
		public static extern int InitSubSystem(uint flags);

		[DllImport(SDL_DLL, EntryPoint = "SDL_QuitSubSystem"), SuppressUnmanagedCodeSecurity]
		public static extern void QuitSubSystem(uint flags);

		[DllImport(SDL_DLL, EntryPoint = "SDL_WasInit"), SuppressUnmanagedCodeSecurity]
		public static extern uint WasInit(uint flags);

		[DllImport(SDL_DLL, EntryPoint = "SDL_Quit"), SuppressUnmanagedCodeSecurity]
		public static extern void Quit();


		[DllImport(SDL_DLL, EntryPoint = "SDL_WM_SetIcon")]
		public static extern void WM_SetIcon(IntPtr icon, IntPtr mask);

		public static string GetError()
		{
			return Marshal.PtrToStringAuto(_GetError());
		}

	}
}
