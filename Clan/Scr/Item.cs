using Godot;
using System;

/// <summary>
/// An Item data can be stone, wood, sword etc.
/// </summary>
[Tool]
public class Item: Resource{
	[Export] public string name;
	[Export] public Sprite Icon;
}
