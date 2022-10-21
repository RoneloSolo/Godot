using Godot;
using System;

public class BuildManager : Node2D{

	Node2D building;
	Vector2 mousePos;

	public override void _Ready(){
		building = GetNode<Node2D>("../../Buildings/Building");
	}

	public override void _Input(InputEvent @event){
		// if (@event is InputEventMouseMotion eventMouseMotion) mousePos = eventMouseMotion.GlobalPosition;
		mousePos = GetGlobalMousePosition();
		building.Position = new Vector2(Mathf.Stepify(mousePos.x,32), Mathf.Stepify(mousePos.y,32));
	}
}
