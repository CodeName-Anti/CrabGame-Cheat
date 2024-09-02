using HarmonyLib;
using ImGuiNET;
using JNNJMods.CrabCheat.Util;
using SharpGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace JNNJMods.CrabCheat.Rendering;

public class GUIRenderer
{
	public Dictionary<TabID, GUITab> Tabs { get; private set; }
	public TabID CurrentTabId { get; private set; }

	public bool RenderGUI
	{
		get => _RenderGUI;
		set
		{
			_RenderGUI = value;
			GUI.HandleInput = _RenderGUI;
			GUI.BlockInput = _RenderGUI;
		}
	}

	private bool _RenderGUI;

	private ImGuiKey MenuKey = ImGuiKey.RightShift;

	public void Initialize()
	{
		SetKey();

		Tabs = Enum.GetValues<TabID>().ToDictionary(id => id, id => new GUITab(TabHelper.GetTabName(id), id));

		// Disable LobbyOwner tab
		Tabs[TabID.LobbyOwner].Enabled = false;

		CurrentTabId = Tabs.ElementAt(0).Key;

		GUI.OnRender += OnRender;
		GUI.OnInitImGui += SetupImGuiStyle;

		GUI.Initialize();
	}

	private void SetKey()
	{
		MenuKey = Cheat.Instance.ModuleManager.ClickGuiKeyBind.ToImGuiKey();
	}

	public void Shutdown()
	{
		GUI.Shutdown();
	}

	private void OnRender()
	{
		try
		{
			RenderModules();
		}
		catch (Exception ex)
		{
			CheatLog.Error(ex.ToString());
		}
	}

	private void RenderModules()
	{
		if (ImGui.IsKeyPressed(MenuKey, false))
			RenderGUI = !RenderGUI;

		// Call OnRender for things like ESP
		Cheat.Instance.ModuleManager.ExecuteForModules(m => m.OnRender());

		if (!RenderGUI)
			return;

		ImGui.Begin("CrabCheat by JNNJ");

		if (ImGui.BeginTabBar("tabs"))
		{
			foreach ((TabID tabId, GUITab guiTab) in Tabs)
			{
				if (!guiTab.Enabled)
					continue;

				if (ImGui.TabItemButton(guiTab.Name))
					CurrentTabId = tabId;
			}

			ImGui.EndTabBar();
		}

		Cheat.Instance.ModuleManager.Modules.Where(m => m.TabId == CurrentTabId).Do(m =>
		{
			try
			{
				m.RenderGUIElements();
			}
			catch (Exception ex)
			{
				CheatLog.Error("Exception in Module \"" + m.Name + "\": " + ex.ToString());
			}
		});

		ImGui.SetWindowSize(new Vector2(0, 0), ImGuiCond.Once);

		ImGui.End();
	}

