using SceneGraph;

public abstract class GameObject {
    private bool removeFlag;
    public bool RemoveFlag {
        get {
            return removeFlag;
        }
    }

    /**
     * This function is called once before the object is inserted into a sector
     * and gives the object the possibility to register own graphics
     */
    public virtual void SetupGraphics(Layer Layer) {
    }

    public virtual void RemoveGraphics(Layer Layer) {
    }

    /**
     * This function is called once per frame
     */
    public virtual void Update(float ElapsedTime) {
    }

    public void RemoveMe() {
        removeFlag = true;
    }
}
