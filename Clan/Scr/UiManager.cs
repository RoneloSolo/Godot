using Godot;
using System;

public class UiManager : Node2D{
	Node2D gameUiTab;
	Node2D buildUiTab;
	
	public override void _Ready(){
		gameUiTab = GetNode<Node2D>("../../UI/Game UI");
		buildUiTab = GetNode<Node2D>("../../UI/Build UI");
	}

	private void OpenBuildTab(){
		buildUiTab.Visible = true;
		gameUiTab.Visible = false;
	}

	private void CloseBuildTab(){
		buildUiTab.Visible = false;
		gameUiTab.Visible = true;
	}
}

