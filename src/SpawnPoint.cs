using Lisp;
using DataStructures;
using System;

public class SpawnPoint {
    public string Name;
    public Vector Pos;

    public void Parse(List Data) {
        Properties Props = new Properties(Data);

        if(!Props.Get("name", ref Name))
            throw new Exception("SpawnPoint has no Name");
        Props.Get("x", ref Pos.X);
        Props.Get("y", ref Pos.Y);
        Props.PrintUnusedWarnings();
    }
}
