using Godot;
using System;

public class Player : KinematicBody{
#region Varibales

	private const int DEACCELERATION = 5; // This Is DeAcceleration Of Player Movement
	private const int ACCELERATION = 3; // This Is Acceleration Of Player Movement
	private const int SPEED = 10; // This Is Player Walk Speed
	private const float MOUSESENSITIVITY = 0.05f; // This Is Mouse Sensetivity Of Playyer
	private const float GRAVITY = 19.62f; // This Is Gravity Of Player
	private float horizontal; // This Is Input of player A,D
	private float deltaTime;// This Is Delta time
	private float forward; // This Is Input of player W,S
	private float time; // This is the time in seconds since the start of the application

	private Vector2 mouseMov; // This Needed For Weapon Sway Mechanic
	private Vector3 disVel; // Desire Velocity Of Player
	private Vector3 vel; // Velocity Of Player
	private Spatial holdPos;

	private Spatial head;
	private Sprite crosshair;
	private RayCast raycast; // This Is Needed To Detect Item Hit

	private string itemHolding;
	private Spatial itemHold;

	#endregion

	public override void _Ready(){
		crosshair = GetNode<Sprite>("Head/Cam/Crosshair");
		raycast = GetNode<RayCast>("Head/Cam/RayCast");
		crosshair.GlobalPosition = GetViewport().GetVisibleRect().Size/2;
		head = GetNode<Spatial>("Head");
		holdPos = GetNode<Spatial>("Head/Hold");
		Input.SetMouseMode(Input.MouseMode.Captured);
	}

	public override void _Process(float delta){
		ProccesMain(delta);
	}

	public override void _PhysicsProcess(float delta){
		ProcessInput();
		ProcessMovement();
		MoveAndSlide(vel,Vector3.Up);
	}

	public override void _Input(InputEvent @event){
		if(@event is InputEventMouseMotion == false) return;
		if(Input.GetMouseMode() != Input.MouseMode.Captured) return;
		InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
		mouseMov = new Vector2(-mouseEvent.Relative.x, mouseEvent.Relative.y);
		ProccesCamera();
	}
	
	private void ProccesMain(float delta){
		crosshair.GlobalPosition = GetViewport().GetVisibleRect().Size/2;
		deltaTime = delta;
		time += deltaTime;
		if(Input.IsActionJustPressed("e")) Interact();
		if(itemHolding != string.Empty && itemHolding != null) itemHold.GlobalTransform = holdPos.GlobalTransform;
	}

	private void ProcessInput(){
		horizontal = (Input.GetActionStrength("right") - Input.GetActionStrength("left"));
		forward = (Input.GetActionStrength("backward") - Input.GetActionStrength("forward"));
		if (Input.IsActionJustPressed("ui_cancel")) {
			if (Input.GetMouseMode() == Input.MouseMode.Visible) 
				Input.SetMouseMode(Input.MouseMode.Captured);
			else Input.SetMouseMode(Input.MouseMode.Visible);
		}
	}

	private void ProcessMovement(){
		disVel = horizontal * Transform.basis.x;
		disVel += forward * Transform.basis.z;
		disVel = disVel.Normalized();
		vel = (Mathf.Abs(disVel.Length()) > .1f) ? 
			vel.LinearInterpolate(disVel * SPEED, ACCELERATION * deltaTime) :  
			vel.LinearInterpolate(disVel * SPEED, DEACCELERATION * deltaTime);

		if(!IsOnFloor()) vel.y -= GRAVITY * deltaTime;
	}

	private void ProccesCamera(){
		head.RotateX(-Mathf.Deg2Rad(mouseMov.y * MOUSESENSITIVITY));
		RotateY(Mathf.Deg2Rad(mouseMov.x * MOUSESENSITIVITY));
		Vector3 cameraRot = head.RotationDegrees;
		cameraRot.x = Mathf.Clamp(cameraRot.x, -90, 90);
		head.RotationDegrees = cameraRot;
	}

	private void Interact(){
		if(itemHolding != null && itemHold != null){
			itemHolding = string.Empty;
			if(itemHold.GetNode<CollisionShape>("Coll") != null) itemHold.GetNode<CollisionShape>("Coll").Disabled = false;
			itemHold = null;
		}
		if(!raycast.IsColliding()) return;
		var target = (Spatial)raycast.GetCollider();
		if(target.HasMethod("Interact")) {
			itemHold = target;
			itemHold.GetNode<CollisionShape>("Coll").Disabled = true;
			itemHolding = target.Call("Interact").ToString();
			target.GlobalTransform = holdPos.GlobalTransform;
		}
	}

}
