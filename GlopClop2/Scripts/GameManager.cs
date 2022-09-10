using Godot;
using System;

public class GameManager : Node{
	private OpenSimplexNoise noise = new OpenSimplexNoise();
	public Player player;
	private Camera2D cam;
	private float shakeStrength,shakeIntensity;
	private float shakeLength;
	private float shakeTime;
	public bool isCamShake;

	public override void _Ready(){
		player = GetNode<Player>("../GamePlay/Player");
		noise.Period = 1;
		cam = GetTree().CurrentScene.GetNode<Camera2D>("GamePlay/Cam");
	}

	public override void _Process(float delta){
		Utilities.deltaTime = delta;
		CamHendler();
	}

	private void CamHendler(){
		var camPos = player.Position + ((player.mousePos - player.Position) * .25f);
		if(shakeLength >= 0){
			shakeLength -= Utilities.deltaTime;
			camPos += ShakeCameraPos();
		}
		cam.Position = camPos;
	}	

	public void ShakeCamera(int shakeStrength, int shakeIntensity, float shakeLength){
		this.shakeStrength = shakeStrength;
		this.shakeIntensity = shakeIntensity;
		this.shakeLength = shakeLength;
	}

	private Vector2 ShakeCameraPos(){
		shakeTime += Utilities.deltaTime * shakeIntensity;
		return new Vector2(
			noise.GetNoise2d(1,shakeTime) * shakeStrength,
			noise.GetNoise2d(100,shakeTime) * shakeStrength
		);
	}
}
