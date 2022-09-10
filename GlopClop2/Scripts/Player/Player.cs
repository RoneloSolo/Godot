using Godot;
using System;

public class Player: KinematicBody2D {
	private int maxHealth = 10;
	private int speedRoll = 65;
	private int health = 10;
	private int speed = 35;
	private float maxRollTime = .5f;
	private float rollTimeLeft;
	private float shakeTime;
	private float deltaTime;
	private float time;
	private Vector2 velocity;
	private Vector2 mousePos;
	public bool isCamShake;

	private AnimationPlayer anim;
	private Sprite sprite;

	private Node2D spriteHolder;
	private Node2D hand;
	private Camera2D cam;
	
	private Gun currentGun;
	private Gun shotgun;
	private Gun bazoka;
	private Gun ak;

	OpenSimplexNoise noise = new OpenSimplexNoise();
	
	public override void _Ready() {
		hand = GetNode <Node2D> ("Hand");
		bazoka = GetNode <Gun> ("Hand/Bazoka");
		ak = GetNode <Gun> ("Hand/Ak");
		shotgun = GetNode <Gun> ("Hand/Shotgun");
		sprite = GetNode <Sprite> ("SpriteHolder/Sprite");
		spriteHolder = GetNode <Node2D> ("SpriteHolder");
		anim = GetNode <AnimationPlayer>("Anim");
		cam = GetTree().CurrentScene.GetNode<Camera2D>("GamePlay/Cam");

		noise.Period = 25;
	}

	public override void _Process(float delta) {
		deltaTime = delta;
		time += delta;
		mousePos = GetGlobalMousePosition();
		HandHendler();
		GunHendler();
		AnimationHendler();
		RollHendler();
		CamHendler();
	}

	public override void _PhysicsProcess(float delta) => Movement();

	public override void _Input(InputEvent inputEvent) {
		if (inputEvent.IsActionPressed("Slot_1")) {
			if (currentGun == null) {
				currentGun = ak;
				currentGun.anim.Play("Idle_Ak");
			} 

			else currentGun.Off();
			currentGun = ak;
			currentGun.anim.Play("Idle_Ak");
			currentGun.On();

		} else if (inputEvent.IsActionPressed("Slot_2")) {
			if (currentGun == null) {
				currentGun = shotgun;
				currentGun.anim.Play("Idle_Shotgun");
			} 

			else currentGun.Off();
			currentGun = shotgun;
			currentGun.anim.Play("Idle_Shotgun");
			currentGun.On();

		} else if (inputEvent.IsActionPressed("Slot_3")) {
			if (currentGun == null) {
				currentGun = bazoka;
				currentGun.anim.Play("Idle_Bazoka");
			} 

			else currentGun.Off();
			currentGun = bazoka;
			currentGun.anim.Play("Idle_Bazoka");
			currentGun.On();
		}
	}

	private void Movement() {
		if (rollTimeLeft > 0) {
			MoveAndSlide(velocity.Normalized() * speedRoll);
			return;
		}
		velocity.x = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		velocity.y = Input.GetActionStrength("down") - Input.GetActionStrength("up");
		MoveAndSlide(velocity.Normalized() * speed);
	}

	private void HandHendler() {
		hand.LookAt(mousePos);
		if (Mathf.Rad2Deg(hand.Rotation) > 180) hand.Rotation = Mathf.Deg2Rad(-181);
		else if (Mathf.Rad2Deg(hand.Rotation) < -180) hand.Rotation = Mathf.Deg2Rad(181);
		if (Mathf.Rad2Deg(hand.Rotation) > -90 && Mathf.Rad2Deg(hand.Rotation) < 90) hand.Scale = new Vector2(1, 1);
		else hand.Scale = new Vector2(1, -1);
		if (Position.x < mousePos.x && sprite.FlipH) sprite.FlipH = false;
		else if (Position.x > mousePos.x && !sprite.FlipH) sprite.FlipH = true;
	}

	private void RollHendler() {
		if (rollTimeLeft > 0) rollTimeLeft -= deltaTime;
		else if (Input.IsActionJustPressed("Roll") && Mathf.Abs(velocity.Length()) > .1f) rollTimeLeft = maxRollTime;
	}

	private void GunHendler() {
		if (currentGun == null) return;
		if (Input.IsActionPressed("fire")) {
			currentGun.Fire(time);
		}else if(isCamShake) isCamShake = false;
	}

	public void Hit(int i) {
		health -= i;
		if (health <= 0) Die();
	}

	private void Die() {
		health = maxHealth;
		GlobalPosition = Vector2.Zero;
	}

	private void AnimationHendler() {
		if (rollTimeLeft > 0) ChangeAnim("Roll");
		else if (Mathf.Abs(velocity.Length()) > 0) ChangeAnim("Walk");
		else ChangeAnim("Idle");
	}

	private void ChangeAnim(string animName) {
		if (anim.CurrentAnimation == animName) return;
		anim.Stop();
		spriteHolder.Scale = new Vector2(1, 1);
		spriteHolder.Rotation = 0;
		anim.Play(animName);
	}

	private void CamHendler(){
		var camPos = Position + ((mousePos - Position) * .25f);
		if(isCamShake) {
			cam.Position = camPos + ShakeCamera();
			return;
		}
		cam.Position = camPos;
	}

	private Vector2 ShakeCamera(int shakeStrength = 5, int shakeIntensity = 125){
		shakeTime += deltaTime * shakeIntensity;
		return new Vector2(
			noise.GetNoise2d(1,shakeTime) * shakeStrength,
			noise.GetNoise2d(100,shakeTime) * shakeStrength
		);
	}
}
