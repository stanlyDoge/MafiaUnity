﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MafiaUnity;

public class SetupGUI : MonoBehaviour {

    public GameObject pathSelection;
    public GameObject modManager;
    public GameObject mainMenu;
    public GameObject startupLight;

    public void StartGame()
    {
        // Revert settings back to default.
        RenderSettings.ambientLight = new Color(54, 58, 66);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;

        GameObject.Destroy(startupLight);
        GameObject.Destroy(GameObject.Find("EventSystem"));
        GameObject.Destroy(gameObject);
        
        var modManager = GameManager.instance.modManager;
        var mods = GetComponent<ModManagerGUI>();

        foreach (var mod in mods.modEntries)
        {
            if (mod.status == ModEntryStatus.Active)
            {
                modManager.LoadMod(mod.modName);
            }
        }

        modManager.InitializeMods();
    }

    public void PathSelectionMenu()
    {
        mainMenu.SetActive(false);
        pathSelection.SetActive(true);
    }

    public void ModManagerMenu()
    {
        mainMenu.SetActive(false);
        modManager.SetActive(true);
    }

    // Use this for initialization
    void Start() {
        if (PlayerPrefs.HasKey("gamePath"))
        {
            if (!GameManager.instance.SetGamePath(PlayerPrefs.GetString("gamePath")))
                PathSelectionMenu();
            else
                SetupDefaultBackground();
        }
        else
            PathSelectionMenu();

    }

    bool bgWasSetup = false;

    public void SetupDefaultBackground()
    {
        if (bgWasSetup)
            return;

        if (GameManager.instance.GetInitialized())
        {
            bgWasSetup = true;

            GameManager.instance.missionManager.LoadMission("00menu");
        }
    }
}
