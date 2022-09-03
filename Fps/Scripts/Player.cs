// using Godot;
// using System;

// public class Player : KinematicBody{
// 	private const int SPEED = 10;
// 	private const int ACCELERATION = 3;
// 	private const int DEACCELERATION = 5;
// 	private const float GRAVITY = 9.81f;
// 	private Vector3 vel = Vector3.Zero;
// 	private Vector3 disVel = Vector3.Zero;

// 	[Export] public float mouseSensitivity = 0.05f;
// 	private Vector2 mouseMov;
// 	private Vector3 swayLeft = new Vector3(0, 5, 0);
// 	private Vector3 swayRight = new Vector3(0, -5, 0);
// 	private Vector3 swayUp = new Vector3(10, 0, 0);
// 	private Vector3 swayDown = new Vector3(-10, 0, 0);

// 	private float deltaTime;
// 	private float time;
// 	private float forward,horizontal;
// 	float lastTimeFire,fireRate = .25f;

//   	private Camera cam;
// 	private Spatial head;
// 	private Spatial hand;
// 	private Spatial handHor;
// 	private Spatial handVer;
// 	private Sprite crosshair;
// 	private RayCast raycast;

// 	private PackedScene hitParticale = (PackedScene)ResourceLoader.Load("res://Prefabs/HitParticale.tscn"); 

// 	private Vector2 windowSize;
	
// 	public override void _Ready(){
// 		cam = GetNode<Camera>("Head/Camera");
// 		head = GetNode<Spatial>("Head");
// 		hand = GetNode<Spatial>("Head/Hand");
// 		handHor = GetNode<Spatial>("Head/Hand/HandHor");
// 		handVer = GetNode<Spatial>("Head/Hand/HandHor/HandVer");
// 		crosshair = GetNode<Sprite>("Head/Cam/Crosshair");
// 		raycast = GetNode<RayCast>("Head/Cam/RayCast");
// 		Input.SetMouseMode(Input.MouseMode.Captured);
// 		crosshair.GlobalPosition = GetViewport().GetVisibleRect().Size/2;
// 	}

// 	public override void _PhysicsProcess(float delta){
// 		ProcessInput();
// 		ProcessMove();
// 		ProccesWallRun();
// 		MoveAndSlide(vel,Vector3.Up);

// 	}

// 	public override void _Process(float delta){
// 		crosshair.GlobalPosition = GetViewport().GetVisibleRect().Size/2;
// 		deltaTime = delta;
// 		time += deltaTime;
// 		ProcessAnimation();
// 		ProcessFire();
// 	}

// 	private void ProcessInput(){
// 		horizontal = (Input.GetActionStrength("right") - Input.GetActionStrength("left"));
// 		forward = (Input.GetActionStrength("backward") - Input.GetActionStrength("forward"));
// 		if (Input.IsActionJustPressed("ui_cancel")) {
// 			if (Input.GetMouseMode() == Input.MouseMode.Visible) Input.SetMouseMode(Input.MouseMode.Captured);
// 			else Input.SetMouseMode(Input.MouseMode.Visible);
// 		}
// 	}

// 	private void ProcessMove(){
// 		disVel = horizontal * Transform.basis.x;
// 		disVel += forward * Transform.basis.z;
// 		disVel = disVel.Normalized();
// 		if(Mathf.Abs(disVel.Length()) > .1f) vel = vel.LinearInterpolate(disVel * SPEED, ACCELERATION * deltaTime);
// 		else vel = vel.LinearInterpolate(disVel * SPEED, DEACCELERATION * deltaTime);
// 		if(!IsOnFloor()) vel += Vector3.Down * deltaTime * GRAVITY;
// 		if(Input.IsActionJustPressed("jump") && IsOnFloor()) vel = new Vector3(vel.x, 5, vel.z);
// 	}

// 	private void ProccesWallRun(){
// 		if(!IsOnWall()) return;
// 		if(!Input.IsActionPressed("forward")) return;
// 		vel = new Vector3(vel.x, 0, vel.z);
// 	}
	
// 	public override void _Input(InputEvent @event){
// 		if (@event is InputEventMouseMotion && Input.GetMouseMode() == Input.MouseMode.Captured){
// 			InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
// 			head.RotateX(-Mathf.Deg2Rad(mouseEvent.Relative.y * mouseSensitivity));
// 			RotateY(Mathf.Deg2Rad(-mouseEvent.Relative.x * mouseSensitivity));
// 			Vector3 cameraRot = head.RotationDegrees;
// 			cameraRot.x = Mathf.Clamp(cameraRot.x, -90, 90);
// 			head.RotationDegrees = cameraRot;
// 			mouseMov.x = -mouseEvent.Relative.x;
// 			mouseMov.y = mouseEvent.Relative.y;
// 		}
// 	}

// 	private void ProcessAnimation(){
// 		if(horizontal > 0) hand.RotationDegrees = hand.RotationDegrees.LinearInterpolate(new Vector3(0,0,-5), deltaTime * 5);
// 		else if(horizontal < 0) hand.RotationDegrees = hand.RotationDegrees.LinearInterpolate(new Vector3(0,0,5), deltaTime * 5);
// 		else hand.RotationDegrees = hand.RotationDegrees.LinearInterpolate(new Vector3(0,0,0), deltaTime * 5);

// 		if(mouseMov.x > 3) handHor.RotationDegrees = handHor.RotationDegrees.LinearInterpolate(swayLeft, 2 * deltaTime);
// 		else if(mouseMov.x < -3) handHor.RotationDegrees = handHor.RotationDegrees.LinearInterpolate(swayRight, 2 * deltaTime);
// 		else handHor.RotationDegrees = handHor.RotationDegrees.LinearInterpolate(Vector3.Zero, 2 * deltaTime);

// 		if(mouseMov.y > 3) handVer.RotationDegrees = handVer.RotationDegrees.LinearInterpolate(swayDown, 3 * deltaTime);
// 		else if(mouseMov.y < -3) handVer.RotationDegrees = handVer.RotationDegrees.LinearInterpolate(swayUp, 3 * deltaTime);
// 		else handVer.RotationDegrees = handVer.RotationDegrees.LinearInterpolate(Vector3.Zero, 3 * deltaTime);
// 	}

// 	private void ProcessFire(){
// 		if(!Input.IsActionPressed("fire")) return;
// 		if(lastTimeFire + fireRate >= time) return;
// 		lastTimeFire = time;
// 		if(!raycast.IsColliding()) return;
// 		var target = raycast.GetCollider();
// 		if(target.HasMethod("Hit")) target.Call("Hit", 25);
// 		var particale = (Particles)hitParticale.Instance();
// 		GetTree().CurrentScene.AddChild(particale);
// 		var pos = raycast.GetCollisionPoint() - particale.Transform.origin;
// 		particale.Translate(pos);
// 		particale.LookAt(particale.Transform.origin + raycast.GetCollisionNormal() * 10, Vector3.Up);
// 		particale.Emitting = true;
// 	}
// }

