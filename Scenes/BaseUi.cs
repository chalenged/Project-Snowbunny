using Godot;
using System;

public partial class BaseUi : Control
{
	public override void _Process(double delta)
	{
		GetNode<ProgressBar>("VBoxContainer/HealthBar").Value = GameController.Instance.PlayerHealth;
		GetNode<RichTextLabel>("VBoxContainer/Ammo").Text = $"{GameController.Instance.ammoCur} / {GameController.Instance.ammoMax}";
	}
}
