using System.Collections.Generic;
using System.IO;
using System;
using Lisp;
using Resources;
using DataStructures;
using Drawing;

namespace Sprites {

    public class SpriteManager  {
        private static Dictionary<string, SpriteData> SpriteDatas 
            = new Dictionary<string, SpriteData>();
        
        public static Sprite Create(string SpriteFile) {
            if(!SpriteDatas.ContainsKey(SpriteFile)) {
                SpriteData Data = LoadSprite(SpriteFile);
                SpriteDatas[SpriteFile] = Data;
                return new Sprite(Data);
            }
            
            return new Sprite(SpriteDatas[SpriteFile]);
        }
        
        public static Sprite CreateFromImage(string ImageFile, Vector offset) {
        	if(!SpriteDatas.ContainsKey(ImageFile)) {
        		Surface Surface = new Surface(ImageFile);
        		SpriteData Data = new SpriteData(Surface, offset);
        		SpriteDatas[ImageFile] = Data;
        		return new Sprite(Data);
        	}
        	
        	return new Sprite(SpriteDatas[ImageFile]);
        } 
        
        public static Sprite CreateFromImage(string ImageFile) {
        	return CreateFromImage(ImageFile, new Vector(0, 0));
        }
        
        private static SpriteData LoadSprite(string Filename) {
            string BaseDir = ResourceManager.Instance.GetDirectoryName(Filename);
			List SpriteData = Util.Load(Filename, "supertux-sprite");
            
            return new SpriteData(SpriteData, BaseDir);
        }
    }
    
}
