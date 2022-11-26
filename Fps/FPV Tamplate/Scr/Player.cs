using Godot;
using System;

public class Player : KinematicBody{
	private const int SPEED = 5;	//Speed the plyer move;
	private const float MOUSESENSITIVITY = 0.05f;
	private const int GRAVITY = 9;
	private Vector3 velocity = new Vector3();
	private Vector2 mouseMov;
	private float time;

	private Position3D head;
	private Sprite crosshair;
	private RayCast interactRay;
	
	public override void _Ready(){
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		head = GetNode<Position3D>("Head");
		crosshair = GetNode<Sprite>("Head/Crosshair");
		interactRay = GetNode<RayCast>("Head/InteractRay");;
		crosshair.GlobalPosition = GetViewport().GetVisibleRect().Size/2;
	}

	//public override void _Process(float delta){
	//}

	public override void _PhysicsProcess(float delta){
		PlayerMovement(delta);
	}

	public override void _Input(InputEvent @event){
		if (Input.IsActionJustPressed("ui_cancel")) {
			if (Input.MouseMode == Input.MouseModeEnum.Visible) 
				Input.MouseMode = Input.MouseModeEnum.Captured;
			else Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		
		if(@event is InputEventMouseMotion == false) return;
		if(Input.MouseMode != Input.MouseModeEnum.Captured) return;
		InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
		mouseMov = new Vector2(-mouseEvent.Relative.x, mouseEvent.Relative.y);
		HeadMovement();
	}
	
	private void HeadMovement(){
		head.RotateX(-Mathf.Deg2Rad(mouseMov.y * MOUSESENSITIVITY));
		RotateY(Mathf.Deg2Rad(mouseMov.x * MOUSESENSITIVITY));
		Vector3 cameraRot = head.RotationDegrees;
		cameraRot.x = Mathf.Clamp(cameraRot.x, -90, 90);
		head.RotationDegrees = cameraRot;
	}
	
	private void PlayerMovement(float delta){
		var horizontal = (Input.GetActionStrength("right") - Input.GetActionStrength("left"));
		var vertical = (Input.GetActionStrength("backward") - Input.GetActionStrength("forward"));
		velocity = horizontal * Transform.basis.x;
		velocity += vertical * Transform.basis.z;
		velocity = velocity.Normalized() * SPEED;
		if(!IsOnFloor()) velocity.y -= GRAVITY * delta;
		MoveAndSlide(velocity);
	}

}
