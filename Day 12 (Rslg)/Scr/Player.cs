using Godot;
using System;

public class Player : KinematicBody{
	private CSGMesh hand;
	private Material scaner = GD.Load<Material>("res://Hands/Scaner.tres");
	private Material idle = GD.Load<Material>("res://Hands/Idle.tres");
	private const int SPEED =25;
	private Vector3 vel;
	private float deltaTime;
	private float horizontal;
	private float forward;
	private Spatial head;
	private Vector2 mouseMov;
	private const float MOUSESENSITIVITY = .05f;
	
	public override void _Ready(){
		hand = GetNode<CSGMesh>("Head/Hand");
		head = GetNode<Spatial>("Head");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	
	public override void _Process(float delta){
		ProcessInput();
	}

	public override void _PhysicsProcess(float delta){
		deltaTime = delta;
		ProcessMovement();
		MoveAndSlide(vel,Vector3.Up);
	}

	private void ProccesCamera(){
		head.RotateX(-Mathf.Deg2Rad(mouseMov.y * MOUSESENSITIVITY));
		RotateY(Mathf.Deg2Rad(mouseMov.x * MOUSESENSITIVITY));
		Vector3 cameraRot = head.RotationDegrees;
		cameraRot.x = Mathf.Clamp(cameraRot.x, -90, 90);
		head.RotationDegrees = cameraRot;
	}

	public override void _Input(InputEvent @event){
		if(@event is InputEventMouseMotion == false) return;
		if(Input.MouseMode != Input.MouseModeEnum.Captured) return;
		InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
		mouseMov = new Vector2(-mouseEvent.Relative.x, mouseEvent.Relative.y);

		ProccesCamera();
	}
	
	private void ProcessInput(){
		horizontal = (Input.GetActionStrength("right") - Input.GetActionStrength("left"));
		forward = (Input.GetActionStrength("backward") - Input.GetActionStrength("forward"));
		if (Input.IsActionJustPressed("ui_cancel")) {
			if (Input.MouseMode == Input.MouseModeEnum.Visible) 
				Input.MouseMode = Input.MouseModeEnum.Captured;
			else Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		if(Input.IsActionPressed("interact")) hand.Material = scaner;
	}

	private void ProcessMovement(){
		vel = horizontal * Transform.basis.x;
		vel += forward * Transform.basis.z;
		vel = vel.Normalized() * SPEED;
	}
}
