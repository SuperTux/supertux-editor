using System.IO;

namespace Resources
{

    /**
     * The ResourceManager is responsible for querying and loading of
     * application resources (resources can be plain files on disk, but could
     * also be stored in compressed archives or dynamically loaded from the net,
     * that's why we use a ResourceManager here)
     */
    public abstract class ResourceManager
    {
        public static ResourceManager Instance = new DefaultResourceManager("data/");

        // Try to avoid this function
        public abstract string GetFilename(string ResourcePath);
        public abstract TextReader Get(string ResourcePath);
        public abstract string GetDirectoryName(string ResourcePath);
    }

}
