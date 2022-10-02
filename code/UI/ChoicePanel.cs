﻿using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;

namespace Sandbox;

public class ChoicePanel : Panel
{
	//private List<StatusIcon> _statusIcons = new List<StatusIcon>();

	public ChoicePanel()
	{
		var icon = new ChoiceModal(this);
		icon.AddClass("choice_modal");
		AddChild(icon);
	}

	public void OnChoiceMade(string statusName)
	{
		Log.Info("OnChoiceMade: " + statusName);

		ConsoleSystem.Run("add_status", statusName);
		//PlayerCitizen.AddStatusCmd(statusName);
		Delete();
	}
}

public class ChoiceModal : Panel
{
	public ChoicePanel ChoicePanel { get; set; }

	public ChoiceModal(ChoicePanel choicePanel)
    {
		ChoicePanel = choicePanel;

		var player = MyGame.Current.LocalPlayer;

		Label lvlLabel = new Label();
		lvlLabel.AddClass("choice_lvl_label");
		lvlLabel.Text = "Level " + player.Level;
		//titleLabel.Text = "Title";
		AddChild(lvlLabel);

		Panel buttonContainer = new Panel();
		buttonContainer.AddClass("choice_button_container");
		AddChild(buttonContainer);

		//Log.Info("--------");
		//      Log.Info("type: " + TypeLibrary.GetDescription("ChoiceButton").TargetType);
		//Log.Info("1. " + TypeLibrary.Create("b0", TypeLibrary.GetDescription("ChoiceButton").TargetType));
		//Log.Info("2. " + (TypeLibrary.Create("b0", TypeLibrary.GetDescription("ChoiceButton").TargetType) as ChoiceButton));
		//var button0 = TypeLibrary.Create("b0", TypeLibrary.GetDescription("ChoiceButton").TargetType) as ChoiceButton;
		//var button0 = TypeLibrary.Create<Panel>("ChoiceButton");

		List<string> statusNames = new List<string>() { "ExampleStatus", "ExampleStatus", "ExampleStatus2" };

		int NUM_CHOICES = 3;
		for(int i = 0; i < NUM_CHOICES; i++)
        {
			var statusName = statusNames[i];
			var status = TypeLibrary.Create<Status>(statusName);
			var button = new ChoiceButton(status);
			button.AddClass("choice_button");
			button.AddEventListener("onclick", () => ChoicePanel.OnChoiceMade(statusName));
			button.status = status;
			buttonContainer.AddChild(button);
		}
	}
}

public class ChoiceButton : Panel
{
	public Status status { get; set; }

	//public string Title;
	//public string Description;

    public ChoiceButton(Status status)
	{
		Label titleLabel = new Label();
		titleLabel.AddClass("choice_title");
		titleLabel.Text = status.Title;
        //titleLabel.Text = "Title";
		AddChild(titleLabel);

		Image icon = new Image();
		icon.AddClass("choice_icon");
		icon.SetTexture(status.IconPath);
		AddChild(icon);

		Label descriptionLabel = new Label();
		descriptionLabel.AddClass("choice_description");
		descriptionLabel.Text = status.Description;
        //descriptionLabel.Text = "Status description goes here.";
		AddChild(descriptionLabel);
	}

    //protected override void OnMouseOver(MousePanelEvent e)
    //{
    //    base.OnMouseOver(e);

    //    if (status == null || string.IsNullOrEmpty(status.Title) || string.IsNullOrEmpty(status.Description))
    //        return;

    //    Tippy.Create(this, Tippy.Pivots.TopRight).WithContent(status.Title, status.Description);
    //}
}