	private static void SetupImGuiStyle()
	{
		// Future Dark stylerewrking from ImThemes
		ImGuiStylePtr style = ImGui.GetStyle();

		style.Alpha = 1.0f;
		style.DisabledAlpha = 1.0f;
		style.WindowPadding = new Vector2(12.0f, 12.0f);
		style.WindowRounding = 0.0f;
		style.WindowBorderSize = 0.0f;
		style.WindowMinSize = new Vector2(20.0f, 20.0f);
		style.WindowTitleAlign = new Vector2(0.5f, 0.5f);
		style.WindowMenuButtonPosition = ImGuiDir.None;
		style.ChildRounding = 0.0f;
		style.ChildBorderSize = 1.0f;
		style.PopupRounding = 0.0f;
		style.PopupBorderSize = 1.0f;
		style.FramePadding = new Vector2(6.0f, 6.0f);
		style.FrameRounding = 0.0f;
		style.FrameBorderSize = 0.0f;
		style.ItemSpacing = new Vector2(12.0f, 6.0f);
		style.ItemInnerSpacing = new Vector2(6.0f, 3.0f);
		style.CellPadding = new Vector2(12.0f, 6.0f);
		style.IndentSpacing = 20.0f;
		style.ColumnsMinSpacing = 6.0f;
		style.ScrollbarSize = 12.0f;
		style.ScrollbarRounding = 0.0f;
		style.GrabMinSize = 12.0f;
		style.GrabRounding = 0.0f;
		style.TabRounding = 0.0f;
		style.TabBorderSize = 0.0f;
		style.TabMinWidthForCloseButton = 0.0f;
		style.ColorButtonPosition = ImGuiDir.Right;
		style.ButtonTextAlign = new Vector2(0.5f, 0.5f);
		style.SelectableTextAlign = new Vector2(0.0f, 0.0f);

		style.Colors[(int)ImGuiCol.Text] = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
		style.Colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.2745098173618317f, 0.3176470696926117f, 0.4509803950786591f, 1.0f);
		style.Colors[(int)ImGuiCol.WindowBg] = new Vector4(0.0784313753247261f, 0.08627451211214066f, 0.1019607856869698f, 1.0f);
		style.Colors[(int)ImGuiCol.ChildBg] = new Vector4(0.0784313753247261f, 0.08627451211214066f, 0.1019607856869698f, 1.0f);
		style.Colors[(int)ImGuiCol.PopupBg] = new Vector4(0.0784313753247261f, 0.08627451211214066f, 0.1019607856869698f, 1.0f);
		style.Colors[(int)ImGuiCol.Border] = new Vector4(0.1568627506494522f, 0.168627455830574f, 0.1921568661928177f, 1.0f);
		style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.0784313753247261f, 0.08627451211214066f, 0.1019607856869698f, 1.0f);
		style.Colors[(int)ImGuiCol.FrameBg] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
		style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.1568627506494522f, 0.168627455830574f, 0.1921568661928177f, 1.0f);
		style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.2352941185235977f, 0.2156862765550613f, 0.5960784554481506f, 1.0f);
		style.Colors[(int)ImGuiCol.TitleBg] = new Vector4(0.0470588244497776f, 0.05490196123719215f, 0.07058823853731155f, 1.0f);
		style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.0470588244497776f, 0.05490196123719215f, 0.07058823853731155f, 1.0f);
		style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.0784313753247261f, 0.08627451211214066f, 0.1019607856869698f, 1.0f);
		style.Colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.09803921729326248f, 0.105882354080677f, 0.1215686276555061f, 1.0f);
		style.Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.0470588244497776f, 0.05490196123719215f, 0.07058823853731155f, 1.0f);
		style.Colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
		style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.1568627506494522f, 0.168627455830574f, 0.1921568661928177f, 1.0f);
		style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
		style.Colors[(int)ImGuiCol.CheckMark] = new Vector4(0.4980392158031464f, 0.5137255191802979f, 1.0f, 1.0f);
		style.Colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.4980392158031464f, 0.5137255191802979f, 1.0f, 1.0f);
		style.Colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.5372549295425415f, 0.5529412031173706f, 1.0f, 1.0f);
		style.Colors[(int)ImGuiCol.Button] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
		style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.196078434586525f, 0.1764705926179886f, 0.5450980663299561f, 1.0f);
		style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.2352941185235977f, 0.2156862765550613f, 0.5960784554481506f, 1.0f);
		style.Colors[(int)ImGuiCol.Header] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
		style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.196078434586525f, 0.1764705926179886f, 0.5450980663299561f, 1.0f);
		style.Colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.2352941185235977f, 0.2156862765550613f, 0.5960784554481506f, 1.0f);
		style.Colors[(int)ImGuiCol.Separator] = new Vector4(0.1568627506494522f, 0.1843137294054031f, 0.250980406999588f, 1.0f);
		style.Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.1568627506494522f, 0.1843137294054031f, 0.250980406999588f, 1.0f);
		style.Colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.1568627506494522f, 0.1843137294054031f, 0.250980406999588f, 1.0f);
		style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
		style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.196078434586525f, 0.1764705926179886f, 0.5450980663299561f, 1.0f);
		style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.2352941185235977f, 0.2156862765550613f, 0.5960784554481506f, 1.0f);
		style.Colors[(int)ImGuiCol.Tab] = new Vector4(0.0470588244497776f, 0.05490196123719215f, 0.07058823853731155f, 1.0f);
		style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
		//style.Colors[(int)ImGuiCol.TabActive] = new Vector4(0.09803921729326248f, 0.105882354080677f, 0.1215686276555061f, 1.0f);
		//style.Colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.0470588244497776f, 0.05490196123719215f, 0.07058823853731155f, 1.0f);
		//style.Colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.0784313753247261f, 0.08627451211214066f, 0.1019607856869698f, 1.0f);
		style.Colors[(int)ImGuiCol.PlotLines] = new Vector4(0.5215686559677124f, 0.6000000238418579f, 0.7019608020782471f, 1.0f);
		style.Colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(0.03921568766236305f, 0.9803921580314636f, 0.9803921580314636f, 1.0f);
		style.Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(1.0f, 0.2901960909366608f, 0.5960784554481506f, 1.0f);
		style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(0.9960784316062927f, 0.4745098054409027f, 0.6980392336845398f, 1.0f);
		style.Colors[(int)ImGuiCol.TableHeaderBg] = new Vector4(0.0470588244497776f, 0.05490196123719215f, 0.07058823853731155f, 1.0f);
		style.Colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.0470588244497776f, 0.05490196123719215f, 0.07058823853731155f, 1.0f);
		style.Colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
		style.Colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
		style.Colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(0.09803921729326248f, 0.105882354080677f, 0.1215686276555061f, 1.0f);
		style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.2352941185235977f, 0.2156862765550613f, 0.5960784554481506f, 1.0f);
		style.Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(0.4980392158031464f, 0.5137255191802979f, 1.0f, 1.0f);
		style.Colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.4980392158031464f, 0.5137255191802979f, 1.0f, 1.0f);
		style.Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(0.4980392158031464f, 0.5137255191802979f, 1.0f, 1.0f);
		style.Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.196078434586525f, 0.1764705926179886f, 0.5450980663299561f, 0.501960813999176f);
		style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.196078434586525f, 0.1764705926179886f, 0.5450980663299561f, 0.501960813999176f);
	}

}
