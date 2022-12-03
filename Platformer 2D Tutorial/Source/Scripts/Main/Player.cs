using Godot;
using System;

public class Player : RigidBody2D{
    private static readonly float WALK_ACCELERATION = 800.0f;
    private static readonly float WALK_DEACCELERATION = 800.0f;
    private static readonly float WALK_MAX_VELOCITY = 200.0f;
    private static readonly float AIR_ACCELERATION = 200.0f;
    private static readonly float AIR_DEACCELERATION = 200.0f;
    private static readonly float JUMP_VELOCITY = 460.0f;
    private static readonly float STOP_JUMP_FORCE = 900.0f;
    private static readonly float MAX_SHOOT_POSE_TIME = 0.3f;
    private static readonly float MAX_FLOOR_AIRBORNE_TIME = 0.15f;

    private int speed = 100;
    
    private void ProcessMovement(Physics2DDirectBodyState bodyState){
		var inputVelocityHorizontal = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		inputVelocityHorizontal *= speed;
		inputVelocityHorizontal -= LinearVelocity.x;
		inputVelocityHorizontal = (inputVelocityHorizontal == 0) ? inputVelocityHorizontal * 1 : inputVelocityHorizontal * 10;
		AppliedForce = new Vector2(inputVelocityHorizontal, 0);
        bodyState.LinearVelocity += bodyState.Step * bodyState.TotalGravity;
    }

    public override void _IntegrateForces(Physics2DDirectBodyState bodyState){
        ProcessMovement(bodyState);
    }
}
