/* WARNING: Automatically generated file */
using System;
using System.Runtime.InteropServices;
using System.Security;


namespace OpenGl {
	public class gl {

        private const string GL_DLL = "GL.dll";

		public const uint _2_BYTES = 0x1407;
		public const uint _2D = 0x0600;
		public const uint _3_BYTES = 0x1408;
		public const uint _3D = 0x0601;
		public const uint _3D_COLOR = 0x0602;
		public const uint _3D_COLOR_TEXTURE = 0x0603;
		public const uint _4_BYTES = 0x1409;
		public const uint _4D_COLOR_TEXTURE = 0x0604;
		public const uint ABGR_EXT = 0x8000;
		public const uint ACCUM = 0x0100;
		public const uint ACCUM_ALPHA_BITS = 0x0D5B;
		public const uint ACCUM_BLUE_BITS = 0x0D5A;
		public const uint ACCUM_BUFFER_BIT = 0x00000200;
		public const uint ACCUM_CLEAR_VALUE = 0x0B80;
		public const uint ACCUM_GREEN_BITS = 0x0D59;
		public const uint ACCUM_RED_BITS = 0x0D58;
		public const uint ACTIVE_TEXTURE = 0x84E0;
		public const uint ADD = 0x0104;
		public const uint ADD_SIGNED = 0x8574;
		public const uint ALIASED_LINE_WIDTH_RANGE = 0x846E;
		public const uint ALIASED_POINT_SIZE_RANGE = 0x846D;
		public const uint ALL_ATTRIB_BITS = 0xFFFFFFFF;
		public const uint ALPHA = 0x1906;
		public const uint ALPHA_BIAS = 0x0D1D;
		public const uint ALPHA_BITS = 0x0D55;
		public const uint ALPHA_SCALE = 0x0D1C;
		public const uint ALPHA_TEST = 0x0BC0;
		public const uint ALPHA_TEST_FUNC = 0x0BC1;
		public const uint ALPHA_TEST_REF = 0x0BC2;
		public const uint ALPHA12 = 0x803D;
		public const uint ALPHA16 = 0x803E;
		public const uint ALPHA4 = 0x803B;
		public const uint ALPHA8 = 0x803C;
		public const uint ALWAYS = 0x0207;
		public const uint AMBIENT = 0x1200;
		public const uint AMBIENT_AND_DIFFUSE = 0x1602;
		public const uint AND = 0x1501;
		public const uint AND_INVERTED = 0x1504;
		public const uint AND_REVERSE = 0x1502;
		public const uint ATTRIB_STACK_DEPTH = 0x0BB0;
		public const uint AUTO_NORMAL = 0x0D80;
		public const uint AUX_BUFFERS = 0x0C00;
		public const uint AUX0 = 0x0409;
		public const uint AUX1 = 0x040A;
		public const uint AUX2 = 0x040B;
		public const uint AUX3 = 0x040C;
		public const uint BACK = 0x0405;
		public const uint BACK_LEFT = 0x0402;
		public const uint BACK_RIGHT = 0x0403;
		public const uint BGR = 0x80E0;
		public const uint BGRA = 0x80E1;
		public const uint BITMAP = 0x1A00;
		public const uint BITMAP_TOKEN = 0x0704;
		public const uint BLEND = 0x0BE2;
		public const uint BLEND_DST = 0x0BE0;
		public const uint BLEND_DST_ALPHA = 0x80CA;
		public const uint BLEND_DST_RGB = 0x80C8;
		public const uint BLEND_SRC = 0x0BE1;
		public const uint BLEND_SRC_ALPHA = 0x80CB;
		public const uint BLEND_SRC_RGB = 0x80C9;
		public const uint BLUE = 0x1905;
		public const uint BLUE_BIAS = 0x0D1B;
		public const uint BLUE_BITS = 0x0D54;
		public const uint BLUE_SCALE = 0x0D1A;
		public const uint BYTE = 0x1400;
		public const uint C3F_V3F = 0x2A24;
		public const uint C4F_N3F_V3F = 0x2A26;
		public const uint C4UB_V2F = 0x2A22;
		public const uint C4UB_V3F = 0x2A23;
		public const uint CCW = 0x0901;
		public const int CLAMP = 0x2900;
		public const int CLAMP_TO_BORDER = 0x812D;
		public const int CLAMP_TO_EDGE = 0x812F;
		public const uint CLEAR = 0x1500;
		public const uint CLIENT_ACTIVE_TEXTURE = 0x84E1;
		public const uint CLIENT_ALL_ATTRIB_BITS = 0xFFFFFFFF;
		public const uint CLIENT_ATTRIB_STACK_DEPTH = 0x0BB1;
		public const uint CLIENT_PIXEL_STORE_BIT = 0x00000001;
		public const uint CLIENT_VERTEX_ARRAY_BIT = 0x00000002;
		public const uint CLIP_PLANE0 = 0x3000;
		public const uint CLIP_PLANE1 = 0x3001;
		public const uint CLIP_PLANE2 = 0x3002;
		public const uint CLIP_PLANE3 = 0x3003;
		public const uint CLIP_PLANE4 = 0x3004;
		public const uint CLIP_PLANE5 = 0x3005;
		public const uint COEFF = 0x0A00;
		public const uint COLOR = 0x1800;
		public const uint COLOR_ARRAY = 0x8076;
		public const uint COLOR_ARRAY_COUNT_EXT = 0x8084;
		public const uint COLOR_ARRAY_EXT = 0x8076;
		public const uint COLOR_ARRAY_POINTER = 0x8090;
		public const uint COLOR_ARRAY_POINTER_EXT = 0x8090;
		public const uint COLOR_ARRAY_SIZE = 0x8081;
		public const uint COLOR_ARRAY_SIZE_EXT = 0x8081;
		public const uint COLOR_ARRAY_STRIDE = 0x8083;
		public const uint COLOR_ARRAY_STRIDE_EXT = 0x8083;
		public const uint COLOR_ARRAY_TYPE = 0x8082;
		public const uint COLOR_ARRAY_TYPE_EXT = 0x8082;
		public const uint COLOR_BUFFER_BIT = 0x00004000;
		public const uint COLOR_CLEAR_VALUE = 0x0C22;
		public const uint COLOR_INDEX = 0x1900;
		public const uint COLOR_INDEXES = 0x1603;
		public const uint COLOR_LOGIC_OP = 0x0BF2;
		public const uint COLOR_MATERIAL = 0x0B57;
		public const uint COLOR_MATERIAL_FACE = 0x0B55;
		public const uint COLOR_MATERIAL_PARAMETER = 0x0B56;
		public const uint COLOR_SUM = 0x8458;
		public const uint COLOR_WRITEMASK = 0x0C23;
		public const uint COMBINE = 0x8570;
		public const uint COMBINE_ALPHA = 0x8572;
		public const uint COMBINE_RGB = 0x8571;
		public const uint COMPARE_R_TO_TEXTURE = 0x884E;
		public const uint COMPILE = 0x1300;
		public const uint COMPILE_AND_EXECUTE = 0x1301;
		public const uint COMPRESSED_ALPHA = 0x84E9;
		public const uint COMPRESSED_INTENSITY = 0x84EC;
		public const uint COMPRESSED_LUMINANCE = 0x84EA;
		public const uint COMPRESSED_LUMINANCE_ALPHA = 0x84EB;
		public const uint COMPRESSED_RGB = 0x84ED;
		public const uint COMPRESSED_RGBA = 0x84EE;
		public const uint COMPRESSED_TEXTURE_FORMATS = 0x86A3;
		public const uint CONSTANT = 0x8576;
		public const uint CONSTANT_ATTENUATION = 0x1207;
		public const uint COPY = 0x1503;
		public const uint COPY_INVERTED = 0x150C;
		public const uint COPY_PIXEL_TOKEN = 0x0706;
		public const uint CULL_FACE = 0x0B44;
		public const uint CULL_FACE_MODE = 0x0B45;
		public const uint CURRENT_BIT = 0x00000001;
		public const uint CURRENT_COLOR = 0x0B00;
		public const uint CURRENT_FOG_COORDINATE = 0x8453;
		public const uint CURRENT_INDEX = 0x0B01;
		public const uint CURRENT_NORMAL = 0x0B02;
		public const uint CURRENT_RASTER_COLOR = 0x0B04;
		public const uint CURRENT_RASTER_DISTANCE = 0x0B09;
		public const uint CURRENT_RASTER_INDEX = 0x0B05;
		public const uint CURRENT_RASTER_POSITION = 0x0B07;
		public const uint CURRENT_RASTER_POSITION_VALID = 0x0B08;
		public const uint CURRENT_RASTER_TEXTURE_COORDS = 0x0B06;
		public const uint CURRENT_SECONDARY_COLOR = 0x8459;
		public const uint CURRENT_TEXTURE_COORDS = 0x0B03;
		public const uint CW = 0x0900;
		public const uint DECAL = 0x2101;
		public const uint DECR = 0x1E03;
		public const uint DECR_WRAP = 0x8508;
		public const uint DEPTH = 0x1801;
		public const uint DEPTH_BIAS = 0x0D1F;
		public const uint DEPTH_BITS = 0x0D56;
		public const uint DEPTH_BUFFER_BIT = 0x00000100;
		public const uint DEPTH_CLEAR_VALUE = 0x0B73;
		public const uint DEPTH_COMPONENT = 0x1902;
		public const uint DEPTH_COMPONENT16 = 0x81A5;
		public const uint DEPTH_COMPONENT16_SGIX = 0x81A5;
		public const uint DEPTH_COMPONENT24 = 0x81A6;
		public const uint DEPTH_COMPONENT24_SGIX = 0x81A6;
		public const uint DEPTH_COMPONENT32 = 0x81A7;
		public const uint DEPTH_COMPONENT32_SGIX = 0x81A7;
		public const uint DEPTH_FUNC = 0x0B74;
		public const uint DEPTH_RANGE = 0x0B70;
		public const uint DEPTH_SCALE = 0x0D1E;
		public const uint DEPTH_TEST = 0x0B71;
		public const uint DEPTH_TEXTURE_MODE = 0x884B;
		public const uint DEPTH_WRITEMASK = 0x0B72;
		public const uint DIFFUSE = 0x1201;
		public const uint DITHER = 0x0BD0;
		public const uint DOMAIN = 0x0A02;
		public const uint DONT_CARE = 0x1100;
		public const uint DOT3_RGB = 0x86AE;
		public const uint DOT3_RGBA = 0x86AF;
		public const uint DOUBLE = 0x140A;
		public const uint DOUBLE_EXT = 0x140A;
		public const uint DOUBLEBUFFER = 0x0C32;
		public const uint DRAW_BUFFER = 0x0C01;
		public const uint DRAW_PIXEL_TOKEN = 0x0705;
		public const uint DST_ALPHA = 0x0304;
		public const uint DST_COLOR = 0x0306;
		public const uint EDGE_FLAG = 0x0B43;
		public const uint EDGE_FLAG_ARRAY = 0x8079;
		public const uint EDGE_FLAG_ARRAY_COUNT_EXT = 0x808D;
		public const uint EDGE_FLAG_ARRAY_EXT = 0x8079;
		public const uint EDGE_FLAG_ARRAY_POINTER = 0x8093;
		public const uint EDGE_FLAG_ARRAY_POINTER_EXT = 0x8093;
		public const uint EDGE_FLAG_ARRAY_STRIDE = 0x808C;
		public const uint EDGE_FLAG_ARRAY_STRIDE_EXT = 0x808C;
		public const uint EMISSION = 0x1600;
		public const uint ENABLE_BIT = 0x00002000;
		public const uint EQUAL = 0x0202;
		public const uint EQUIV = 0x1509;
		public const uint EVAL_BIT = 0x00010000;
		public const uint EXP = 0x0800;
		public const uint EXP2 = 0x0801;
		public const uint EXTENSIONS = 0x1F03;
		public const uint EYE_LINEAR = 0x2400;
		public const uint EYE_PLANE = 0x2502;
		public const uint FALSE = 0;
		public const uint FASTEST = 0x1101;
		public const uint FEEDBACK = 0x1C01;
		public const uint FEEDBACK_BUFFER_POINTER = 0x0DF0;
		public const uint FEEDBACK_BUFFER_SIZE = 0x0DF1;
		public const uint FEEDBACK_BUFFER_TYPE = 0x0DF2;
		public const uint FILL = 0x1B02;
		public const uint FLAT = 0x1D00;
		public const uint FLOAT = 0x1406;
		public const uint FOG = 0x0B60;
		public const uint FOG_BIT = 0x00000080;
		public const uint FOG_COLOR = 0x0B66;
		public const uint FOG_COORDINATE = 0x8451;
		public const uint FOG_COORDINATE_ARRAY = 0x8457;
		public const uint FOG_COORDINATE_ARRAY_POINTER = 0x8456;
		public const uint FOG_COORDINATE_ARRAY_STRIDE = 0x8455;
		public const uint FOG_COORDINATE_ARRAY_TYPE = 0x8454;
		public const uint FOG_COORDINATE_SOURCE = 0x8450;
		public const uint FOG_DENSITY = 0x0B62;
		public const uint FOG_END = 0x0B64;
		public const uint FOG_HINT = 0x0C54;
		public const uint FOG_INDEX = 0x0B61;
		public const uint FOG_MODE = 0x0B65;
		public const uint FOG_START = 0x0B63;
		public const uint FRAGMENT_DEPTH = 0x8452;
		public const uint FRONT = 0x0404;
		public const uint FRONT_AND_BACK = 0x0408;
		public const uint FRONT_FACE = 0x0B46;
		public const uint FRONT_LEFT = 0x0400;
		public const uint FRONT_RIGHT = 0x0401;
		public const uint FUNC_REVERSE_SUBTRACT_EXT = 0x800B;
		public const uint FUNC_SUBTRACT_EXT = 0x800A;
		public const uint GENERATE_MIPMAP = 0x8191;
		public const uint GENERATE_MIPMAP_HINT = 0x8192;
		public const uint GENERATE_MIPMAP_HINT_SGIS = 0x8192;
		public const uint GENERATE_MIPMAP_SGIS = 0x8191;
		public const uint GEQUAL = 0x0206;
		public const uint GREATER = 0x0204;
		public const uint GREEN = 0x1904;
		public const uint GREEN_BIAS = 0x0D19;
		public const uint GREEN_BITS = 0x0D53;
		public const uint GREEN_SCALE = 0x0D18;
		public const uint HINT_BIT = 0x00008000;
		public const uint INCR = 0x1E02;
		public const uint INCR_WRAP = 0x8507;
		public const uint INDEX_ARRAY = 0x8077;
		public const uint INDEX_ARRAY_COUNT_EXT = 0x8087;
		public const uint INDEX_ARRAY_EXT = 0x8077;
		public const uint INDEX_ARRAY_POINTER = 0x8091;
		public const uint INDEX_ARRAY_POINTER_EXT = 0x8091;
		public const uint INDEX_ARRAY_STRIDE = 0x8086;
		public const uint INDEX_ARRAY_STRIDE_EXT = 0x8086;
		public const uint INDEX_ARRAY_TYPE = 0x8085;
		public const uint INDEX_ARRAY_TYPE_EXT = 0x8085;
		public const uint INDEX_BITS = 0x0D51;
		public const uint INDEX_CLEAR_VALUE = 0x0C20;
		public const uint INDEX_LOGIC_OP = 0x0BF1;
		public const uint INDEX_MODE = 0x0C30;
		public const uint INDEX_OFFSET = 0x0D13;
		public const uint INDEX_SHIFT = 0x0D12;
		public const uint INDEX_WRITEMASK = 0x0C21;
		public const uint INT = 0x1404;
		public const uint INTENSITY = 0x8049;
		public const uint INTENSITY12 = 0x804C;
		public const uint INTENSITY16 = 0x804D;
		public const uint INTENSITY4 = 0x804A;
		public const uint INTENSITY8 = 0x804B;
		public const uint INTERPOLATE = 0x8575;
		public const uint INVALID_ENUM = 0x0500;
		public const uint INVALID_OPERATION = 0x0502;
		public const uint INVALID_VALUE = 0x0501;
		public const uint INVERT = 0x150A;
		public const uint KEEP = 0x1E00;
		public const uint LEFT = 0x0406;
		public const uint LEQUAL = 0x0203;
		public const uint LESS = 0x0201;
		public const uint LIGHT_MODEL_AMBIENT = 0x0B53;
		public const uint LIGHT_MODEL_COLOR_CONTROL = 0x81F8;
		public const uint LIGHT_MODEL_LOCAL_VIEWER = 0x0B51;
		public const uint LIGHT_MODEL_TWO_SIDE = 0x0B52;
		public const uint LIGHT0 = 0x4000;
		public const uint LIGHT1 = 0x4001;
		public const uint LIGHT2 = 0x4002;
		public const uint LIGHT3 = 0x4003;
		public const uint LIGHT4 = 0x4004;
		public const uint LIGHT5 = 0x4005;
		public const uint LIGHT6 = 0x4006;
		public const uint LIGHT7 = 0x4007;
		public const uint LIGHTING = 0x0B50;
		public const uint LIGHTING_BIT = 0x00000040;
		public const uint LINE = 0x1B01;
		public const uint LINE_BIT = 0x00000004;
		public const uint LINE_LOOP = 0x0002;
		public const uint LINE_RESET_TOKEN = 0x0707;
		public const uint LINE_SMOOTH = 0x0B20;
		public const uint LINE_SMOOTH_HINT = 0x0C52;
		public const uint LINE_STIPPLE = 0x0B24;
		public const uint LINE_STIPPLE_PATTERN = 0x0B25;
		public const uint LINE_STIPPLE_REPEAT = 0x0B26;
		public const uint LINE_STRIP = 0x0003;
		public const uint LINE_TOKEN = 0x0702;
		public const uint LINE_WIDTH = 0x0B21;
		public const uint LINE_WIDTH_GRANULARITY = 0x0B23;
		public const uint LINE_WIDTH_RANGE = 0x0B22;
		public const int LINEAR = 0x2601;
		public const int LINEAR_ATTENUATION = 0x1208;
		public const int LINEAR_MIPMAP_LINEAR = 0x2703;
		public const int LINEAR_MIPMAP_NEAREST = 0x2701;
		public const uint LINES = 0x0001;
		public const uint LIST_BASE = 0x0B32;
		public const uint LIST_BIT = 0x00020000;
		public const uint LIST_INDEX = 0x0B33;
		public const uint LIST_MODE = 0x0B30;
		public const uint LOAD = 0x0101;
		public const uint LOGIC_OP = 0x0BF1;
		public const uint LOGIC_OP_MODE = 0x0BF0;
		public const uint LUMINANCE = 0x1909;
		public const uint LUMINANCE_ALPHA = 0x190A;
		public const uint LUMINANCE12 = 0x8041;
		public const uint LUMINANCE12_ALPHA12 = 0x8047;
		public const uint LUMINANCE12_ALPHA4 = 0x8046;
		public const uint LUMINANCE16 = 0x8042;
		public const uint LUMINANCE16_ALPHA16 = 0x8048;
		public const uint LUMINANCE4 = 0x803F;
		public const uint LUMINANCE4_ALPHA4 = 0x8043;
		public const uint LUMINANCE6_ALPHA2 = 0x8044;
		public const uint LUMINANCE8 = 0x8040;
		public const uint LUMINANCE8_ALPHA8 = 0x8045;
		public const uint MAP_COLOR = 0x0D10;
		public const uint MAP_STENCIL = 0x0D11;
		public const uint MAP1_COLOR_4 = 0x0D90;
		public const uint MAP1_GRID_DOMAIN = 0x0DD0;
		public const uint MAP1_GRID_SEGMENTS = 0x0DD1;
		public const uint MAP1_INDEX = 0x0D91;
		public const uint MAP1_NORMAL = 0x0D92;
		public const uint MAP1_TEXTURE_COORD_1 = 0x0D93;
		public const uint MAP1_TEXTURE_COORD_2 = 0x0D94;
		public const uint MAP1_TEXTURE_COORD_3 = 0x0D95;
		public const uint MAP1_TEXTURE_COORD_4 = 0x0D96;
		public const uint MAP1_VERTEX_3 = 0x0D97;
		public const uint MAP1_VERTEX_4 = 0x0D98;
		public const uint MAP2_COLOR_4 = 0x0DB0;
		public const uint MAP2_GRID_DOMAIN = 0x0DD2;
		public const uint MAP2_GRID_SEGMENTS = 0x0DD3;
		public const uint MAP2_INDEX = 0x0DB1;
		public const uint MAP2_NORMAL = 0x0DB2;
		public const uint MAP2_TEXTURE_COORD_1 = 0x0DB3;
		public const uint MAP2_TEXTURE_COORD_2 = 0x0DB4;
		public const uint MAP2_TEXTURE_COORD_3 = 0x0DB5;
		public const uint MAP2_TEXTURE_COORD_4 = 0x0DB6;
		public const uint MAP2_VERTEX_3 = 0x0DB7;
		public const uint MAP2_VERTEX_4 = 0x0DB8;
		public const uint MATRIX_MODE = 0x0BA0;
		public const uint MAX_3D_TEXTURE_SIZE = 0x8073;
		public const uint MAX_ATTRIB_STACK_DEPTH = 0x0D35;
		public const uint MAX_CLIENT_ATTRIB_STACK_DEPTH = 0x0D3B;
		public const uint MAX_CLIP_PLANES = 0x0D32;
		public const uint MAX_CUBE_MAP_TEXTURE_SIZE = 0x851C;
		public const uint MAX_ELEMENTS_INDICES = 0x80E9;
		public const uint MAX_ELEMENTS_VERTICES = 0x80E8;
		public const uint MAX_EVAL_ORDER = 0x0D30;
		public const uint MAX_LIGHTS = 0x0D31;
		public const uint MAX_LIST_NESTING = 0x0B31;
		public const uint MAX_MODELVIEW_STACK_DEPTH = 0x0D36;
		public const uint MAX_NAME_STACK_DEPTH = 0x0D37;
		public const uint MAX_PIXEL_MAP_TABLE = 0x0D34;
		public const uint MAX_PROJECTION_STACK_DEPTH = 0x0D38;
		public const uint MAX_TEXTURE_LOD_BIAS = 0x84FD;
		public const uint MAX_TEXTURE_SIZE = 0x0D33;
		public const uint MAX_TEXTURE_STACK_DEPTH = 0x0D39;
		public const uint MAX_TEXTURE_UNITS = 0x84E2;
		public const uint MAX_VIEWPORT_DIMS = 0x0D3A;
		public const uint MIRRORED_REPEAT = 0x8370;
		public const uint MODELVIEW = 0x1700;
		public const uint MODELVIEW_MATRIX = 0x0BA6;
		public const uint MODELVIEW_STACK_DEPTH = 0x0BA3;
		public const uint MODULATE = 0x2100;
		public const uint MULT = 0x0103;
		public const uint MULTISAMPLE = 0x809D;
		public const uint MULTISAMPLE_BIT = 0x20000000;
		public const uint N3F_V3F = 0x2A25;
		public const uint NAME_STACK_DEPTH = 0x0D70;
		public const uint NAND = 0x150E;
		public const int NEAREST = 0x2600;
		public const int NEAREST_MIPMAP_LINEAR = 0x2702;
		public const int NEAREST_MIPMAP_NEAREST = 0x2700;
		public const uint NEVER = 0x0200;
		public const uint NICEST = 0x1102;
		public const uint NO_ERROR = 0;
		public const uint NONE = 0;
		public const uint NOOP = 0x1505;
		public const uint NOR = 0x1508;
		public const uint NORMAL_ARRAY = 0x8075;
		public const uint NORMAL_ARRAY_COUNT_EXT = 0x8080;
		public const uint NORMAL_ARRAY_EXT = 0x8075;
		public const uint NORMAL_ARRAY_POINTER = 0x808F;
		public const uint NORMAL_ARRAY_POINTER_EXT = 0x808F;
		public const uint NORMAL_ARRAY_STRIDE = 0x807F;
		public const uint NORMAL_ARRAY_STRIDE_EXT = 0x807F;
		public const uint NORMAL_ARRAY_TYPE = 0x807E;
		public const uint NORMAL_ARRAY_TYPE_EXT = 0x807E;
		public const uint NORMAL_MAP = 0x8511;
		public const uint NORMALIZE = 0x0BA1;
		public const uint NOTEQUAL = 0x0205;
		public const uint NUM_COMPRESSED_TEXTURE_FORMATS = 0x86A2;
		public const uint OBJECT_LINEAR = 0x2401;
		public const uint OBJECT_PLANE = 0x2501;
		public const uint ONE = 1;
		public const uint ONE_MINUS_DST_ALPHA = 0x0305;
		public const uint ONE_MINUS_DST_COLOR = 0x0307;
		public const uint ONE_MINUS_SRC_ALPHA = 0x0303;
		public const uint ONE_MINUS_SRC_COLOR = 0x0301;
		public const uint OPERAND0_ALPHA = 0x8598;
		public const uint OPERAND0_RGB = 0x8590;
		public const uint OPERAND1_ALPHA = 0x8599;
		public const uint OPERAND1_RGB = 0x8591;
		public const uint OPERAND2_ALPHA = 0x859A;
		public const uint OPERAND2_RGB = 0x8592;
		public const uint OR = 0x1507;
		public const uint OR_INVERTED = 0x150D;
		public const uint OR_REVERSE = 0x150B;
		public const uint ORDER = 0x0A01;
		public const uint OUT_OF_MEMORY = 0x0505;
		public const uint PACK_ALIGNMENT = 0x0D05;
		public const uint PACK_IMAGE_HEIGHT = 0x806C;
		public const uint PACK_LSB_FIRST = 0x0D01;
		public const uint PACK_ROW_LENGTH = 0x0D02;
		public const uint PACK_SKIP_IMAGES = 0x806B;
		public const uint PACK_SKIP_PIXELS = 0x0D04;
		public const uint PACK_SKIP_ROWS = 0x0D03;
		public const uint PACK_SWAP_BYTES = 0x0D00;
		public const uint PASS_THROUGH_TOKEN = 0x0700;
		public const uint PERSPECTIVE_CORRECTION_HINT = 0x0C50;
		public const uint PIXEL_MAP_A_TO_A = 0x0C79;
		public const uint PIXEL_MAP_A_TO_A_SIZE = 0x0CB9;
		public const uint PIXEL_MAP_B_TO_B = 0x0C78;
		public const uint PIXEL_MAP_B_TO_B_SIZE = 0x0CB8;
		public const uint PIXEL_MAP_G_TO_G = 0x0C77;
		public const uint PIXEL_MAP_G_TO_G_SIZE = 0x0CB7;
		public const uint PIXEL_MAP_I_TO_A = 0x0C75;
		public const uint PIXEL_MAP_I_TO_A_SIZE = 0x0CB5;
		public const uint PIXEL_MAP_I_TO_B = 0x0C74;
		public const uint PIXEL_MAP_I_TO_B_SIZE = 0x0CB4;
		public const uint PIXEL_MAP_I_TO_G = 0x0C73;
		public const uint PIXEL_MAP_I_TO_G_SIZE = 0x0CB3;
		public const uint PIXEL_MAP_I_TO_I = 0x0C70;
		public const uint PIXEL_MAP_I_TO_I_SIZE = 0x0CB0;
		public const uint PIXEL_MAP_I_TO_R = 0x0C72;
		public const uint PIXEL_MAP_I_TO_R_SIZE = 0x0CB2;
		public const uint PIXEL_MAP_R_TO_R = 0x0C76;
		public const uint PIXEL_MAP_R_TO_R_SIZE = 0x0CB6;
		public const uint PIXEL_MAP_S_TO_S = 0x0C71;
		public const uint PIXEL_MAP_S_TO_S_SIZE = 0x0CB1;
		public const uint PIXEL_MODE_BIT = 0x00000020;
		public const uint POINT = 0x1B00;
		public const uint POINT_BIT = 0x00000002;
		public const uint POINT_DISTANCE_ATTENUATION = 0x8129;
		public const uint POINT_FADE_THRESHOLD_SIZE = 0x8128;
		public const uint POINT_SIZE = 0x0B11;
		public const uint POINT_SIZE_GRANULARITY = 0x0B13;
		public const uint POINT_SIZE_MAX = 0x8127;
		public const uint POINT_SIZE_MIN = 0x8126;
		public const uint POINT_SIZE_RANGE = 0x0B12;
		public const uint POINT_SMOOTH = 0x0B10;
		public const uint POINT_SMOOTH_HINT = 0x0C51;
		public const uint POINT_TOKEN = 0x0701;
		public const uint POINTS = 0x0000;
		public const uint POLYGON = 0x0009;
		public const uint POLYGON_BIT = 0x00000008;
		public const uint POLYGON_MODE = 0x0B40;
		public const uint POLYGON_OFFSET_FACTOR = 0x8038;
		public const uint POLYGON_OFFSET_FILL = 0x8037;
		public const uint POLYGON_OFFSET_LINE = 0x2A02;
		public const uint POLYGON_OFFSET_POINT = 0x2A01;
		public const uint POLYGON_OFFSET_UNITS = 0x2A00;
		public const uint POLYGON_SMOOTH = 0x0B41;
		public const uint POLYGON_SMOOTH_HINT = 0x0C53;
		public const uint POLYGON_STIPPLE = 0x0B42;
		public const uint POLYGON_STIPPLE_BIT = 0x00000010;
		public const uint POLYGON_TOKEN = 0x0703;
		public const uint POSITION = 0x1203;
		public const uint PREVIOUS = 0x8578;
		public const uint PRIMARY_COLOR = 0x8577;
		public const uint PROJECTION = 0x1701;
		public const uint PROJECTION_MATRIX = 0x0BA7;
		public const uint PROJECTION_STACK_DEPTH = 0x0BA4;
		public const uint PROXY_TEXTURE_1D = 0x8063;
		public const uint PROXY_TEXTURE_2D = 0x8064;
		public const uint PROXY_TEXTURE_3D = 0x8070;
		public const uint PROXY_TEXTURE_CUBE_MAP = 0x851B;
		public const uint Q = 0x2003;
		public const uint QUAD_STRIP = 0x0008;
		public const uint QUADRATIC_ATTENUATION = 0x1209;
		public const uint QUADS = 0x0007;
		public const uint R = 0x2002;
		public const uint R3_G3_B2 = 0x2A10;
		public const uint READ_BUFFER = 0x0C02;
		public const uint RED = 0x1903;
		public const uint RED_BIAS = 0x0D15;
		public const uint RED_BITS = 0x0D52;
		public const uint RED_SCALE = 0x0D14;
		public const uint REFLECTION_MAP = 0x8512;
		public const uint RENDER = 0x1C00;
		public const uint RENDER_MODE = 0x0C40;
		public const uint RENDERER = 0x1F01;
		public const int REPEAT = 0x2901;
		public const uint REPLACE = 0x1E01;
		public const uint RESCALE_NORMAL = 0x803A;
		public const uint RESCALE_NORMAL_EXT = 0x803A;
		public const uint RETURN = 0x0102;
		public const uint RGB = 0x1907;
		public const uint RGB_SCALE = 0x8573;
		public const uint RGB10 = 0x8052;
		public const uint RGB10_A2 = 0x8059;
		public const uint RGB12 = 0x8053;
		public const uint RGB16 = 0x8054;
		public const uint RGB4 = 0x804F;
		public const uint RGB5 = 0x8050;
		public const uint RGB5_A1 = 0x8057;
		public const uint RGB8 = 0x8051;
		public const uint RGBA = 0x1908;
		public const uint RGBA_MODE = 0x0C31;
		public const uint RGBA12 = 0x805A;
		public const uint RGBA16 = 0x805B;
		public const uint RGBA2 = 0x8055;
		public const uint RGBA4 = 0x8056;
		public const uint RGBA8 = 0x8058;
		public const uint RIGHT = 0x0407;
		public const uint S = 0x2000;
		public const uint SAMPLE_ALPHA_TO_COVERAGE = 0x809E;
		public const uint SAMPLE_ALPHA_TO_ONE = 0x809F;
		public const uint SAMPLE_BUFFERS = 0x80A8;
		public const uint SAMPLE_COVERAGE = 0x80A0;
		public const uint SAMPLE_COVERAGE_INVERT = 0x80AB;
		public const uint SAMPLE_COVERAGE_VALUE = 0x80AA;
		public const uint SAMPLES = 0x80A9;
		public const uint SCISSOR_BIT = 0x00080000;
		public const uint SCISSOR_BOX = 0x0C10;
		public const uint SCISSOR_TEST = 0x0C11;
		public const uint SECONDARY_COLOR_ARRAY = 0x845E;
		public const uint SECONDARY_COLOR_ARRAY_POINTER = 0x845D;
		public const uint SECONDARY_COLOR_ARRAY_SIZE = 0x845A;
		public const uint SECONDARY_COLOR_ARRAY_STRIDE = 0x845C;
		public const uint SECONDARY_COLOR_ARRAY_TYPE = 0x845B;
		public const uint SELECT = 0x1C02;
		public const uint SELECTION_BUFFER_POINTER = 0x0DF3;
		public const uint SELECTION_BUFFER_SIZE = 0x0DF4;
		public const uint SEPARATE_SPECULAR_COLOR = 0x81FA;
		public const uint SET = 0x150F;
		public const uint SHADE_MODEL = 0x0B54;
		public const uint SHARED_TEXTURE_PALETTE_EXT = 0x81FB;
		public const uint SHININESS = 0x1601;
		public const uint SHORT = 0x1402;
		public const uint SINGLE_COLOR = 0x81F9;
		public const uint SMOOTH = 0x1D01;
		public const uint SMOOTH_LINE_WIDTH_GRANULARITY = 0x0B23;
		public const uint SMOOTH_LINE_WIDTH_RANGE = 0x0B22;
		public const uint SMOOTH_POINT_SIZE_GRANULARITY = 0x0B13;
		public const uint SMOOTH_POINT_SIZE_RANGE = 0x0B12;
		public const uint SOURCE0_ALPHA = 0x8588;
		public const uint SOURCE0_RGB = 0x8580;
		public const uint SOURCE1_ALPHA = 0x8589;
		public const uint SOURCE1_RGB = 0x8581;
		public const uint SOURCE2_ALPHA = 0x858A;
		public const uint SOURCE2_RGB = 0x8582;
		public const uint SPECULAR = 0x1202;
		public const uint SPHERE_MAP = 0x2402;
		public const uint SPOT_CUTOFF = 0x1206;
		public const uint SPOT_DIRECTION = 0x1204;
		public const uint SPOT_EXPONENT = 0x1205;
		public const uint SRC_ALPHA = 0x0302;
		public const uint SRC_ALPHA_SATURATE = 0x0308;
		public const uint SRC_COLOR = 0x0300;
		public const uint STACK_OVERFLOW = 0x0503;
		public const uint STACK_UNDERFLOW = 0x0504;
		public const uint STENCIL = 0x1802;
		public const uint STENCIL_BITS = 0x0D57;
		public const uint STENCIL_BUFFER_BIT = 0x00000400;
		public const uint STENCIL_CLEAR_VALUE = 0x0B91;
		public const uint STENCIL_FAIL = 0x0B94;
		public const uint STENCIL_FUNC = 0x0B92;
		public const uint STENCIL_INDEX = 0x1901;
		public const uint STENCIL_PASS_DEPTH_FAIL = 0x0B95;
		public const uint STENCIL_PASS_DEPTH_PASS = 0x0B96;
		public const uint STENCIL_REF = 0x0B97;
		public const uint STENCIL_TEST = 0x0B90;
		public const uint STENCIL_VALUE_MASK = 0x0B93;
		public const uint STENCIL_WRITEMASK = 0x0B98;
		public const uint STEREO = 0x0C33;
		public const uint SUBPIXEL_BITS = 0x0D50;
		public const uint SUBTRACT = 0x84E7;
		public const uint T = 0x2001;
		public const uint T2F_C3F_V3F = 0x2A2A;
		public const uint T2F_C4F_N3F_V3F = 0x2A2C;
		public const uint T2F_C4UB_V3F = 0x2A29;
		public const uint T2F_N3F_V3F = 0x2A2B;
		public const uint T2F_V3F = 0x2A27;
		public const uint T4F_C4F_N3F_V4F = 0x2A2D;
		public const uint T4F_V4F = 0x2A28;
		public const uint TABLE_TOO_LARGE = 0x8031;
		public const uint TEXTURE = 0x1702;
		public const uint TEXTURE_1D = 0x0DE0;
		public const uint TEXTURE_2D = 0x0DE1;
		public const uint TEXTURE_3D = 0x806F;
		public const uint TEXTURE_ALPHA_SIZE = 0x805F;
		public const uint TEXTURE_BASE_LEVEL = 0x813C;
		public const uint TEXTURE_BASE_LEVEL_SGIS = 0x813C;
		public const uint TEXTURE_BINDING_1D = 0x8068;
		public const uint TEXTURE_BINDING_2D = 0x8069;
		public const uint TEXTURE_BINDING_3D = 0x806A;
		public const uint TEXTURE_BINDING_CUBE_MAP = 0x8514;
		public const uint TEXTURE_BIT = 0x00040000;
		public const uint TEXTURE_BLUE_SIZE = 0x805E;
		public const uint TEXTURE_BORDER = 0x1005;
		public const uint TEXTURE_BORDER_COLOR = 0x1004;
		public const uint TEXTURE_COMPARE_FUNC = 0x884D;
		public const uint TEXTURE_COMPARE_MODE = 0x884C;
		public const uint TEXTURE_COMPARE_OPERATOR_SGIX = 0x819B;
		public const uint TEXTURE_COMPARE_SGIX = 0x819A;
		public const uint TEXTURE_COMPONENTS = 0x1003;
		public const uint TEXTURE_COMPRESSED = 0x86A1;
		public const uint TEXTURE_COMPRESSED_IMAGE_SIZE = 0x86A0;
		public const uint TEXTURE_COMPRESSION_HINT = 0x84EF;
		public const uint TEXTURE_COORD_ARRAY = 0x8078;
		public const uint TEXTURE_COORD_ARRAY_COUNT_EXT = 0x808B;
		public const uint TEXTURE_COORD_ARRAY_EXT = 0x8078;
		public const uint TEXTURE_COORD_ARRAY_POINTER = 0x8092;
		public const uint TEXTURE_COORD_ARRAY_POINTER_EXT = 0x8092;
		public const uint TEXTURE_COORD_ARRAY_SIZE = 0x8088;
		public const uint TEXTURE_COORD_ARRAY_SIZE_EXT = 0x8088;
		public const uint TEXTURE_COORD_ARRAY_STRIDE = 0x808A;
		public const uint TEXTURE_COORD_ARRAY_STRIDE_EXT = 0x808A;
		public const uint TEXTURE_COORD_ARRAY_TYPE = 0x8089;
		public const uint TEXTURE_COORD_ARRAY_TYPE_EXT = 0x8089;
		public const uint TEXTURE_CUBE_MAP = 0x8513;
		public const uint TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516;
		public const uint TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518;
		public const uint TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A;
		public const uint TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515;
		public const uint TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517;
		public const uint TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519;
		public const uint TEXTURE_DEPTH = 0x8071;
		public const uint TEXTURE_DEPTH_SIZE = 0x884A;
		public const uint TEXTURE_ENV = 0x2300;
		public const uint TEXTURE_ENV_COLOR = 0x2201;
		public const uint TEXTURE_ENV_MODE = 0x2200;
		public const uint TEXTURE_FILTER_CONTROL = 0x8500;
		public const uint TEXTURE_GEN_MODE = 0x2500;
		public const uint TEXTURE_GEN_Q = 0x0C63;
		public const uint TEXTURE_GEN_R = 0x0C62;
		public const uint TEXTURE_GEN_S = 0x0C60;
		public const uint TEXTURE_GEN_T = 0x0C61;
		public const uint TEXTURE_GEQUAL_R_SGIX = 0x819D;
		public const uint TEXTURE_GREEN_SIZE = 0x805D;
		public const uint TEXTURE_HEIGHT = 0x1001;
		public const uint TEXTURE_INTENSITY_SIZE = 0x8061;
		public const uint TEXTURE_INTERNAL_FORMAT = 0x1003;
		public const uint TEXTURE_LEQUAL_R_SGIX = 0x819C;
		public const uint TEXTURE_LOD_BIAS = 0x8501;
		public const uint TEXTURE_LUMINANCE_SIZE = 0x8060;
		public const uint TEXTURE_MAG_FILTER = 0x2800;
		public const uint TEXTURE_MATRIX = 0x0BA8;
		public const uint TEXTURE_MAX_LEVEL = 0x813D;
		public const uint TEXTURE_MAX_LEVEL_SGIS = 0x813D;
		public const uint TEXTURE_MAX_LOD = 0x813B;
		public const uint TEXTURE_MAX_LOD_SGIS = 0x813B;
		public const uint TEXTURE_MIN_FILTER = 0x2801;
		public const uint TEXTURE_MIN_LOD = 0x813A;
		public const uint TEXTURE_MIN_LOD_SGIS = 0x813A;
		public const uint TEXTURE_PRIORITY = 0x8066;
		public const uint TEXTURE_RED_SIZE = 0x805C;
		public const uint TEXTURE_RESIDENT = 0x8067;
		public const uint TEXTURE_STACK_DEPTH = 0x0BA5;
		public const uint TEXTURE_WIDTH = 0x1000;
		public const uint TEXTURE_WRAP_R = 0x8072;
		public const uint TEXTURE_WRAP_S = 0x2802;
		public const uint TEXTURE_WRAP_T = 0x2803;
		public const uint TEXTURE0 = 0x84C0;
		public const uint TEXTURE1 = 0x84C1;
		public const uint TEXTURE10 = 0x84CA;
		public const uint TEXTURE11 = 0x84CB;
		public const uint TEXTURE12 = 0x84CC;
		public const uint TEXTURE13 = 0x84CD;
		public const uint TEXTURE14 = 0x84CE;
		public const uint TEXTURE15 = 0x84CF;
		public const uint TEXTURE16 = 0x84D0;
		public const uint TEXTURE17 = 0x84D1;
		public const uint TEXTURE18 = 0x84D2;
		public const uint TEXTURE19 = 0x84D3;
		public const uint TEXTURE2 = 0x84C2;
		public const uint TEXTURE20 = 0x84D4;
		public const uint TEXTURE21 = 0x84D5;
		public const uint TEXTURE22 = 0x84D6;
		public const uint TEXTURE23 = 0x84D7;
		public const uint TEXTURE24 = 0x84D8;
		public const uint TEXTURE25 = 0x84D9;
		public const uint TEXTURE26 = 0x84DA;
		public const uint TEXTURE27 = 0x84DB;
		public const uint TEXTURE28 = 0x84DC;
		public const uint TEXTURE29 = 0x84DD;
		public const uint TEXTURE3 = 0x84C3;
		public const uint TEXTURE30 = 0x84DE;
		public const uint TEXTURE31 = 0x84DF;
		public const uint TEXTURE4 = 0x84C4;
		public const uint TEXTURE5 = 0x84C5;
		public const uint TEXTURE6 = 0x84C6;
		public const uint TEXTURE7 = 0x84C7;
		public const uint TEXTURE8 = 0x84C8;
		public const uint TEXTURE9 = 0x84C9;
		public const uint TRANSFORM_BIT = 0x00001000;
		public const uint TRANSPOSE_COLOR_MATRIX = 0x84E6;
		public const uint TRANSPOSE_MODELVIEW_MATRIX = 0x84E3;
		public const uint TRANSPOSE_PROJECTION_MATRIX = 0x84E4;
		public const uint TRANSPOSE_TEXTURE_MATRIX = 0x84E5;
		public const uint TRIANGLE_FAN = 0x0006;
		public const uint TRIANGLE_STRIP = 0x0005;
		public const uint TRIANGLES = 0x0004;
		public const uint TRUE = 1;
		public const uint UNPACK_ALIGNMENT = 0x0CF5;
		public const uint UNPACK_IMAGE_HEIGHT = 0x806E;
		public const uint UNPACK_LSB_FIRST = 0x0CF1;
		public const uint UNPACK_ROW_LENGTH = 0x0CF2;
		public const uint UNPACK_SKIP_IMAGES = 0x806D;
		public const uint UNPACK_SKIP_PIXELS = 0x0CF4;
		public const uint UNPACK_SKIP_ROWS = 0x0CF3;
		public const uint UNPACK_SWAP_BYTES = 0x0CF0;
		public const uint UNSIGNED_BYTE = 0x1401;
		public const uint UNSIGNED_BYTE_2_3_3_REV = 0x8362;
		public const uint UNSIGNED_BYTE_3_3_2 = 0x8032;
		public const uint UNSIGNED_BYTE_3_3_2_EXT = 0x8032;
		public const uint UNSIGNED_INT = 0x1405;
		public const uint UNSIGNED_INT_10_10_10_2 = 0x8036;
		public const uint UNSIGNED_INT_10_10_10_2_EXT = 0x8036;
		public const uint UNSIGNED_INT_2_10_10_10_REV = 0x8368;
		public const uint UNSIGNED_INT_8_8_8_8 = 0x8035;
		public const uint UNSIGNED_INT_8_8_8_8_EXT = 0x8035;
		public const uint UNSIGNED_INT_8_8_8_8_REV = 0x8367;
		public const uint UNSIGNED_SHORT = 0x1403;
		public const uint UNSIGNED_SHORT_1_5_5_5_REV = 0x8366;
		public const uint UNSIGNED_SHORT_4_4_4_4 = 0x8033;
		public const uint UNSIGNED_SHORT_4_4_4_4_EXT = 0x8033;
		public const uint UNSIGNED_SHORT_4_4_4_4_REV = 0x8365;
		public const uint UNSIGNED_SHORT_5_5_5_1 = 0x8034;
		public const uint UNSIGNED_SHORT_5_5_5_1_EXT = 0x8034;
		public const uint UNSIGNED_SHORT_5_6_5 = 0x8363;
		public const uint UNSIGNED_SHORT_5_6_5_REV = 0x8364;
		public const uint V2F = 0x2A20;
		public const uint V3F = 0x2A21;
		public const uint VENDOR = 0x1F00;
		public const uint VERSION = 0x1F02;
		public const uint VERSION_1_1 = 1;
		public const uint VERTEX_ARRAY = 0x8074;
		public const uint VERTEX_ARRAY_COUNT_EXT = 0x807D;
		public const uint VERTEX_ARRAY_EXT = 0x8074;
		public const uint VERTEX_ARRAY_POINTER = 0x808E;
		public const uint VERTEX_ARRAY_POINTER_EXT = 0x808E;
		public const uint VERTEX_ARRAY_SIZE = 0x807A;
		public const uint VERTEX_ARRAY_SIZE_EXT = 0x807A;
		public const uint VERTEX_ARRAY_STRIDE = 0x807C;
		public const uint VERTEX_ARRAY_STRIDE_EXT = 0x807C;
		public const uint VERTEX_ARRAY_TYPE = 0x807B;
		public const uint VERTEX_ARRAY_TYPE_EXT = 0x807B;
		public const uint VIEWPORT = 0x0BA2;
		public const uint VIEWPORT_BIT = 0x00000800;
		public const uint XOR = 0x1506;
		public const uint ZERO = 0;
		public const uint ZOOM_X = 0x0D16;
		public const uint ZOOM_Y = 0x0D17;

