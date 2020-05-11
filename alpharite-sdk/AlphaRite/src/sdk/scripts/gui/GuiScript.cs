using BloodGUI;
using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AlphaRite.sdk.scripts.gui {
    public class GuiScript: AlphaCycle
    {
        private GameObject _panel = null;
        private GameObject _canvas = null;
        private GameObject _eventSystem = null;
        private bool _change = false;
        
        public GuiScript(AlphariteSdk sdk) : base(sdk) {
        }

        protected override void onStart()
        {
            Alpharite.println("GUI: Starting...");
            BuildUI(AlphaRiteLoader.AlphaRiteInstance);
        }

        protected override void onStop()
        {
            Alpharite.println("GUI: Stopping...");
        }

        protected override void onRenderingUpdate()
        {
            base.onRenderingUpdate();
            if (Input.GetKey(KeyCode.Tab) && Input.GetKey(KeyCode.C) && !_change)
            {
                _panel.SetActive(!_panel.activeSelf);
                _change = true;
            } else if ((!Input.GetKey(KeyCode.Tab) || !Input.GetKey(KeyCode.C)) && _change)
                _change = false;
        }
        
        private void BuildUI(GameObject parent)
        {
            Alpharite.println("GUI: Building...");
            
            //Base
            _eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            
            _canvas = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = _canvas.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            _canvas.transform.SetParent(parent.transform, false);
            
            _eventSystem.transform.SetParent(parent.transform, false);
            
            _panel = CreatePanel(_canvas);
            var obj = CreateToggle("Fog state", _panel);
            var toggle = obj.GetComponent<Toggle>();
            toggle.isOn = false;
            toggle.onValueChanged.AddListener((bool value) =>
            {
                Alpharite.println("GUI: Fog state - " + value);
                
                if (value)
                    sdk.disableCycle("wallhack");
                else
                    sdk.enableCycle("wallhack");
            });
            
            Alpharite.println("GUI: Builded");
        }

        private GameObject CreatePanel(GameObject parent)
        {
            var panel = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(DragableUI));
            panel.transform.SetParent(parent.transform, false);
            
            var panelRectTransform = panel.GetComponent<RectTransform>();
            panelRectTransform.anchorMin = new Vector2(0, 1);
            panelRectTransform.anchorMax = new Vector2(0, 1);
            panelRectTransform.pivot = new Vector2(0, 1);
            panelRectTransform.sizeDelta = new Vector2(200, 200);

            var panelBg = panel.GetComponent<Image>();
            panelBg.color = new Color(255, 255, 255, 175); //White a bit transparent
            return panel;
        }

        private GameObject CreateToggle(string txt, GameObject panel)
        {
            var objToggle = new GameObject("Toggle", typeof(RectTransform), typeof(Toggle));
            objToggle.transform.SetParent(panel.transform, false);

            var toggleTransform = objToggle.GetComponent<RectTransform>();
            toggleTransform.anchorMin = new Vector2(0, 1);
            toggleTransform.anchorMax = new Vector2(1, 1);
            toggleTransform.pivot = new Vector2(0.5f, 1);
            toggleTransform.sizeDelta = new Vector2(0, 20);
            
            var objBackground = new GameObject("Background", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            objBackground.transform.SetParent(objToggle.transform, false);

            objBackground.GetComponent<Image>().color = Color.gray;

            var fogBackgroundTransform = objBackground.GetComponent<RectTransform>();
            fogBackgroundTransform.anchorMin = new Vector2(0, 1);
            fogBackgroundTransform.anchorMax = new Vector2(0, 1);
            fogBackgroundTransform.pivot = new Vector2(0, 1);
            fogBackgroundTransform.sizeDelta = new Vector2(20, 20);
            
            var objFogCheck = new GameObject("Check", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            objFogCheck.transform.SetParent(objBackground.transform, false);
            
            var checkTransform = objFogCheck.GetComponent<RectTransform>();
            checkTransform.anchorMin = new Vector2(0, 1);
            checkTransform.anchorMax = new Vector2(0, 1);
            checkTransform.pivot = new Vector2(0, 1);
            checkTransform.sizeDelta = new Vector2(14, 14);
            checkTransform.anchoredPosition = new Vector2(3, -3);

            objToggle.GetComponent<Toggle>().graphic = objFogCheck.GetComponent<Image>();
            
            var objTxt = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            objTxt.transform.SetParent(objToggle.transform, false);

            var lbl = objTxt.GetComponent<Text>();
            lbl.text = txt;
            lbl.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
            lbl.color = Color.black;
            
            var lblTransform = objTxt.GetComponent<RectTransform>();
            lblTransform.anchorMin = new Vector2(0, 1);
            lblTransform.anchorMax = new Vector2(0, 1);
            lblTransform.pivot = new Vector2(0, 1);
            lblTransform.sizeDelta = new Vector2(100, 20);
            lblTransform.anchoredPosition = new Vector2(23, -2);
            
            return objToggle;
        }
    }
}