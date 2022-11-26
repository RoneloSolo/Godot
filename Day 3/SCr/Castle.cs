using Godot;
using System;

public class Castle : Building{
	private int gold = 100;
	private Building mineshaft = new Mineshaft(2,5, GD.Load<PackedScene>("res://Mineshaft.tscn"));
	private Building tower =  new Tower(4, 10, GD.Load<PackedScene>("res://Tower.tscn"));
	private Building currentBuilding;
	Node2D building;
	PackedScene enemyPrefab = GD.Load<PackedScene>("res://Enemy.tscn");
		
	public override void _Process(float delta){
		var mousePos = GetGlobalMousePosition();
		if(Input.IsActionJustPressed("slot1")) SetCurrentBuiding(mineshaft);
		else if(Input.IsActionJustPressed("slot2")) SetCurrentBuiding(tower);
		else if(Input.IsActionJustPressed("exit")) {
			currentBuilding = null;
			building.QueueFree();
			building = null;
		}
		if(currentBuilding == null) return;
		building.Position = new Vector2(Mathf.Stepify(mousePos.x,16), Mathf.Stepify(mousePos.y,16));
		if(Input.IsActionJustPressed("fire1")){
			if(currentBuilding.Build(gold) == -1){
				building.QueueFree();
				building = null;
				currentBuilding = null;
				return;
			}
			gold -= currentBuilding.cost;
			building = null;
			SetCurrentBuiding(currentBuilding);
		}
	}
	
	private void SetCurrentBuiding(Building build){
		if(building != null) building.QueueFree();
		currentBuilding = build;
		building = (Node2D)currentBuilding.prefab.Instance();
		GetTree().CurrentScene.AddChild(building);
	}
	
	private void OnWaveBegin(){
		int amount = (int)GD.RandRange(0,5);
		for(int i = amount;i > 0; i--){
			var enemy = (Node2D)enemyPrefab.Instance();
			GetTree().CurrentScene.AddChild(enemy);
			float step = 2 * Mathf.Pi / 360;
		 	enemy.Position = new Vector2(250,0).Rotated(step * (int)GD.RandRange(0,360));
		}
	}
}
 
