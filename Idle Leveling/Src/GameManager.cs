using Godot;
using System;

public class GameManager : Node2D{
	public Unit player = new Unit(2,6,6);
	public Unit enemy = new Unit();

	public int level;
	public int levelXp;
	public int xpToLevelUp = 3;

	public ProgressBar playerHealthBar;
	public ProgressBar enemyHealthBar;
	public ProgressBar levelBar;

	ColorRect inventoryUI;

	enum Turn{PLAYER, ENEMY};
	Turn turn;
	Timer timer;

	private void InitializeVaribales(){
		playerHealthBar = GetNode<ProgressBar>("UI/Player Health");
		enemyHealthBar = GetNode<ProgressBar>("UI/Enemy Health");
		levelBar = GetNode<ProgressBar>("UI/Level Bar");
		inventoryUI = GetNode<ColorRect>("UI/Inventory");
		timer = GetNode<Timer>("Timer");
	}

	private void UpdateBattleUI(){
		playerHealthBar.Value = player.health;
		playerHealthBar.MaxValue = player.maxHealth;

		enemyHealthBar.Value = enemy.health;
		enemyHealthBar.MaxValue = enemy.maxHealth;

		levelBar.Value = levelXp;
		levelBar.MaxValue = xpToLevelUp;
	}

	private void EnemyDefeat(){
		enemy.maxHealth = 4;
		enemy.health = 4;
		enemy.damage = 1;

		levelXp += 1;
	}

	private void PlayerDefeat(){
		player.maxHealth = 6;
		player.health = 6;
		player.damage = 2;
	}

	private void StartGame(){
		EnemyDefeat();
		UpdateBattleUI();
		turn = Turn.PLAYER;
	}

	public override void _Ready(){
		InitializeVaribales();
		StartGame();
	}

	private void PlayerTurn(){
		if(enemy.TakeDamage(player.damage)) EnemyDefeat();
		if(levelXp >= xpToLevelUp) LevelUp();
		UpdateBattleUI();
		turn = Turn.ENEMY;
	}

	private void EnemyTurn(){
		if(player.TakeDamage(enemy.damage)) PlayerDefeat();
		UpdateBattleUI();
		turn = Turn.PLAYER;
	}

	private void LevelUp(){
		player.maxHealth += 6;
		player.health = player.maxHealth;
		player.damage += 4;
		levelXp = 0;
		level++;
		xpToLevelUp += Mathf.RoundToInt(Mathf.Pow(level, 1f / 1.5f));
		UpdateBattleUI();
	}

	public override void _Process(float delta){
		Battle();
	}

	public override void _Input(InputEvent inputEvent){
		if(inputEvent.IsActionPressed("E")) OpenInventory();
	}

	private void OpenInventory(){
		timer.Paused = !timer.Paused;
		inventoryUI.Visible = !inventoryUI.Visible;
	}

	private void Battle(){
		if(timer.Paused) return;
		if(timer.TimeLeft > 0.02f) return;
		if(turn == Turn.ENEMY) EnemyTurn();
		else PlayerTurn();
	}
}

