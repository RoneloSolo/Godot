using Godot;
using System;

public class Player : KinematicBody
{
#region Varibales

	private const int DEACCELERATION = 5; // This Is DeAcceleration Of Player Movement
	private const int ACCELERATION = 3; // This Is Acceleration Of Player Movement
	private const int THRUST = 15; // This Is Jump Power
	private const int SPEED = 10; // This Is Player Walk Speed
	private const float MOUSESENSITIVITY = 0.05f; // This Is Mouse Sensetivity Of Playyer
	private const float GRAVITY = 19.62f; // This Is Gravity Of Player
	private float lastTimeFire; // This Is Last Time Player Shoot 
	private float horizontal; // This Is Input of player A,D
	private float deltaTime;// This Is Delta time
	private float forward; // This Is Input of player W,S
	private float time; // This is the time in seconds since the start of the application
	
	private Vector3 SWAYLEFT = new Vector3(0, 5, 0);
	private Vector3 SWAYRIGHT = new Vector3(0, -5, 0);
	private Vector3 SWAYUP = new Vector3(10, 0, 0);
	private Vector3 SWAYDOWN = new Vector3(-10, 0, 0);
	private Vector3 wallNormal; // Normal Of The Wall
	private Vector2 mouseMov; // This Needed For Weapon Sway Mechanic
	private Vector3 disVel; // Desire Velocity Of Player
	private Vector3 vel; // Velocity Of Player

	private Spatial head; // This Is Needed For Camera Rotation
	private Spatial hand; // This Is Needed For Weapon Rotation
	private Spatial handHor; // This Is For Sway Weapon Horizontaly
	private Spatial handVer; // This Is For Sway Weapon Verticly
	private Sprite crosshair;

	private RayCast raycastForward; // This Is Needed To Detect Wall Collision (Wall Run)
  	private Camera cam; // This Is Needed For Camera Rotation
	private AnimationPlayer anim;

	#endregion

	public override void _Ready(){
		handVer = GetNode<Spatial>("Head/Hand/HandHor/HandVer");
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
		crosshair = GetNode<Sprite>("Head/Cam/Crosshair");
		handHor = GetNode<Spatial>("Head/Hand/HandHor");
		raycastForward = GetNode<RayCast>("RayCast");
		hand = GetNode<Spatial>("Head/Hand");
		cam = GetNode<Camera>("Head/Cam");
		head = GetNode<Spatial>("Head");

		crosshair.GlobalPosition = GetViewport().GetVisibleRect().Size/2;
		Input.MouseMode = Input.MouseModeEnum.Captured;
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
		if(Input.MouseMode != Input.MouseModeEnum.Captured) return;
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
			if (Input.MouseMode == Input.MouseModeEnum.Visible) 
				Input.MouseMode = Input.MouseModeEnum.Captured;
			else Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		if(Input.IsActionPressed("fire0")) Fire();
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

	private const float FIRERATE = 1f;
	private bool isLeftFire;
	private bool isFire;

	private void Fire(){
		if(lastTimeFire + FIRERATE >= time) return;
		isFire = true;
		lastTimeFire = time;
		anim.PlaybackSpeed = 1/FIRERATE;
		if(isLeftFire) anim.Play("FireLeft");
		else anim.Play("FireRight");
		isLeftFire = !isLeftFire;
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

		if(isFire){
			if(!anim.IsPlaying()) isFire = false;
			return;
		}
		anim.PlaybackSpeed = 1;
		if(vel.Length() > 1 || vel.Length() < -1) anim.Play("Walk");
		else anim.Play("Idle");
	}
}
