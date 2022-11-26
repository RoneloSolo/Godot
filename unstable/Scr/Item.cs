using Godot;
using System;

public class Item : KinematicBody{
    [Export] public string itemName;
    public string Interact() =>  itemName;
}