		[DllImport(GL_DLL, EntryPoint="glAccum"), SuppressUnmanagedCodeSecurity]
		public static extern void Accum(uint op, float _value);

		[DllImport(GL_DLL, EntryPoint="glAlphaFunc"), SuppressUnmanagedCodeSecurity]
		public static extern void AlphaFunc(uint func, float _ref);

		[DllImport(GL_DLL, EntryPoint="glAreTexturesResident"), SuppressUnmanagedCodeSecurity]
		public static extern byte AreTexturesResident(int n, [In] uint[] textures, [In, Out] byte[] residences);

		[DllImport(GL_DLL, EntryPoint="glArrayElement"), SuppressUnmanagedCodeSecurity]
		public static extern void ArrayElement(int i);

		[DllImport(GL_DLL, EntryPoint="glBegin"), SuppressUnmanagedCodeSecurity]
		public static extern void Begin(uint mode);

		[DllImport(GL_DLL, EntryPoint="glBindTexture"), SuppressUnmanagedCodeSecurity]
		public static extern void BindTexture(uint target, uint texture);

		[DllImport(GL_DLL, EntryPoint="glBitmap"), SuppressUnmanagedCodeSecurity]
		public static extern void Bitmap(int width, int height, float xorig, float yorig, float xmove, float ymove, [In] byte[] bitmap);

