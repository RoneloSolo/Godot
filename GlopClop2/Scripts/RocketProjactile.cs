using Godot;
using System;

public class RocketProjactile : Projactile{
	private PackedScene blast = (PackedScene)ResourceLoader.Load("res://Prefab/Blast.tscn");
	private Vector2 target;
	private Vector2 center;
	private Vector2 startPos;
	private float time;

	public override void Start(Vector2 pos, float dir, Vector2 target){
		speed = 2;
    	base.Start(pos, dir);
		this.target = target;
		target += RandVector2(-15,15);
		startPos = GlobalPosition;
		center = (target + startPos) / 2 + RandVector2(-125,125);
    }

  	public override void _PhysicsProcess(float delta){
		time += delta;
		var disPos = Arc(startPos, center, target, time / speed);
		var velocity = disPos - GlobalPosition;
		LookAt(disPos);
		var collision = MoveAndCollide(velocity);
		if (collision != null){
			if (!collision.Collider.HasMethod("Hit")) return;
			collision.Collider.Call("Hit");
			Explode();
		}
		else if(GlobalPosition.DistanceTo(target) < 1) {
			var _blast = (Node2D)blast.Instance();
			Explode();
		}
  	}

	private void Explode(){
		var _blast = (Node2D)blast.Instance();
		_blast.Position = Position;
		GetTree().CurrentScene.AddChild(_blast);
		QueueFree();
	}

	private Vector2 RandVector2(float min, float max) {return new Vector2((float)GD.RandRange(min,max),(float)GD.RandRange(min,max));}
	
	private static Vector2 Arc(Vector2 from, Vector2 b, Vector2 to, float weight){
		var p0 = from.LinearInterpolate(b, weight);
		var p1 = b.LinearInterpolate(to, weight);
		return p0.LinearInterpolate(p1, weight);
	}
}
