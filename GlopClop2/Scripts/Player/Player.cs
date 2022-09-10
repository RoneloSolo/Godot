using Godot;
using System;

public class Player: KinematicBody2D {
	private int maxHealth = 10;
	private int speedRoll = 65;
	private int health = 10;
	private int speed = 35;
	private float maxRollTime = .5f;
	private float rollTimeLeft;
	private float time;
	private Vector2 velocity;
	public Vector2 mousePos;
	public Vector2 pos;

	private AnimationPlayer anim;
	private Sprite sprite;

	private Node2D spriteHolder;
	private Node2D hand;
	
	private Gun currentGun;
	private Gun shotgun;
	private Gun bazoka;
	private Gun ak;

	
	public override void _Ready() {
		hand = GetNode <Node2D> ("Hand");
		bazoka = GetNode <Gun> ("Hand/Bazoka");
		ak = GetNode <Gun> ("Hand/Ak");
		shotgun = GetNode <Gun> ("Hand/Shotgun");
		sprite = GetNode <Sprite> ("SpriteHolder/Sprite");
		spriteHolder = GetNode <Node2D> ("SpriteHolder");
		anim = GetNode <AnimationPlayer>("Anim");
	}

	public override void _Process(float delta) {
		time += delta;
		pos = GlobalPosition;
		mousePos = GetGlobalMousePosition();
		HandHendler();
		GunHendler();
		AnimationHendler();
		RollHendler();
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
		if (rollTimeLeft > 0) rollTimeLeft -= Utilities.deltaTime;
		else if (Input.IsActionJustPressed("Roll") && Mathf.Abs(velocity.Length()) > .1f) rollTimeLeft = maxRollTime;
	}

	private void GunHendler() {
		if (currentGun == null) return;
		if (Input.IsActionPressed("fire")) currentGun.Fire(time, mousePos);
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
}
