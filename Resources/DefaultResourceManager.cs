// $Id: DefaultResourceManager.cs 4703 2007-01-28 13:44:03Z anmaster $
using System;
using System.IO;

namespace Resources
{

	/// <summary>The default implementation of the ResourceManager</summary>
	public class DefaultResourceManager : ResourceManager
	{
		private string DataDir;

		public DefaultResourceManager(string Path)
		{
			this.DataDir = Path;
		}

		// Try to avoid this function
		[System.Obsolete("Do not use GetFileName: resource could be inside an archive file")]
		public override string GetFileName(string ResourcePath)
		{
			return DataDir + ResourcePath;
		}

		public override TextReader Get(string ResourcePath)
		{
			try {
				return new StreamReader(DataDir + ResourcePath);
			} catch(Exception e) {
				throw new Exception("Couldn't load resource '" + ResourcePath + "'", e);
			}
		}

		public override string GetDirectoryName(string ResourcePath)
		{
			return Path.GetDirectoryName(ResourcePath);
		}
	}

}
