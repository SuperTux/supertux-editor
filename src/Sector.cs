using System.Collections.Generic;
using SceneGraph;
using System;

public class SectorBase {
    public string Name;
    public string Music;
    private List<GameObject> Objects = new List<GameObject>();
    private Layer Layer = new Layer();

    public SectorBase() {
    }

    public virtual void AddObject(GameObject Object) {
        Object.SetupGraphics(Layer);
        Objects.Add(Object);
    }
    
    protected virtual void RemoveObject(GameObject Object) {
        Object.RemoveGraphics(Layer);
        Objects.Remove(Object);
    }

    public virtual void RemovePendingObjects() {
        foreach(GameObject Object in Objects) {
            if(Object.RemoveFlag) {
                RemoveObject(Object);
            }
        }
    }

    public virtual void Draw() {
        Layer.Draw();
    }
    
    public virtual void Update(float ElapsedTime) {
        foreach(GameObject Object in Objects) {
            Object.Update(ElapsedTime);
        }
    }
}

