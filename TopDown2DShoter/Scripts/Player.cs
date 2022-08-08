using Godot;
using System;

public class Player : KinematicBody2D{

	// player
	private Vector2 vel = new Vector2();
	private float speed = 3.5f;
	Node2D hand;
	
	PlayerState currentState;
	PlayerState range;
	PlayerState melle;
	PlayerState sheild;
	PlayerState push;
	PlayerState ult;

	float time = 0;
	float timeDiceNext = 0;
	float diceInterval = 5;
	
	public override void _Ready(){
		hand = GetNode<Node2D>("Hand");
		range = GetNode<PlayerState>("Hand/range");
		melle = GetNode<PlayerState>("Hand/melle");
		sheild = GetNode<PlayerState>("Hand/reflect");
		ult = GetNode<PlayerState>("Hand/ult");
		push = GetNode<PlayerState>("Hand/push");
		GD.Randomize();
	}
	
	public override void _Process(float delta){
		time += delta;
		HandHendler();
		Movement();
		DiceHendler();
		if(currentState == null) return;
		currentState.Update();
	}
	
	public override void _Input(InputEvent inputEvent){	
		if(currentState == null) return;
		currentState.input(inputEvent);
	}

	void DiceHendler(){
		if(timeDiceNext > time) return;
		timeDiceNext = time + diceInterval;
		var num = (int)GD.RandRange(0,2);
		if(num == 1) ChangeState();
	}

	void ChangeState(){
		var num = (int)GD.RandRange(0,6);
		if(num == 0) SwitchState(range);
		if(num == 1) SwitchState(melle);
		if(num == 2) SwitchState(sheild);
		if(num == 3) SwitchState(push);
		if(num == 4) SwitchState(ult);
		if(num == 5) SwitchState(null);
	}

	void SwitchState(PlayerState state){
		if(currentState != null) currentState.Off();
		currentState = state;
		if(state == null) return;
		currentState.On();
	}
	
	void Movement(){
		vel.x = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		vel.y = Input.GetActionStrength("down") - Input.GetActionStrength("up");
		MoveAndCollide(vel.Normalized() * speed);
	}

	void HandHendler(){
		hand.LookAt(GetGlobalMousePosition());
		if(Rad2Deg(hand.Rotation) > 180) hand.Rotation = Deg2Rad(-181);
		else if(Rad2Deg(hand.Rotation) < -180) hand.Rotation = Deg2Rad(181);
		if(Rad2Deg(hand.Rotation) > -90 && Rad2Deg(hand.Rotation) < 90) hand.SetScale(new Vector2(1,1));
		else hand.SetScale(new Vector2(1,-1));
	}

	float Rad2Deg(float num){
		return num * 180 / Mathf.Pi;
	}
	
	float Deg2Rad(float num){
		return num * Mathf.Pi / 180;
	}
}
