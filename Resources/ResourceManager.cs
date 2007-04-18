// $Id: ResourceManager.cs 4703 2007-01-28 13:44:03Z anmaster $
using System.IO;

namespace Resources
{

	/// <summary>
	/// The ResourceManager is responsible for querying and loading of
	/// application resources
	/// </summary>
	/// <remarks>
	/// Resources can be plain files on disk, but could
	/// also be stored in compressed archives or dynamically loaded from the net,
	/// that's why we use a ResourceManager here.
	/// </remarks>
	public abstract class ResourceManager
	{
		public static ResourceManager Instance = new DefaultResourceManager("data/");

		// Try to avoid this function
		[System.Obsolete("Do not use GetFileName: resource could be inside an archive file")]
		public abstract string GetFileName(string ResourcePath);
		public abstract TextReader Get(string ResourcePath);
		public abstract string GetDirectoryName(string ResourcePath);
	}

}