		[DllImport(GL_DLL, EntryPoint="glBlendFunc"), SuppressUnmanagedCodeSecurity]
		public static extern void BlendFunc(uint sfactor, uint dfactor);

		[DllImport(GL_DLL, EntryPoint="glCallList"), SuppressUnmanagedCodeSecurity]
		public static extern void CallList(uint list);

		[DllImport(GL_DLL, EntryPoint="glCallLists"), SuppressUnmanagedCodeSecurity]
		public static extern void CallLists(int n, uint type, IntPtr lists);

		[DllImport(GL_DLL, EntryPoint="glClear"), SuppressUnmanagedCodeSecurity]
		public static extern void Clear(uint mask);

		[DllImport(GL_DLL, EntryPoint="glClearAccum"), SuppressUnmanagedCodeSecurity]
		public static extern void ClearAccum(float red, float green, float blue, float alpha);

		[DllImport(GL_DLL, EntryPoint="glClearColor"), SuppressUnmanagedCodeSecurity]
		public static extern void ClearColor(float red, float green, float blue, float alpha);

		[DllImport(GL_DLL, EntryPoint="glClearDepth"), SuppressUnmanagedCodeSecurity]
		public static extern void ClearDepth(double depth);

		[DllImport(GL_DLL, EntryPoint="glClearIndex"), SuppressUnmanagedCodeSecurity]
		public static extern void ClearIndex(float c);

