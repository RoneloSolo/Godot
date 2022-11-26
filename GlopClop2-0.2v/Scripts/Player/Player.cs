using Godot;
using System;

public class Player: KinematicBody2D {
	private int maxHealth = 6;
	private int speedDash = 250;
	private int health = 6;
	private int speed = 70;
	private float maxRollTime = .5f;
	private float dashTimeLeft;
	private float dashEffectTime;
	private float deltaTime;
	private float time;
	private Vector2 velocity;
	public Vector2 mousePos;

	private AnimationPlayer anim;
	private Sprite sprite;

	private Node2D spriteHolder;
	private Node2D hand;

	private PackedScene dashEffect = GD.Load<PackedScene>("res://Prefab/Effects/Smoke.tscn");
	private Gun currentGun;
	private PackedScene ak = GD.Load<PackedScene>("res://Prefab/Guns/Ak.tscn");

	
	public override void _Ready() {
		hand = GetNode <Node2D> ("Hand");
		sprite = GetNode <Sprite> ("SpriteHolder/Sprite");
		spriteHolder = GetNode <Node2D> ("SpriteHolder");
		anim = GetNode <AnimationPlayer>("Anim");
	}

	public override void _Process(float delta) {
		deltaTime = delta;
		time += deltaTime;
		mousePos = GetGlobalMousePosition();
		HandHendler();
		GunHendler();
		PlayerAnimationHendler();
		DashHendler();
	}
	
	public override void _PhysicsProcess(float delta) => Movement();

	private void Movement() {
		if (dashTimeLeft > 0) {
			MoveAndSlide(velocity.Normalized() * speedDash);
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

	private void DashHendler() {
		if (Input.IsActionJustPressed("Roll") && Mathf.Abs(velocity.Length()) > .1f && dashTimeLeft <= 0) dashTimeLeft = maxRollTime;
		if (dashTimeLeft < 0) return;
		dashTimeLeft -= deltaTime;
		if(dashEffectTime <= 0) {
			dashEffectTime = .05f;
			var eff = (Node2D)dashEffect.Instance();
			GetTree().CurrentScene.AddChild(eff);
			eff.Position = Position;
		}
		dashEffectTime  -= deltaTime;
	}

	private void GunHendler() {
		if (currentGun == null) return;
		if (Input.IsActionPressed("fire")) currentGun.Fire(time, mousePos);
	}

	public void Hit(Vector2 pos, int i, Godot.Object obj){
		if (obj.HasMethod("Hit")) obj.Call("Hit");
		health -=i;
		anim.Play("Hit");
		if(health<=0) Die();
	}

	private void Die() {
		health = maxHealth;
		GlobalPosition = Vector2.Zero;
	}

	private void PlayerAnimationHendler() {
		if (Mathf.Abs(velocity.Length()) > 0) ChangeAnim("Walk");
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
