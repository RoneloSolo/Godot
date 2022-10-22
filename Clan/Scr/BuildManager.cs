using Godot;
using System;

public class BuildManager : Node2D{

	int tileSize = 8;
	Vector2 mousePos;

	[Export] public BuildPlan[] buildPlans;
	Node buildingLayer;

	Node2D building;

	public override void _Ready(){
		buildingLayer = GetNode<Node>("../../Buildings");
	}

	public override void _Process(float delta){
		mousePos = GetGlobalMousePosition();
		if(building != null){
			building.Position = new Vector2(Mathf.Stepify(mousePos.x,tileSize), Mathf.Stepify(mousePos.y,tileSize));
			if(Input.IsActionPressed("Build")){
				buildingLayer.AddChild(building);
				building = null;
			}
		}
	}

	private void House0Button(int i){
		building = (Node2D)buildPlans[i].GetPrefab().Instance();
		buildingLayer.AddChild(building);
	}
}
