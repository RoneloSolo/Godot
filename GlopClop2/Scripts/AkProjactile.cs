using Godot;
using System;

public class AkProjactile : Projactile{

    public override void Start(Vector2 pos, float dir, Vector2 a){
		speed = 258;
    	base.Start(pos, dir);
		velocity = new Vector2(speed, 0).Rotated(Rotation);
    }
}
