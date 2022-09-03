using Godot;
using System;

public class PlayerNew: KinematicBody{
	#region Varibales

	private const int DEACCELERATION = 5; // This Is DeAcceleration Of Player Movement
	private const int ACCELERATION = 3; // This Is Acceleration Of Player Movement
	private const int THRUST = 15; // This Is Jump Power
	private const int SPEED = 10; // This Is Player Walk Speed
	private const float MOUSESENSITIVITY = 0.05f; // This Is Mouse Sensetivity Of Playyer
	private const float GRAVITY = 19.62f; // This Is Gravity Of Player
	private const float FIRERATE = .25f; // This Is Fire Rate Of Player
	private float lastTimeFire; // This Is Last Time Player Shoot 
	private float horizontal; // This Is Input of player A,D
	private float deltaTime;// This Is Delta time
	private float forward; // This Is Input of player W,S
	private float time; // This is the time in seconds since the start of the application
	
	private Vector3 SWAYLEFT = new Vector3(0, 5, 0);
	private Vector3 SWAYRIGHT = new Vector3(0, -5, 0);
	private Vector3 SWAYUP = new Vector3(10, 0, 0);
	private Vector3 SWAYDOWN = new Vector3(-10, 0, 0);
	private Vector2 mouseMov; // This Needed For Weapon Sway Mechanic
	private Vector3 disVel; // Desire Velocity Of Player
	private Vector3 vel; // Velocity Of Player

	private Spatial head; // This Is Needed For Camera Rotation
	private Spatial hand; // This Is Needed For Weapon Rotation
	private Spatial handHor; // This Is For Sway Weapon Horizontaly
	private Spatial handVer; // This Is For Sway Weapon Verticly
	private Sprite crosshair;

	private RayCast raycast; // This Is Needed To Detect Weapon Hit
  	private Camera cam; // This Is Needed For Camera Rotation

	private PackedScene hitParticale = (PackedScene)ResourceLoader.Load("res://Prefabs/HitParticale.tscn"); 

	#endregion

	public override void _Ready(){
		handVer = GetNode<Spatial>("Head/Hand/HandHor/HandVer");
		crosshair = GetNode<Sprite>("Head/Cam/Crosshair");
		handHor = GetNode<Spatial>("Head/Hand/HandHor");
		raycast = GetNode<RayCast>("Head/Cam/RayCast");
		hand = GetNode<Spatial>("Head/Hand");
		cam = GetNode<Camera>("Head/Cam");
		head = GetNode<Spatial>("Head");

		crosshair.GlobalPosition = GetViewport().GetVisibleRect().Size/2;
		Input.SetMouseMode(Input.MouseMode.Captured);
	}

	public override void _Process(float delta){
		ProccesMain(delta);
		ProcessAnimation();
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
	}

	private void ProcessInput(){
		horizontal = (Input.GetActionStrength("right") - Input.GetActionStrength("left"));
		forward = (Input.GetActionStrength("backward") - Input.GetActionStrength("forward"));
		if (Input.IsActionJustPressed("ui_cancel")) {
			if (Input.GetMouseMode() == Input.MouseMode.Visible) 
				Input.SetMouseMode(Input.MouseMode.Captured);
			else Input.SetMouseMode(Input.MouseMode.Visible);
		}
		if(Input.IsActionPressed("fire")) ProcessFire();
	}

	private void ProcessMovement(){
		disVel = horizontal * Transform.basis.x;
		disVel += forward * Transform.basis.z;
		disVel = disVel.Normalized();
		vel = (Mathf.Abs(disVel.Length()) > .1f) ? 
			vel.LinearInterpolate(disVel * SPEED, ACCELERATION * deltaTime) :  
			vel.LinearInterpolate(disVel * SPEED, DEACCELERATION * deltaTime);

		if(!IsOnFloor()) vel.y -= GRAVITY * deltaTime;
		else if(IsOnFloor() && Input.IsActionJustPressed("jump")) vel.y = THRUST;
	}

	private void ProccesCamera(){
		head.RotateX(-Mathf.Deg2Rad(mouseMov.y * MOUSESENSITIVITY));
		RotateY(Mathf.Deg2Rad(mouseMov.x * MOUSESENSITIVITY));
		Vector3 cameraRot = head.RotationDegrees;
		cameraRot.x = Mathf.Clamp(cameraRot.x, -90, 90);
		head.RotationDegrees = cameraRot;
	}

	private void ProcessAnimation(){
		if(horizontal > 0) hand.RotationDegrees = hand.RotationDegrees.LinearInterpolate(new Vector3(0,0,-5), deltaTime * 5);
		else if(horizontal < 0) hand.RotationDegrees = hand.RotationDegrees.LinearInterpolate(new Vector3(0,0,5), deltaTime * 5);
		else hand.RotationDegrees = hand.RotationDegrees.LinearInterpolate(new Vector3(0,0,0), deltaTime * 5);

		if(mouseMov.x > 3) handHor.RotationDegrees = handHor.RotationDegrees.LinearInterpolate(SWAYLEFT, 2 * deltaTime);
		else if(mouseMov.x < -3) handHor.RotationDegrees = handHor.RotationDegrees.LinearInterpolate(SWAYRIGHT, 2 * deltaTime);
		else handHor.RotationDegrees = handHor.RotationDegrees.LinearInterpolate(Vector3.Zero, 2 * deltaTime);

		if(mouseMov.y > 3) handVer.RotationDegrees = handVer.RotationDegrees.LinearInterpolate(SWAYDOWN, 3 * deltaTime);
		else if(mouseMov.y < -3) handVer.RotationDegrees = handVer.RotationDegrees.LinearInterpolate(SWAYUP, 3 * deltaTime);
		else handVer.RotationDegrees = handVer.RotationDegrees.LinearInterpolate(Vector3.Zero, 3 * deltaTime);
	}

	private void ProcessFire(){
		if(lastTimeFire + FIRERATE >= time) return;
		lastTimeFire = time;
		if(!raycast.IsColliding()) return;
		var target = raycast.GetCollider();
		if(target.HasMethod("Hit")) target.Call("Hit", 25);
		var particale = (Particles)hitParticale.Instance();
		GetTree().CurrentScene.AddChild(particale);
		var pos = raycast.GetCollisionPoint() - particale.Transform.origin;
		particale.Translate(pos);
		particale.LookAt(particale.Transform.origin + raycast.GetCollisionNormal() * 10, Vector3.Up);

		if(target.IsClass("StaticBody")) particale.DrawPass1.SurfaceSetMaterial(0,((StaticBody)target).GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0));
		else if(target.IsClass("CSGSphere")) particale.DrawPass1.SurfaceSetMaterial(0,((CSGSphere)target).Material);
		else if(target.IsClass("KinematicBody")) particale.DrawPass1.SurfaceSetMaterial(0,((KinematicBody)target).GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0));
		else if(target.IsClass("CSGBox")) particale.DrawPass1.SurfaceSetMaterial(0,((CSGBox)target).Material);
		
		particale.Emitting = true;
	}
}
