using Godot;
using System;

public class Player : KinematicBody2D
{
	private int health = 10;
	private int maxHealth = 10;
	private int speed = 35;
	private float time;
	private float rollTimeLeft;
	private float maxRollTime = .5f;
	private int speedRoll = 65;
	private Vector2 velocity;
	private Vector2 mousePos;

	private Sprite sprite;
	private AnimationPlayer anim;

	private Node2D spriteHolder;
	private Node2D hand;
	private Gun currentGun;
	private Gun bazoka;
	private Gun ak;
	private Gun shotgun;

	public override void _Ready()
	{
		hand = GetNode<Node2D>("Hand");
		bazoka = GetNode<Gun>("Hand/Bazoka");
		ak = GetNode<Gun>("Hand/Ak");
		shotgun = GetNode<Gun>("Hand/Shotgun");
		sprite = GetNode<Sprite>("SpriteHolder/Sprite");
		spriteHolder = GetNode<Node2D>("SpriteHolder");
		anim = GetNode<AnimationPlayer>("Anim");
	}

	public override void _Process(float delta)
	{
		time += delta;
		HandHendler();
		mousePos = GetGlobalMousePosition();
		GunHendler();
		AnimationHendler();
		RollHendler(delta);
	}

	public override void _PhysicsProcess(float delta)
	{
		Movement();
	}

	public override void _Input(InputEvent inputEvent){
		if(inputEvent.IsActionPressed("Slot_1"))
		{
			if(currentGun == null)
			{
				currentGun = ak;
			}
			else currentGun.Off();
			currentGun = ak;
			currentGun.On();
		}
		else if(inputEvent.IsActionPressed("Slot_2"))
		{
			if(currentGun == null)
			{
				currentGun = shotgun;
			}
			else currentGun.Off();
			currentGun = shotgun;
			currentGun.On();
		}
		else if(inputEvent.IsActionPressed("Slot_3"))
		{
			if(currentGun == null)
			{
				currentGun = bazoka;
			}
			else currentGun.Off();
			currentGun = bazoka;
			currentGun.On();
		}
	}

	private void Movement()
	{
		if(rollTimeLeft > 0) MoveAndSlide(velocity.Normalized() * speedRoll);
		else {
			velocity.x = Input.GetActionStrength("right") - Input.GetActionStrength("left");
			velocity.y = Input.GetActionStrength("down") - Input.GetActionStrength("up");
			MoveAndSlide(velocity.Normalized() * speed);
		}
	}

	private void HandHendler(){
		hand.LookAt(mousePos);
		if(Mathf.Rad2Deg(hand.Rotation) > 180) hand.Rotation = Mathf.Deg2Rad(-181);
		else if(Mathf.Rad2Deg(hand.Rotation) < -180) hand.Rotation = Mathf.Deg2Rad(181);
		if(Mathf.Rad2Deg(hand.Rotation) > -90 && Mathf.Rad2Deg(hand.Rotation) < 90) hand.Scale = new Vector2(1,1);
		else hand.Scale = new Vector2(1,-1);
		if(Position.x < mousePos.x && sprite.FlipH) sprite.FlipH = false;
		else if(Position.x > mousePos.x && !sprite.FlipH) sprite.FlipH = true;
	}	

	private void RollHendler(float delta){
		if(rollTimeLeft > 0) rollTimeLeft -= delta;
		else if(Input.IsActionJustPressed("Roll") && Mathf.Abs(velocity.Length()) > .1f) rollTimeLeft = maxRollTime;
	}
	private void GunHendler()
	{
		if(currentGun == null) return;
		if(Input.IsActionPressed("fire")) currentGun.Fire(time);
	}

	public void Hit(int i){
		health -=i;
		if(health <= 0) Die();
	}
	
	void Die(){
		health = maxHealth;
		GlobalPosition = Vector2.Zero;
	}

	void AnimationHendler(){
		if(Mathf.Abs(velocity.Length()) > 0) ChangeAnim("Walk");
		else ChangeAnim("Idle");
	}

	void ChangeAnim(string animName){
		if(anim.CurrentAnimation == animName) return;
		anim.Stop();
		spriteHolder.Scale = new Vector2(1,1);
		spriteHolder.Rotation = 0;
		anim.Play(animName);
	}
}
