using Godot;
using System;

/// <summary>
/// Build plan storing name, icon, and cost.
/// </summary>
[Tool]
public class BuildPlan : Resource{
	[Export] PackedScene prefab;
	[Export] Item[] costItem;
	[Export] int[] costAmount;

	/// <summary>
	/// Returning cost of building.
	/// item[] and amount[].
	/// </summary>
	public Cost GetCost(){
		var cost = new Cost(costItem, costAmount);
		return cost;
	}

	public class Cost{
		public Item[] item;
		public int[] amount;

		public Cost(Item[] _item, int[] _amount){
			item = _item;
			amount = _amount;
		}
	}

	public PackedScene GetPrefab() => prefab;
}
