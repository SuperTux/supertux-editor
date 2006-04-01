using System;
using System.Collections.Generic;
using System.Collections;

namespace SceneGraph
{

public class Layer : Node {
    private SortedList Layers = new SortedList();

    public void Add(float Layer, Node Child) {
        if(Layers[Layer] == null)
            Layers[Layer] = new List<Node>();
        List<Node> Childs = (List<Node>) Layers[Layer];
        Childs.Add(Child);
    }

    public void Remove(float Layer, Node Child) {
        if(Layers[Layer] == null)
            throw new Exception("Specified Layer is empty");
        List<Node> Childs = (List<Node>) Layers[Layer];
        Childs.Remove(Child);
    }

	public void Clear() {
		Layers.Clear();
	}

    public void Draw() {
        foreach(DictionaryEntry Entry in Layers) {
            List<Node> List = (List<Node>) Entry.Value;
            foreach(Node Child in List) {
                Child.Draw();
            }
        }
    }
}

}

