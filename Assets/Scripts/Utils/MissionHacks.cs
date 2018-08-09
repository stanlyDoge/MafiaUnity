﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MafiaUnity
{
    /* 
        This file consists of hacks used to modify game's scene depending on the mission loaded.
    */
    public class MissionHacks
    {
        public MissionHacks(string missionName, MafiaFormats.Scene2BINLoader data)
        {
            // Fix backdrop sector
            var backdrop = GameObject.Find("Backdrop sector");
            {
                if (backdrop != null)
                {
                    backdrop.AddComponent<BackdropManipulator>();
                }
            }

            // Change view distance
            {
                var mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

                var viewDistance = data.viewDistance;

                mainCamera.farClipPlane = viewDistance;
            }

            switch (missionName)
            {
                // example usage
                case "tutorial":
                {
                    var skybox = GameObject.Find("o_m_");

                    if (skybox != null)
                    {
                        SetUpSkybox(skybox.transform.Find("Box02"));
                        SetUpSkybox(skybox.transform.Find("Box03"));

                        var slunko = skybox.transform.Find("slunko");
                        slunko.gameObject.SetActive(false);
                    }

                    var light22 = GameObject.Find("sector Box12/light22")?.GetComponent<Light>();
                    if (light22 != null) light22.shadows = LightShadows.Soft;

                    var light10 = GameObject.Find("sector Box12/light10")?.GetComponent<Light>();
                    if (light10 != null) light10.shadows = LightShadows.Soft;
                }
                break;

                case "mise06-autodrom":
                {
                    var box01 = GameObject.Find("oblohamirrored/Box01");
                    var box02 = GameObject.Find("denjasno/Box02");

                    if (box01 != null)
                        SetUpSkybox(box01.transform);

                    if (box02 != null)
                        SetUpSkybox(box02.transform);
                }
                break;

                case "mise20-galery":
                {
                    var obloha = GameObject.Find("obloha");
                    var obloha01 = GameObject.Find("obloha01");

                    if (obloha != null)
                    {
                        SetUpSkybox(obloha.transform);
                        obloha.transform.parent = backdrop.transform;

                        obloha.transform.localScale = new Vector3(2, 2, 2);
                    }

                    if (obloha01 != null)
                    {
                        obloha01.SetActive(false);
                    }
                }
                break;

                case "freeride":
                {
                    var box02 = GameObject.Find("zapad/Box02");

                    SetUpSkybox(box02.transform);
                }
                break;

                case "freekrajina":
                {
                    var box01 = GameObject.Find("denjasno00/Box01");
                    var box02 = GameObject.Find("denjasno00/Box02");

                    SetUpSkybox(box01.transform);
                    SetUpSkybox(box02.transform);
                }
                break;
            }
        }

        void SetUpSkybox(Transform skybox)
        {
            skybox.gameObject.layer = LayerMask.NameToLayer("Backdrop");

            var meshRenderer = skybox.GetComponent<MeshRenderer>();

            foreach (var mat in meshRenderer.sharedMaterials)
            {
                mat.shader = Shader.Find("Unlit/Texture");
            }
        }
    }

    public class BackdropManipulator : MonoBehaviour
    {
        Transform mainCamera = null;
        Transform skyboxCamera = null;

        private void Start()
        {
            mainCamera = GameObject.Find("Main Camera").transform;
            skyboxCamera = new GameObject("Backdrop camera").transform;

            skyboxCamera.parent = mainCamera;

            skyboxCamera.localPosition = Vector3.zero;
            skyboxCamera.localRotation = Quaternion.identity;
            skyboxCamera.localScale = Vector3.one;

            var cam = skyboxCamera.gameObject.AddComponent<Camera>();
            cam.farClipPlane = 5000f;
            cam.cullingMask = (1 << LayerMask.NameToLayer("Backdrop"));
            cam.depth = 1 + mainCamera.GetComponent<Camera>().depth;
            cam.clearFlags = CameraClearFlags.Nothing;

            gameObject.layer = LayerMask.NameToLayer("Backdrop");
        }

        private void Update()
        {
            if (mainCamera == null)
                return;

            transform.position = mainCamera.position;
        }

        private void OnDestroy()
        {
            GameObject.Destroy(skyboxCamera);
        }
    }
}