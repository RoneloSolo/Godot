using Godot;
using System;

public class BuildManager : Node2D{

	int tileSize = 16;
	Vector2 mousePos;

	[Export] public BuildPlan[] buildPlans;
	Node buildingLayer;

	Node2D building;

	public override void _Ready(){
		buildingLayer = GetNode<Node>("../../Buildings");
	}

	public override void _Process(float delta){
		mousePos = GetGlobalMousePosition();
		if(building != null)
			building.Position = new Vector2(Mathf.Stepify(mousePos.x,tileSize), Mathf.Stepify(mousePos.y,tileSize));
	}

	private void House0Button(){
		building = (Node2D)buildPlans[0].GetPrefab().Instance();
		buildingLayer.AddChild(building);
	}
}