		[DllImport(GL_DLL, EntryPoint="glClearStencil"), SuppressUnmanagedCodeSecurity]
		public static extern void ClearStencil(int s);

		[DllImport(GL_DLL, EntryPoint="glClipPlane"), SuppressUnmanagedCodeSecurity]
		public static extern void ClipPlane(uint plane, [In] double[] equation);

		[DllImport(GL_DLL, EntryPoint="glColor3b"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3b(sbyte red, sbyte green, sbyte blue);

		[DllImport(GL_DLL, EntryPoint="glColor3bv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3bv(string v);

		[DllImport(GL_DLL, EntryPoint="glColor3d"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3d(double red, double green, double blue);

		[DllImport(GL_DLL, EntryPoint="glColor3dv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3dv([In] double[] v);

		[DllImport(GL_DLL, EntryPoint="glColor3f"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3f(float red, float green, float blue);

		[DllImport(GL_DLL, EntryPoint="glColor3fv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3fv([In] float[] v);

		[DllImport(GL_DLL, EntryPoint="glColor3i"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3i(int red, int green, int blue);

		[DllImport(GL_DLL, EntryPoint="glColor3iv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3iv([In] int[] v);

		[DllImport(GL_DLL, EntryPoint="glColor3s"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3s(short red, short green, short blue);

		[DllImport(GL_DLL, EntryPoint="glColor3sv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3sv([In] short[] v);

		[DllImport(GL_DLL, EntryPoint="glColor3ub"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3ub(byte red, byte green, byte blue);

		[DllImport(GL_DLL, EntryPoint="glColor3ubv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3ubv([In] byte[] v);

		[DllImport(GL_DLL, EntryPoint="glColor3ui"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3ui(uint red, uint green, uint blue);

		[DllImport(GL_DLL, EntryPoint="glColor3uiv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3uiv([In] uint[] v);

		[DllImport(GL_DLL, EntryPoint="glColor3us"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3us(ushort red, ushort green, ushort blue);

		[DllImport(GL_DLL, EntryPoint="glColor3usv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color3usv([In] ushort[] v);

		[DllImport(GL_DLL, EntryPoint="glColor4b"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4b(sbyte red, sbyte green, sbyte blue, sbyte alpha);

		[DllImport(GL_DLL, EntryPoint="glColor4bv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4bv(string v);

		[DllImport(GL_DLL, EntryPoint="glColor4d"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4d(double red, double green, double blue, double alpha);

		[DllImport(GL_DLL, EntryPoint="glColor4dv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4dv([In] double[] v);

		[DllImport(GL_DLL, EntryPoint="glColor4f"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4f(float red, float green, float blue, float alpha);

		[DllImport(GL_DLL, EntryPoint="glColor4fv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4fv([In] float[] v);

		[DllImport(GL_DLL, EntryPoint="glColor4i"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4i(int red, int green, int blue, int alpha);

		[DllImport(GL_DLL, EntryPoint="glColor4iv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4iv([In] int[] v);

		[DllImport(GL_DLL, EntryPoint="glColor4s"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4s(short red, short green, short blue, short alpha);

		[DllImport(GL_DLL, EntryPoint="glColor4sv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4sv([In] short[] v);

		[DllImport(GL_DLL, EntryPoint="glColor4ub"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4ub(byte red, byte green, byte blue, byte alpha);

		[DllImport(GL_DLL, EntryPoint="glColor4ubv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4ubv([In] byte[] v);

		[DllImport(GL_DLL, EntryPoint="glColor4ui"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4ui(uint red, uint green, uint blue, uint alpha);

		[DllImport(GL_DLL, EntryPoint="glColor4uiv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4uiv([In] uint[] v);

		[DllImport(GL_DLL, EntryPoint="glColor4us"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4us(ushort red, ushort green, ushort blue, ushort alpha);

		[DllImport(GL_DLL, EntryPoint="glColor4usv"), SuppressUnmanagedCodeSecurity]
		public static extern void Color4usv([In] ushort[] v);

		[DllImport(GL_DLL, EntryPoint="glColorMask"), SuppressUnmanagedCodeSecurity]
		public static extern void ColorMask(byte red, byte green, byte blue, byte alpha);

		[DllImport(GL_DLL, EntryPoint="glColorMaterial"), SuppressUnmanagedCodeSecurity]
		public static extern void ColorMaterial(uint face, uint mode);

		[DllImport(GL_DLL, EntryPoint="glColorPointer"), SuppressUnmanagedCodeSecurity]
		public static extern void ColorPointer(int size, uint type, int stride, IntPtr pointer);

		[DllImport(GL_DLL, EntryPoint="glCopyPixels"), SuppressUnmanagedCodeSecurity]
		public static extern void CopyPixels(int x, int y, int width, int height, uint type);

		[DllImport(GL_DLL, EntryPoint="glCopyTexImage1D"), SuppressUnmanagedCodeSecurity]
		public static extern void CopyTexImage1D(uint target, int level, uint internalFormat, int x, int y, int width, int border);

		[DllImport(GL_DLL, EntryPoint="glCopyTexImage2D"), SuppressUnmanagedCodeSecurity]
		public static extern void CopyTexImage2D(uint target, int level, uint internalFormat, int x, int y, int width, int height, int border);

		[DllImport(GL_DLL, EntryPoint="glCopyTexSubImage1D"), SuppressUnmanagedCodeSecurity]
		public static extern void CopyTexSubImage1D(uint target, int level, int xoffset, int x, int y, int width);

		[DllImport(GL_DLL, EntryPoint="glCopyTexSubImage2D"), SuppressUnmanagedCodeSecurity]
		public static extern void CopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height);

		[DllImport(GL_DLL, EntryPoint="glCullFace"), SuppressUnmanagedCodeSecurity]
		public static extern void CullFace(uint mode);

		[DllImport(GL_DLL, EntryPoint="glDeleteLists"), SuppressUnmanagedCodeSecurity]
		public static extern void DeleteLists(uint list, int range);

		[DllImport(GL_DLL, EntryPoint="glDeleteTextures"), SuppressUnmanagedCodeSecurity]
		public static extern void DeleteTextures(int n, [In] uint[] textures);

		[DllImport(GL_DLL, EntryPoint="glDepthFunc"), SuppressUnmanagedCodeSecurity]
		public static extern void DepthFunc(uint func);

		[DllImport(GL_DLL, EntryPoint="glDepthMask"), SuppressUnmanagedCodeSecurity]
		public static extern void DepthMask(byte flag);

		[DllImport(GL_DLL, EntryPoint="glDepthRange"), SuppressUnmanagedCodeSecurity]
		public static extern void DepthRange(double zNear, double zFar);

		[DllImport(GL_DLL, EntryPoint="glDisable"), SuppressUnmanagedCodeSecurity]
		public static extern void Disable(uint cap);

		[DllImport(GL_DLL, EntryPoint="glDisableClientState"), SuppressUnmanagedCodeSecurity]
		public static extern void DisableClientState(uint array);

		[DllImport(GL_DLL, EntryPoint="glDrawArrays"), SuppressUnmanagedCodeSecurity]
		public static extern void DrawArrays(uint mode, int first, int count);

		[DllImport(GL_DLL, EntryPoint="glDrawBuffer"), SuppressUnmanagedCodeSecurity]
		public static extern void DrawBuffer(uint mode);

		[DllImport(GL_DLL, EntryPoint="glDrawElements"), SuppressUnmanagedCodeSecurity]
		public static extern void DrawElements(uint mode, int count, uint type, IntPtr indices);

		[DllImport(GL_DLL, EntryPoint="glDrawPixels"), SuppressUnmanagedCodeSecurity]
		public static extern void DrawPixels(int width, int height, uint format, uint type, IntPtr pixels);

		[DllImport(GL_DLL, EntryPoint="glEdgeFlag"), SuppressUnmanagedCodeSecurity]
		public static extern void EdgeFlag(byte flag);

		[DllImport(GL_DLL, EntryPoint="glEdgeFlagPointer"), SuppressUnmanagedCodeSecurity]
		public static extern void EdgeFlagPointer(int stride, IntPtr pointer);

		[DllImport(GL_DLL, EntryPoint="glEdgeFlagv"), SuppressUnmanagedCodeSecurity]
		public static extern void EdgeFlagv([In] byte[] flag);

		[DllImport(GL_DLL, EntryPoint="glEnable"), SuppressUnmanagedCodeSecurity]
		public static extern void Enable(uint cap);

		[DllImport(GL_DLL, EntryPoint="glEnableClientState"), SuppressUnmanagedCodeSecurity]
		public static extern void EnableClientState(uint array);

		[DllImport(GL_DLL, EntryPoint="glEnd"), SuppressUnmanagedCodeSecurity]
		public static extern void End();

		[DllImport(GL_DLL, EntryPoint="glEndList"), SuppressUnmanagedCodeSecurity]
		public static extern void EndList();

		[DllImport(GL_DLL, EntryPoint="glEvalCoord1d"), SuppressUnmanagedCodeSecurity]
		public static extern void EvalCoord1d(double u);

		[DllImport(GL_DLL, EntryPoint="glEvalCoord1dv"), SuppressUnmanagedCodeSecurity]
		public static extern void EvalCoord1dv([In] double[] u);

		[DllImport(GL_DLL, EntryPoint="glEvalCoord1f"), SuppressUnmanagedCodeSecurity]
		public static extern void EvalCoord1f(float u);

		[DllImport(GL_DLL, EntryPoint="glEvalCoord1fv"), SuppressUnmanagedCodeSecurity]
		public static extern void EvalCoord1fv([In] float[] u);

		[DllImport(GL_DLL, EntryPoint="glEvalCoord2d"), SuppressUnmanagedCodeSecurity]
		public static extern void EvalCoord2d(double u, double v);

		[DllImport(GL_DLL, EntryPoint="glEvalCoord2dv"), SuppressUnmanagedCodeSecurity]
		public static extern void EvalCoord2dv([In] double[] u);

		[DllImport(GL_DLL, EntryPoint="glEvalCoord2f"), SuppressUnmanagedCodeSecurity]
		public static extern void EvalCoord2f(float u, float v);

		[DllImport(GL_DLL, EntryPoint="glEvalCoord2fv"), SuppressUnmanagedCodeSecurity]
		public static extern void EvalCoord2fv([In] float[] u);

		[DllImport(GL_DLL, EntryPoint="glEvalMesh1"), SuppressUnmanagedCodeSecurity]
		public static extern void EvalMesh1(uint mode, int i1, int i2);

		[DllImport(GL_DLL, EntryPoint="glEvalMesh2"), SuppressUnmanagedCodeSecurity]
		public static extern void EvalMesh2(uint mode, int i1, int i2, int j1, int j2);

		[DllImport(GL_DLL, EntryPoint="glEvalPoint1"), SuppressUnmanagedCodeSecurity]
		public static extern void EvalPoint1(int i);

		[DllImport(GL_DLL, EntryPoint="glEvalPoint2"), SuppressUnmanagedCodeSecurity]
		public static extern void EvalPoint2(int i, int j);

		[DllImport(GL_DLL, EntryPoint="glFeedbackBuffer"), SuppressUnmanagedCodeSecurity]
		public static extern void FeedbackBuffer(int size, uint type, [In, Out] float[] buffer);

		[DllImport(GL_DLL, EntryPoint="glFinish"), SuppressUnmanagedCodeSecurity]
		public static extern void Finish();

		[DllImport(GL_DLL, EntryPoint="glFlush"), SuppressUnmanagedCodeSecurity]
		public static extern void Flush();

		[DllImport(GL_DLL, EntryPoint="glFogf"), SuppressUnmanagedCodeSecurity]
		public static extern void Fogf(uint pname, float param);

		[DllImport(GL_DLL, EntryPoint="glFogfv"), SuppressUnmanagedCodeSecurity]
		public static extern void Fogfv(uint pname, [In] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glFogi"), SuppressUnmanagedCodeSecurity]
		public static extern void Fogi(uint pname, int param);

		[DllImport(GL_DLL, EntryPoint="glFogiv"), SuppressUnmanagedCodeSecurity]
		public static extern void Fogiv(uint pname, [In] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glFrontFace"), SuppressUnmanagedCodeSecurity]
		public static extern void FrontFace(uint mode);

		[DllImport(GL_DLL, EntryPoint="glFrustum"), SuppressUnmanagedCodeSecurity]
		public static extern void Frustum(double left, double right, double bottom, double top, double zNear, double zFar);

		[DllImport(GL_DLL, EntryPoint="glGenLists"), SuppressUnmanagedCodeSecurity]
		public static extern uint GenLists(int range);

		[DllImport(GL_DLL, EntryPoint="glGenTextures"), SuppressUnmanagedCodeSecurity]
		public static extern void GenTextures(int n, [In, Out] uint[] textures);

		[DllImport(GL_DLL, EntryPoint="glGetBooleanv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetBooleanv(uint pname, [In, Out] byte[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetClipPlane"), SuppressUnmanagedCodeSecurity]
		public static extern void GetClipPlane(uint plane, [In, Out] double[] equation);

		[DllImport(GL_DLL, EntryPoint="glGetDoublev"), SuppressUnmanagedCodeSecurity]
		public static extern void GetDoublev(uint pname, [In, Out] double[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetError"), SuppressUnmanagedCodeSecurity]
		public static extern uint GetError();

		[DllImport(GL_DLL, EntryPoint="glGetFloatv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetFloatv(uint pname, [In, Out] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetIntegerv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetIntegerv(uint pname, [In, Out] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetLightfv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetLightfv(uint light, uint pname, [In, Out] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetLightiv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetLightiv(uint light, uint pname, [In, Out] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetMapdv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetMapdv(uint target, uint query, [In, Out] double[] v);

		[DllImport(GL_DLL, EntryPoint="glGetMapfv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetMapfv(uint target, uint query, [In, Out] float[] v);

		[DllImport(GL_DLL, EntryPoint="glGetMapiv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetMapiv(uint target, uint query, [In, Out] int[] v);

		[DllImport(GL_DLL, EntryPoint="glGetMaterialfv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetMaterialfv(uint face, uint pname, [In, Out] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetMaterialiv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetMaterialiv(uint face, uint pname, [In, Out] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetPixelMapfv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetPixelMapfv(uint map, [In, Out] float[] values);

		[DllImport(GL_DLL, EntryPoint="glGetPixelMapuiv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetPixelMapuiv(uint map, [In, Out] uint[] values);

		[DllImport(GL_DLL, EntryPoint="glGetPixelMapusv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetPixelMapusv(uint map, [In, Out] ushort[] values);

		[DllImport(GL_DLL, EntryPoint="glGetPointerv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetPointerv(uint pname, IntPtr /*IntPtr void*/ _params);

		[DllImport(GL_DLL, EntryPoint="glGetPolygonStipple"), SuppressUnmanagedCodeSecurity]
		public static extern void GetPolygonStipple([In, Out] byte[] mask);

		[DllImport(GL_DLL, EntryPoint="glGetString"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr /*byte*/ GetString(uint name);

		[DllImport(GL_DLL, EntryPoint="glGetTexEnvfv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetTexEnvfv(uint target, uint pname, [In, Out] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetTexEnviv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetTexEnviv(uint target, uint pname, [In, Out] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetTexGendv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetTexGendv(uint coord, uint pname, [In, Out] double[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetTexGenfv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetTexGenfv(uint coord, uint pname, [In, Out] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetTexGeniv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetTexGeniv(uint coord, uint pname, [In, Out] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetTexImage"), SuppressUnmanagedCodeSecurity]
		public static extern void GetTexImage(uint target, int level, uint format, uint type, IntPtr pixels);

		[DllImport(GL_DLL, EntryPoint="glGetTexLevelParameterfv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetTexLevelParameterfv(uint target, int level, uint pname, [In, Out] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetTexLevelParameteriv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetTexLevelParameteriv(uint target, int level, uint pname, [In, Out] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetTexParameterfv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetTexParameterfv(uint target, uint pname, [In, Out] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glGetTexParameteriv"), SuppressUnmanagedCodeSecurity]
		public static extern void GetTexParameteriv(uint target, uint pname, [In, Out] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glHint"), SuppressUnmanagedCodeSecurity]
		public static extern void Hint(uint target, uint mode);

		[DllImport(GL_DLL, EntryPoint="glIndexMask"), SuppressUnmanagedCodeSecurity]
		public static extern void IndexMask(uint mask);

		[DllImport(GL_DLL, EntryPoint="glIndexPointer"), SuppressUnmanagedCodeSecurity]
		public static extern void IndexPointer(uint type, int stride, IntPtr pointer);

		[DllImport(GL_DLL, EntryPoint="glIndexd"), SuppressUnmanagedCodeSecurity]
		public static extern void Indexd(double c);

		[DllImport(GL_DLL, EntryPoint="glIndexdv"), SuppressUnmanagedCodeSecurity]
		public static extern void Indexdv([In] double[] c);

		[DllImport(GL_DLL, EntryPoint="glIndexf"), SuppressUnmanagedCodeSecurity]
		public static extern void Indexf(float c);

		[DllImport(GL_DLL, EntryPoint="glIndexfv"), SuppressUnmanagedCodeSecurity]
		public static extern void Indexfv([In] float[] c);

		[DllImport(GL_DLL, EntryPoint="glIndexi"), SuppressUnmanagedCodeSecurity]
		public static extern void Indexi(int c);

		[DllImport(GL_DLL, EntryPoint="glIndexiv"), SuppressUnmanagedCodeSecurity]
		public static extern void Indexiv([In] int[] c);

		[DllImport(GL_DLL, EntryPoint="glIndexs"), SuppressUnmanagedCodeSecurity]
		public static extern void Indexs(short c);

		[DllImport(GL_DLL, EntryPoint="glIndexsv"), SuppressUnmanagedCodeSecurity]
		public static extern void Indexsv([In] short[] c);

		[DllImport(GL_DLL, EntryPoint="glIndexub"), SuppressUnmanagedCodeSecurity]
		public static extern void Indexub(byte c);

		[DllImport(GL_DLL, EntryPoint="glIndexubv"), SuppressUnmanagedCodeSecurity]
		public static extern void Indexubv([In] byte[] c);

		[DllImport(GL_DLL, EntryPoint="glInitNames"), SuppressUnmanagedCodeSecurity]
		public static extern void InitNames();

		[DllImport(GL_DLL, EntryPoint="glInterleavedArrays"), SuppressUnmanagedCodeSecurity]
		public static extern void InterleavedArrays(uint format, int stride, IntPtr pointer);

		[DllImport(GL_DLL, EntryPoint="glIsEnabled"), SuppressUnmanagedCodeSecurity]
		public static extern byte IsEnabled(uint cap);

		[DllImport(GL_DLL, EntryPoint="glIsList"), SuppressUnmanagedCodeSecurity]
		public static extern byte IsList(uint list);

		[DllImport(GL_DLL, EntryPoint="glIsTexture"), SuppressUnmanagedCodeSecurity]
		public static extern byte IsTexture(uint texture);

		[DllImport(GL_DLL, EntryPoint="glLightModelf"), SuppressUnmanagedCodeSecurity]
		public static extern void LightModelf(uint pname, float param);

		[DllImport(GL_DLL, EntryPoint="glLightModelfv"), SuppressUnmanagedCodeSecurity]
		public static extern void LightModelfv(uint pname, [In] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glLightModeli"), SuppressUnmanagedCodeSecurity]
		public static extern void LightModeli(uint pname, int param);

		[DllImport(GL_DLL, EntryPoint="glLightModeliv"), SuppressUnmanagedCodeSecurity]
		public static extern void LightModeliv(uint pname, [In] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glLightf"), SuppressUnmanagedCodeSecurity]
		public static extern void Lightf(uint light, uint pname, float param);

		[DllImport(GL_DLL, EntryPoint="glLightfv"), SuppressUnmanagedCodeSecurity]
		public static extern void Lightfv(uint light, uint pname, [In] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glLighti"), SuppressUnmanagedCodeSecurity]
		public static extern void Lighti(uint light, uint pname, int param);

		[DllImport(GL_DLL, EntryPoint="glLightiv"), SuppressUnmanagedCodeSecurity]
		public static extern void Lightiv(uint light, uint pname, [In] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glLineStipple"), SuppressUnmanagedCodeSecurity]
		public static extern void LineStipple(int factor, ushort pattern);

		[DllImport(GL_DLL, EntryPoint="glLineWidth"), SuppressUnmanagedCodeSecurity]
		public static extern void LineWidth(float width);

		[DllImport(GL_DLL, EntryPoint="glListBase"), SuppressUnmanagedCodeSecurity]
		public static extern void ListBase(uint _base);

		[DllImport(GL_DLL, EntryPoint="glLoadIdentity"), SuppressUnmanagedCodeSecurity]
		public static extern void LoadIdentity();

		[DllImport(GL_DLL, EntryPoint="glLoadMatrixd"), SuppressUnmanagedCodeSecurity]
		public static extern void LoadMatrixd([In] double[] m);

		[DllImport(GL_DLL, EntryPoint="glLoadMatrixf"), SuppressUnmanagedCodeSecurity]
		public static extern void LoadMatrixf([In] float[] m);

		[DllImport(GL_DLL, EntryPoint="glLoadName"), SuppressUnmanagedCodeSecurity]
		public static extern void LoadName(uint name);

		[DllImport(GL_DLL, EntryPoint="glLogicOp"), SuppressUnmanagedCodeSecurity]
		public static extern void LogicOp(uint opcode);

		[DllImport(GL_DLL, EntryPoint="glMap1d"), SuppressUnmanagedCodeSecurity]
		public static extern void Map1d(uint target, double u1, double u2, int stride, int order, [In] double[] points);

		[DllImport(GL_DLL, EntryPoint="glMap1f"), SuppressUnmanagedCodeSecurity]
		public static extern void Map1f(uint target, float u1, float u2, int stride, int order, [In] float[] points);

		[DllImport(GL_DLL, EntryPoint="glMap2d"), SuppressUnmanagedCodeSecurity]
		public static extern void Map2d(uint target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder, [In] double[] points);

		[DllImport(GL_DLL, EntryPoint="glMap2f"), SuppressUnmanagedCodeSecurity]
		public static extern void Map2f(uint target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder, [In] float[] points);

		[DllImport(GL_DLL, EntryPoint="glMapGrid1d"), SuppressUnmanagedCodeSecurity]
		public static extern void MapGrid1d(int un, double u1, double u2);

		[DllImport(GL_DLL, EntryPoint="glMapGrid1f"), SuppressUnmanagedCodeSecurity]
		public static extern void MapGrid1f(int un, float u1, float u2);

		[DllImport(GL_DLL, EntryPoint="glMapGrid2d"), SuppressUnmanagedCodeSecurity]
		public static extern void MapGrid2d(int un, double u1, double u2, int vn, double v1, double v2);

		[DllImport(GL_DLL, EntryPoint="glMapGrid2f"), SuppressUnmanagedCodeSecurity]
		public static extern void MapGrid2f(int un, float u1, float u2, int vn, float v1, float v2);

		[DllImport(GL_DLL, EntryPoint="glMaterialf"), SuppressUnmanagedCodeSecurity]
		public static extern void Materialf(uint face, uint pname, float param);

		[DllImport(GL_DLL, EntryPoint="glMaterialfv"), SuppressUnmanagedCodeSecurity]
		public static extern void Materialfv(uint face, uint pname, [In] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glMateriali"), SuppressUnmanagedCodeSecurity]
		public static extern void Materiali(uint face, uint pname, int param);

		[DllImport(GL_DLL, EntryPoint="glMaterialiv"), SuppressUnmanagedCodeSecurity]
		public static extern void Materialiv(uint face, uint pname, [In] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glMatrixMode"), SuppressUnmanagedCodeSecurity]
		public static extern void MatrixMode(uint mode);

		[DllImport(GL_DLL, EntryPoint="glMultMatrixd"), SuppressUnmanagedCodeSecurity]
		public static extern void MultMatrixd([In] double[] m);

		[DllImport(GL_DLL, EntryPoint="glMultMatrixf"), SuppressUnmanagedCodeSecurity]
		public static extern void MultMatrixf([In] float[] m);

		[DllImport(GL_DLL, EntryPoint="glNewList"), SuppressUnmanagedCodeSecurity]
		public static extern void NewList(uint list, uint mode);

		[DllImport(GL_DLL, EntryPoint="glNormal3b"), SuppressUnmanagedCodeSecurity]
		public static extern void Normal3b(sbyte nx, sbyte ny, sbyte nz);

		[DllImport(GL_DLL, EntryPoint="glNormal3bv"), SuppressUnmanagedCodeSecurity]
		public static extern void Normal3bv(string v);

		[DllImport(GL_DLL, EntryPoint="glNormal3d"), SuppressUnmanagedCodeSecurity]
		public static extern void Normal3d(double nx, double ny, double nz);

		[DllImport(GL_DLL, EntryPoint="glNormal3dv"), SuppressUnmanagedCodeSecurity]
		public static extern void Normal3dv([In] double[] v);

		[DllImport(GL_DLL, EntryPoint="glNormal3f"), SuppressUnmanagedCodeSecurity]
		public static extern void Normal3f(float nx, float ny, float nz);

		[DllImport(GL_DLL, EntryPoint="glNormal3fv"), SuppressUnmanagedCodeSecurity]
		public static extern void Normal3fv([In] float[] v);

		[DllImport(GL_DLL, EntryPoint="glNormal3i"), SuppressUnmanagedCodeSecurity]
		public static extern void Normal3i(int nx, int ny, int nz);

		[DllImport(GL_DLL, EntryPoint="glNormal3iv"), SuppressUnmanagedCodeSecurity]
		public static extern void Normal3iv([In] int[] v);

		[DllImport(GL_DLL, EntryPoint="glNormal3s"), SuppressUnmanagedCodeSecurity]
		public static extern void Normal3s(short nx, short ny, short nz);

		[DllImport(GL_DLL, EntryPoint="glNormal3sv"), SuppressUnmanagedCodeSecurity]
		public static extern void Normal3sv([In] short[] v);

		[DllImport(GL_DLL, EntryPoint="glNormalPointer"), SuppressUnmanagedCodeSecurity]
		public static extern void NormalPointer(uint type, int stride, IntPtr pointer);

		[DllImport(GL_DLL, EntryPoint="glOrtho"), SuppressUnmanagedCodeSecurity]
		public static extern void Ortho(double left, double right, double bottom, double top, double zNear, double zFar);

		[DllImport(GL_DLL, EntryPoint="glPassThrough"), SuppressUnmanagedCodeSecurity]
		public static extern void PassThrough(float token);

		[DllImport(GL_DLL, EntryPoint="glPixelMapfv"), SuppressUnmanagedCodeSecurity]
		public static extern void PixelMapfv(uint map, int mapsize, [In] float[] values);

		[DllImport(GL_DLL, EntryPoint="glPixelMapuiv"), SuppressUnmanagedCodeSecurity]
		public static extern void PixelMapuiv(uint map, int mapsize, [In] uint[] values);

		[DllImport(GL_DLL, EntryPoint="glPixelMapusv"), SuppressUnmanagedCodeSecurity]
		public static extern void PixelMapusv(uint map, int mapsize, [In] ushort[] values);

		[DllImport(GL_DLL, EntryPoint="glPixelStoref"), SuppressUnmanagedCodeSecurity]
		public static extern void PixelStoref(uint pname, float param);

		[DllImport(GL_DLL, EntryPoint="glPixelStorei"), SuppressUnmanagedCodeSecurity]
		public static extern void PixelStorei(uint pname, int param);

		[DllImport(GL_DLL, EntryPoint="glPixelTransferf"), SuppressUnmanagedCodeSecurity]
		public static extern void PixelTransferf(uint pname, float param);

		[DllImport(GL_DLL, EntryPoint="glPixelTransferi"), SuppressUnmanagedCodeSecurity]
		public static extern void PixelTransferi(uint pname, int param);

		[DllImport(GL_DLL, EntryPoint="glPixelZoom"), SuppressUnmanagedCodeSecurity]
		public static extern void PixelZoom(float xfactor, float yfactor);

		[DllImport(GL_DLL, EntryPoint="glPointSize"), SuppressUnmanagedCodeSecurity]
		public static extern void PointSize(float size);

		[DllImport(GL_DLL, EntryPoint="glPolygonMode"), SuppressUnmanagedCodeSecurity]
		public static extern void PolygonMode(uint face, uint mode);

		[DllImport(GL_DLL, EntryPoint="glPolygonOffset"), SuppressUnmanagedCodeSecurity]
		public static extern void PolygonOffset(float factor, float units);

		[DllImport(GL_DLL, EntryPoint="glPolygonStipple"), SuppressUnmanagedCodeSecurity]
		public static extern void PolygonStipple([In] byte[] mask);

		[DllImport(GL_DLL, EntryPoint="glPopAttrib"), SuppressUnmanagedCodeSecurity]
		public static extern void PopAttrib();

		[DllImport(GL_DLL, EntryPoint="glPopClientAttrib"), SuppressUnmanagedCodeSecurity]
		public static extern void PopClientAttrib();

		[DllImport(GL_DLL, EntryPoint="glPopMatrix"), SuppressUnmanagedCodeSecurity]
		public static extern void PopMatrix();

		[DllImport(GL_DLL, EntryPoint="glPopName"), SuppressUnmanagedCodeSecurity]
		public static extern void PopName();

		[DllImport(GL_DLL, EntryPoint="glPrioritizeTextures"), SuppressUnmanagedCodeSecurity]
		public static extern void PrioritizeTextures(int n, [In] uint[] textures, [In] float[] priorities);

		[DllImport(GL_DLL, EntryPoint="glPushAttrib"), SuppressUnmanagedCodeSecurity]
		public static extern void PushAttrib(uint mask);

		[DllImport(GL_DLL, EntryPoint="glPushClientAttrib"), SuppressUnmanagedCodeSecurity]
		public static extern void PushClientAttrib(uint mask);

		[DllImport(GL_DLL, EntryPoint="glPushMatrix"), SuppressUnmanagedCodeSecurity]
		public static extern void PushMatrix();

		[DllImport(GL_DLL, EntryPoint="glPushName"), SuppressUnmanagedCodeSecurity]
		public static extern void PushName(uint name);

		[DllImport(GL_DLL, EntryPoint="glRasterPos2d"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos2d(double x, double y);

		[DllImport(GL_DLL, EntryPoint="glRasterPos2dv"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos2dv([In] double[] v);

		[DllImport(GL_DLL, EntryPoint="glRasterPos2f"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos2f(float x, float y);

		[DllImport(GL_DLL, EntryPoint="glRasterPos2fv"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos2fv([In] float[] v);

		[DllImport(GL_DLL, EntryPoint="glRasterPos2i"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos2i(int x, int y);

		[DllImport(GL_DLL, EntryPoint="glRasterPos2iv"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos2iv([In] int[] v);

		[DllImport(GL_DLL, EntryPoint="glRasterPos2s"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos2s(short x, short y);

		[DllImport(GL_DLL, EntryPoint="glRasterPos2sv"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos2sv([In] short[] v);

		[DllImport(GL_DLL, EntryPoint="glRasterPos3d"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos3d(double x, double y, double z);

		[DllImport(GL_DLL, EntryPoint="glRasterPos3dv"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos3dv([In] double[] v);

		[DllImport(GL_DLL, EntryPoint="glRasterPos3f"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos3f(float x, float y, float z);

		[DllImport(GL_DLL, EntryPoint="glRasterPos3fv"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos3fv([In] float[] v);

		[DllImport(GL_DLL, EntryPoint="glRasterPos3i"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos3i(int x, int y, int z);

		[DllImport(GL_DLL, EntryPoint="glRasterPos3iv"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos3iv([In] int[] v);

		[DllImport(GL_DLL, EntryPoint="glRasterPos3s"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos3s(short x, short y, short z);

		[DllImport(GL_DLL, EntryPoint="glRasterPos3sv"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos3sv([In] short[] v);

		[DllImport(GL_DLL, EntryPoint="glRasterPos4d"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos4d(double x, double y, double z, double w);

		[DllImport(GL_DLL, EntryPoint="glRasterPos4dv"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos4dv([In] double[] v);

		[DllImport(GL_DLL, EntryPoint="glRasterPos4f"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos4f(float x, float y, float z, float w);

		[DllImport(GL_DLL, EntryPoint="glRasterPos4fv"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos4fv([In] float[] v);

		[DllImport(GL_DLL, EntryPoint="glRasterPos4i"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos4i(int x, int y, int z, int w);

		[DllImport(GL_DLL, EntryPoint="glRasterPos4iv"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos4iv([In] int[] v);

		[DllImport(GL_DLL, EntryPoint="glRasterPos4s"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos4s(short x, short y, short z, short w);

		[DllImport(GL_DLL, EntryPoint="glRasterPos4sv"), SuppressUnmanagedCodeSecurity]
		public static extern void RasterPos4sv([In] short[] v);

		[DllImport(GL_DLL, EntryPoint="glReadBuffer"), SuppressUnmanagedCodeSecurity]
		public static extern void ReadBuffer(uint mode);

		[DllImport(GL_DLL, EntryPoint="glReadPixels"), SuppressUnmanagedCodeSecurity]
		public static extern void ReadPixels(int x, int y, int width, int height, uint format, uint type, IntPtr pixels);

		[DllImport(GL_DLL, EntryPoint="glRectd"), SuppressUnmanagedCodeSecurity]
		public static extern void Rectd(double x1, double y1, double x2, double y2);

		[DllImport(GL_DLL, EntryPoint="glRectdv"), SuppressUnmanagedCodeSecurity]
		public static extern void Rectdv([In] double[] v1, [In] double[] v2);

		[DllImport(GL_DLL, EntryPoint="glRectf"), SuppressUnmanagedCodeSecurity]
		public static extern void Rectf(float x1, float y1, float x2, float y2);

		[DllImport(GL_DLL, EntryPoint="glRectfv"), SuppressUnmanagedCodeSecurity]
		public static extern void Rectfv([In] float[] v1, [In] float[] v2);

		[DllImport(GL_DLL, EntryPoint="glRecti"), SuppressUnmanagedCodeSecurity]
		public static extern void Recti(int x1, int y1, int x2, int y2);

		[DllImport(GL_DLL, EntryPoint="glRectiv"), SuppressUnmanagedCodeSecurity]
		public static extern void Rectiv([In] int[] v1, [In] int[] v2);

		[DllImport(GL_DLL, EntryPoint="glRects"), SuppressUnmanagedCodeSecurity]
		public static extern void Rects(short x1, short y1, short x2, short y2);

		[DllImport(GL_DLL, EntryPoint="glRectsv"), SuppressUnmanagedCodeSecurity]
		public static extern void Rectsv([In] short[] v1, [In] short[] v2);

		[DllImport(GL_DLL, EntryPoint="glRenderMode"), SuppressUnmanagedCodeSecurity]
		public static extern int RenderMode(uint mode);

		[DllImport(GL_DLL, EntryPoint="glRotated"), SuppressUnmanagedCodeSecurity]
		public static extern void Rotated(double angle, double x, double y, double z);

		[DllImport(GL_DLL, EntryPoint="glRotatef"), SuppressUnmanagedCodeSecurity]
		public static extern void Rotatef(float angle, float x, float y, float z);

		[DllImport(GL_DLL, EntryPoint="glScaled"), SuppressUnmanagedCodeSecurity]
		public static extern void Scaled(double x, double y, double z);

		[DllImport(GL_DLL, EntryPoint="glScalef"), SuppressUnmanagedCodeSecurity]
		public static extern void Scalef(float x, float y, float z);

		[DllImport(GL_DLL, EntryPoint="glScissor"), SuppressUnmanagedCodeSecurity]
		public static extern void Scissor(int x, int y, int width, int height);

		[DllImport(GL_DLL, EntryPoint="glSelectBuffer"), SuppressUnmanagedCodeSecurity]
		public static extern void SelectBuffer(int size, [In, Out] uint[] buffer);

		[DllImport(GL_DLL, EntryPoint="glShadeModel"), SuppressUnmanagedCodeSecurity]
		public static extern void ShadeModel(uint mode);

		[DllImport(GL_DLL, EntryPoint="glStencilFunc"), SuppressUnmanagedCodeSecurity]
		public static extern void StencilFunc(uint func, int _ref, uint mask);

		[DllImport(GL_DLL, EntryPoint="glStencilMask"), SuppressUnmanagedCodeSecurity]
		public static extern void StencilMask(uint mask);

		[DllImport(GL_DLL, EntryPoint="glStencilOp"), SuppressUnmanagedCodeSecurity]
		public static extern void StencilOp(uint fail, uint zfail, uint zpass);

		[DllImport(GL_DLL, EntryPoint="glTexCoord1d"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord1d(double s);

		[DllImport(GL_DLL, EntryPoint="glTexCoord1dv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord1dv([In] double[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord1f"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord1f(float s);

		[DllImport(GL_DLL, EntryPoint="glTexCoord1fv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord1fv([In] float[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord1i"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord1i(int s);

		[DllImport(GL_DLL, EntryPoint="glTexCoord1iv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord1iv([In] int[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord1s"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord1s(short s);

		[DllImport(GL_DLL, EntryPoint="glTexCoord1sv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord1sv([In] short[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord2d"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord2d(double s, double t);

		[DllImport(GL_DLL, EntryPoint="glTexCoord2dv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord2dv([In] double[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord2f"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord2f(float s, float t);

		[DllImport(GL_DLL, EntryPoint="glTexCoord2fv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord2fv([In] float[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord2i"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord2i(int s, int t);

		[DllImport(GL_DLL, EntryPoint="glTexCoord2iv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord2iv([In] int[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord2s"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord2s(short s, short t);

		[DllImport(GL_DLL, EntryPoint="glTexCoord2sv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord2sv([In] short[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord3d"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord3d(double s, double t, double r);

		[DllImport(GL_DLL, EntryPoint="glTexCoord3dv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord3dv([In] double[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord3f"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord3f(float s, float t, float r);

		[DllImport(GL_DLL, EntryPoint="glTexCoord3fv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord3fv([In] float[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord3i"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord3i(int s, int t, int r);

		[DllImport(GL_DLL, EntryPoint="glTexCoord3iv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord3iv([In] int[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord3s"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord3s(short s, short t, short r);

		[DllImport(GL_DLL, EntryPoint="glTexCoord3sv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord3sv([In] short[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord4d"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord4d(double s, double t, double r, double q);

		[DllImport(GL_DLL, EntryPoint="glTexCoord4dv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord4dv([In] double[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord4f"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord4f(float s, float t, float r, float q);

		[DllImport(GL_DLL, EntryPoint="glTexCoord4fv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord4fv([In] float[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord4i"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord4i(int s, int t, int r, int q);

		[DllImport(GL_DLL, EntryPoint="glTexCoord4iv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord4iv([In] int[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoord4s"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord4s(short s, short t, short r, short q);

		[DllImport(GL_DLL, EntryPoint="glTexCoord4sv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoord4sv([In] short[] v);

		[DllImport(GL_DLL, EntryPoint="glTexCoordPointer"), SuppressUnmanagedCodeSecurity]
		public static extern void TexCoordPointer(int size, uint type, int stride, IntPtr pointer);

		[DllImport(GL_DLL, EntryPoint="glTexEnvf"), SuppressUnmanagedCodeSecurity]
		public static extern void TexEnvf(uint target, uint pname, float param);

		[DllImport(GL_DLL, EntryPoint="glTexEnvfv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexEnvfv(uint target, uint pname, [In] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glTexEnvi"), SuppressUnmanagedCodeSecurity]
		public static extern void TexEnvi(uint target, uint pname, int param);

		[DllImport(GL_DLL, EntryPoint="glTexEnviv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexEnviv(uint target, uint pname, [In] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glTexGend"), SuppressUnmanagedCodeSecurity]
		public static extern void TexGend(uint coord, uint pname, double param);

		[DllImport(GL_DLL, EntryPoint="glTexGendv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexGendv(uint coord, uint pname, [In] double[] _params);

		[DllImport(GL_DLL, EntryPoint="glTexGenf"), SuppressUnmanagedCodeSecurity]
		public static extern void TexGenf(uint coord, uint pname, float param);

		[DllImport(GL_DLL, EntryPoint="glTexGenfv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexGenfv(uint coord, uint pname, [In] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glTexGeni"), SuppressUnmanagedCodeSecurity]
		public static extern void TexGeni(uint coord, uint pname, int param);

		[DllImport(GL_DLL, EntryPoint="glTexGeniv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexGeniv(uint coord, uint pname, [In] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glTexImage1D"), SuppressUnmanagedCodeSecurity]
		public static extern void TexImage1D(uint target, int level, int internalformat, int width, int border, uint format, uint type, IntPtr pixels);

		[DllImport(GL_DLL, EntryPoint="glTexImage2D"), SuppressUnmanagedCodeSecurity]
		public static extern void TexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels);

		[DllImport(GL_DLL, EntryPoint="glTexParameterf"), SuppressUnmanagedCodeSecurity]
		public static extern void TexParameterf(uint target, uint pname, float param);

		[DllImport(GL_DLL, EntryPoint="glTexParameterfv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexParameterfv(uint target, uint pname, [In] float[] _params);

		[DllImport(GL_DLL, EntryPoint="glTexParameteri"), SuppressUnmanagedCodeSecurity]
		public static extern void TexParameteri(uint target, uint pname, int param);

		[DllImport(GL_DLL, EntryPoint="glTexParameteriv"), SuppressUnmanagedCodeSecurity]
		public static extern void TexParameteriv(uint target, uint pname, [In] int[] _params);

		[DllImport(GL_DLL, EntryPoint="glTexSubImage1D"), SuppressUnmanagedCodeSecurity]
		public static extern void TexSubImage1D(uint target, int level, int xoffset, int width, uint format, uint type, IntPtr pixels);

		[DllImport(GL_DLL, EntryPoint="glTexSubImage2D"), SuppressUnmanagedCodeSecurity]
		public static extern void TexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);

		[DllImport(GL_DLL, EntryPoint="glTranslated"), SuppressUnmanagedCodeSecurity]
		public static extern void Translated(double x, double y, double z);

		[DllImport(GL_DLL, EntryPoint="glTranslatef"), SuppressUnmanagedCodeSecurity]
		public static extern void Translatef(float x, float y, float z);

		[DllImport(GL_DLL, EntryPoint="glVertex2d"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex2d(double x, double y);

		[DllImport(GL_DLL, EntryPoint="glVertex2dv"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex2dv([In] double[] v);

		[DllImport(GL_DLL, EntryPoint="glVertex2f"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex2f(float x, float y);

		[DllImport(GL_DLL, EntryPoint="glVertex2fv"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex2fv([In] float[] v);

		[DllImport(GL_DLL, EntryPoint="glVertex2i"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex2i(int x, int y);

		[DllImport(GL_DLL, EntryPoint="glVertex2iv"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex2iv([In] int[] v);

		[DllImport(GL_DLL, EntryPoint="glVertex2s"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex2s(short x, short y);

		[DllImport(GL_DLL, EntryPoint="glVertex2sv"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex2sv([In] short[] v);

		[DllImport(GL_DLL, EntryPoint="glVertex3d"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex3d(double x, double y, double z);

		[DllImport(GL_DLL, EntryPoint="glVertex3dv"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex3dv([In] double[] v);

		[DllImport(GL_DLL, EntryPoint="glVertex3f"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex3f(float x, float y, float z);

		[DllImport(GL_DLL, EntryPoint="glVertex3fv"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex3fv([In] float[] v);

		[DllImport(GL_DLL, EntryPoint="glVertex3i"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex3i(int x, int y, int z);

		[DllImport(GL_DLL, EntryPoint="glVertex3iv"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex3iv([In] int[] v);

		[DllImport(GL_DLL, EntryPoint="glVertex3s"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex3s(short x, short y, short z);

		[DllImport(GL_DLL, EntryPoint="glVertex3sv"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex3sv([In] short[] v);

		[DllImport(GL_DLL, EntryPoint="glVertex4d"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex4d(double x, double y, double z, double w);

		[DllImport(GL_DLL, EntryPoint="glVertex4dv"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex4dv([In] double[] v);

		[DllImport(GL_DLL, EntryPoint="glVertex4f"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex4f(float x, float y, float z, float w);

		[DllImport(GL_DLL, EntryPoint="glVertex4fv"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex4fv([In] float[] v);

		[DllImport(GL_DLL, EntryPoint="glVertex4i"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex4i(int x, int y, int z, int w);

		[DllImport(GL_DLL, EntryPoint="glVertex4iv"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex4iv([In] int[] v);

		[DllImport(GL_DLL, EntryPoint="glVertex4s"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex4s(short x, short y, short z, short w);

		[DllImport(GL_DLL, EntryPoint="glVertex4sv"), SuppressUnmanagedCodeSecurity]
		public static extern void Vertex4sv([In] short[] v);

		[DllImport(GL_DLL, EntryPoint="glVertexPointer"), SuppressUnmanagedCodeSecurity]
		public static extern void VertexPointer(int size, uint type, int stride, IntPtr pointer);

		[DllImport(GL_DLL, EntryPoint="glViewport"), SuppressUnmanagedCodeSecurity]
		public static extern void Viewport(int x, int y, int width, int height);


	}
}
