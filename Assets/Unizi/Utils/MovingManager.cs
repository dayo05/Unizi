using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Unizi.Utils.Attributes;

namespace Unizi.Utils
{
    public static class UIUtil
    {
        private static GameObject canvas;

        public static void Initialize(GameObject canvas)
        {
            UIUtil.canvas = canvas;
        }

        public static void Initialize()
        {
            Initialize(GameObject.Find("Canvas"));
        }
        
        public static void SetButtonColor(this GameObject g, Func<ColorBlock, ColorBlock> transaction)
            => g.GetComponent<Button>().colors = transaction(g.GetComponent<Button>().colors);

        public static T Transaction<T>(this T c, Action<T> transaction) where T : Component
        {
            transaction(c);
            return c;
        }

        public static GameObject Transaction(this GameObject g, Action<GameObject> transaction)
        {
            transaction(g);
            return g;
        } 

        public static void SetObjectSize(this GameObject g, float width, float height)
            => g.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

        public static void SetObjectPos(this GameObject g, float x, float y, float z = 0)
            => g.GetComponent<RectTransform>().localPosition = new Vector3(x, y, z);

        public static void SetObjectDefaultPos(this GameObject g)
            => g.GetComponent<RectTransform>().localPosition = Vector3.zero;

        public static void AssignRectTransform(this GameObject g)
            => g.AddComponent<RectTransform>();

        public static void ButtonTransaction(this GameObject g, Action<Button> action)
            => action(g.GetComponent<Button>());

        public static GameObject CreateObject(string name)
        {
            var g = new GameObject(name);
            g.transform.SetParent(canvas.transform);
            return g;
        }

        public static GameObject CreateObject(string name, GameObject parent)
        {
            var g = new GameObject(name);
            g.transform.SetParent(parent.transform);
            return g;
        }

        public static Image CreateImage(string name = "image")
            => CreateImage(canvas, name);

        public static Image CreateImage(GameObject parent, string name = "image")
        {
            var g = CreateObject(name);
            return g.AddComponent<Image>();
        }

        public static Button CreateButton(string name, UnityAction onClick)
            => CreateButton(name, canvas, onClick);

        public static Button CreateButton(string name, GameObject parent, UnityAction onClick)
        {
            var g = CreateObject(name, parent);
            var btn = g.AddComponent<Button>();
            btn.image = g.AddComponent<Image>();
            btn.onClick.AddListener(onClick);
            return btn;
        }

        public static Text CreateText(string name, string text, Color? color = null, TextAnchor? alignment = null)
            => CreateText(name, text, canvas, color, alignment);

        public static Text CreateText(string name, string text, GameObject parent, Color? color = null, TextAnchor? alignment = null)
        {
            var g = CreateObject(name, parent);
            var t = g.AddComponent<Text>();
            t.text = text;
            t.font = Resources.Load("d2") as Font;
            t.color = color ?? Color.white;
            t.alignment = alignment ?? TextAnchor.MiddleCenter;
            return t;
        }

        public static InputField CreateInputField(string name, string defaultText = "", bool withDescription = false)
            => CreateInputField(name, canvas, defaultText, withDescription);

        public static InputField CreateInputField(string name, GameObject parent, string defaultText = "", bool withDescription = false)
        {
            var g = CreateObject(name, parent);
            g.AssignRectTransform();
            if (withDescription)
            {
                CreateText(name, name, g, alignment: TextAnchor.MiddleCenter)
                    .Transaction(text => text.fontSize = 19).gameObject.Transaction(x =>
                    {
                        x.SetObjectSize(100, 42);
                        x.SetObjectDefaultPos();
                    });
            }

            var inputFieldObject = CreateObject("Input field", g);
            var ipf = inputFieldObject.AddComponent<InputField>();
            ipf.text = defaultText;
            ipf.image = inputFieldObject.AddComponent<Image>();
            inputFieldObject.SetObjectSize(200, 40);
            if(withDescription)
                inputFieldObject.SetObjectPos(150, 0);
            else inputFieldObject.SetObjectDefaultPos();

            var inputFieldTextObj = CreateText("Input field text", "", inputFieldObject).gameObject;
            inputFieldTextObj.SetObjectDefaultPos();
            inputFieldTextObj.SetObjectSize(196, 36);

            ipf.textComponent = inputFieldTextObj.GetComponent<Text>();
            ipf.textComponent.supportRichText = false;
            return ipf;
        }
    }
    
    public abstract class MovingManager: MonoBehaviour
    {
        protected readonly List<IMovingAlgorithm> MovingObjects = new ();

        [InitializeOnce]
        protected MovingManager()
        {
            
        }

        [DestroyInitializationState]
        private void OnDestroy() { } //Manage on attribute

        private void Start()
        {
            Initialize();
        }

        protected abstract void Initialize();

        protected bool IsMoving => MovingObjects.Count != 0;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MovePrevious();
                return;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveNext();
                return;
            }
            RedirectUpdate();
            if (MovingObjects.Count != 0)
                for (var i = 0; i < MovingObjects.Count; i++)
                {
                    MovingObjects[i].Move();
                    if (MovingObjects[i].Check())
                        MovingObjects.RemoveAt(i);
                    imv = true;
                }
            else if (imv) FirstProcessAfterMoving();
            else ProcessUpdate();
        }

        private bool imv = false;

        protected virtual void RedirectUpdate() { }

        protected virtual void FirstProcessAfterMoving()
        {
            imv = false;
            ProcessUpdate();
        }

        protected virtual void ProcessUpdate() { }
        
        protected virtual void MoveNext() { }

        protected virtual void MovePrevious() { }
    }
}