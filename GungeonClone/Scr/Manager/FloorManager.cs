using Godot;
using System;

public class FloorManager : Node2D{
	private GameManager gameManager;
	private PackedScene teleporter = GD.Load<PackedScene>("res://Prefab/Teleporter.tscn");
	public Control uiTeleporter;
	private PackedScene[] enemyPrefab = {
		GD.Load<PackedScene>("res://Prefab/Enemies/Enemy.tscn"),
		GD.Load<PackedScene>("res://Prefab/Enemies/Target.tscn")
	};

	PackedScene[] Floor = {
		GD.Load<PackedScene>("res://Prefab/Floor/VillageFloor.tscn"),
		GD.Load<PackedScene>("res://Prefab/Floor/ArenaFloor.tscn"),
		GD.Load<PackedScene>("res://Prefab/Floor/ShopFloor.tscn"),
		GD.Load<PackedScene>("res://Prefab/Floor/BossFloor.tscn")
	};

	public enum FloorType{
		VILLAGE = 0,
		ARENA = 1,
		SHOP = 2,
		BOSS = 3
	};

	FloorType currentFloorType;

	int arenaLevel;
	Node2D currentFloor;
	Node2D currentTeleporter;

	float time;

	public Node2D SpawnFloor(FloorType floorNum){
		if(currentTeleporter != null) currentTeleporter.QueueFree();
		if(currentFloor != null) currentFloor.QueueFree();
		Node2D floor = (Node2D)Floor[(int)floorNum].Instance();
		GetTree().CurrentScene.AddChild(floor);

		if(GetTree().CurrentScene.GetNode<Area2D>("Floor/Teleporter") != null){
			GetTree().CurrentScene.GetNode<Area2D>("Floor/Teleporter").Connect("body_entered", this, "OnTeleporterEnter");
		}
		if(floorNum == FloorType.ARENA) EnterArena();
		return floor;
	}

	public override void _Ready(){
		uiTeleporter = GetTree().CurrentScene.GetNode<Control>("Canvas/UiTeleporter");
	}

	public override void _Process(float delta){
		time += delta;
		if(time >= 1 && currentFloor == null){
			currentFloor = SpawnFloor(FloorType.VILLAGE);
		}
	}

	public void SpawnTeleporter(){
		var tel = (Area2D)teleporter.Instance();
		GetTree().CurrentScene.GetNode("YSort").AddChild(tel);
		tel.Connect("body_entered", this, "OnTeleporterEnter");
		currentTeleporter = tel;
	}

	private void ButtonPressed(int extra_arg_0){
		currentFloor = SpawnFloor((FloorType)extra_arg_0);
		GetTree().Paused = false;
		uiTeleporter.Visible = false;
	}

	private void OnTeleporterEnter(object body){
	 	GetTree().Paused = true;
		uiTeleporter.Visible = true;
	}

	private void EnterArena(){
		var amount = 10;
		enemyLeft = amount;
		for(int i = 0; i < amount; i++){
			var enemy = (RigidBody2D)enemyPrefab[(int)GD.RandRange(0,enemyPrefab.Length)].Instance();
			enemy.Connect("tree_exiting", this, "EnemyDie");
			GetTree().CurrentScene.GetNode("YSort").AddChild(enemy);
		}
	}

	private int enemyLeft;

	private void EnemyDie(){
		enemyLeft--;
		if(enemyLeft <=0) SpawnTeleporter();
	}
}



