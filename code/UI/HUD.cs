﻿using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;

namespace Test2D;

public class PlayerCursor : Panel
{

	public PlayerCursor()
	{

	}

	public override void Tick()
	{

		Style.Top = Parent.MousePosition.y * ScaleToScreen;
		Style.Left = Parent.MousePosition.x * ScaleToScreen;

	}

}

//public partial class HUD : Sandbox.HudEntity<RootPanel>
public partial class HUD : RootPanel
{
	//public static Vector2 MousePos { get; private set; }

	public StatusPanel StatusPanel { get; private set; }
	public InfoPanel InfoPanel { get; private set; }
	public TimerPanel TimerPanel { get; private set; }
	public ChoicePanel ChoicePanel { get; set; }
	public DeathPanel DeathPanel{ get; set; }
	private EnemyNametag _bossNametag;

	public HUD()
	{
		Sandbox.Game.RootPanel = this;

		//RootPanel.StyleSheet.Load("ui/HUD.scss");
		//      RootPanel.AddChild<ToolsPanel>("tools");
		//      RootPanel.AddChild<ToolsPanel>("tools");

		StyleSheet.Load("ui/HUD.scss");

		StatusPanel = AddChild<StatusPanel>("status_panel");
		InfoPanel = AddChild<InfoPanel>("info_panel");
		TimerPanel = AddChild<TimerPanel>("timer_panel");

        AddChild<BoomerChatBox>();
        //var modal = AddChild<Modal>("modal");

        //AddChild<PlayerCursor>("cursor");
    }

	public void SpawnChoicePanel()
    {
		if (MyGame.Current.IsGameOver)
			return;

		ChoicePanel = AddChild<ChoicePanel>("choice_panel_root");
	}

	public Nametag SpawnNametag(PlayerCitizen player)
    {
		var nametag = AddChild<Nametag>();
		nametag.Player = player;

		if(player == MyGame.Current.LocalPlayer)
			nametag.AddReloadBar();

		return nametag;
	}

	public EnemyNametag SpawnEnemyNametag(Enemy enemy)
	{
		if (_bossNametag != null)
        {
			_bossNametag.Delete();
			_bossNametag = null;
		}

		_bossNametag = AddChild<EnemyNametag>();
		_bossNametag.Enemy = enemy;

		return _bossNametag;
	}

	public void GameOver()
    {
		DeathPanel = AddChild<DeathPanel>("death_panel_root");
		DeathPanel.FailureAsync();


        RemoveChoicePanel();
	}

	public void Victory()
	{
        if (_bossNametag != null)
        {
            _bossNametag.Delete();
            _bossNametag = null;
        }
        
		DeathPanel = AddChild<DeathPanel>("death_panel_root");
		DeathPanel.VictoryAsync();

		RemoveChoicePanel();
	}

	public void RemoveChoicePanel()
    {
		if (ChoicePanel != null)
		{
			ChoicePanel.Delete();
			ChoicePanel = null;
		}
	}

	public void Restart()
    {
		if (ChoicePanel != null)
        {
			ChoicePanel.Delete();
			ChoicePanel = null;
		}

		if(DeathPanel != null)
        {
			DeathPanel.Delete();
			DeathPanel = null;
		}

		if (_bossNametag != null)
		{
			_bossNametag.Delete();
			_bossNametag = null;
		}
	}
}
