using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LntrUtilities
{
    public class DebugConsole : MonoBehaviour
    {
        [Header("UI (Logs)")]
        [SerializeField] private RectTransform _contentRectTransform;
        [SerializeField] private RectTransform _logRectTransform;

        [Header("UI (Button)")]
        [SerializeField] private GameObject _visibleConsole;
        [SerializeField] private RectTransform _buttonRectTransform;
        [SerializeField] private Button _switchConsoleButton;

        private static DebugConsole _instance;

        private DebugConsoleUI _debugConsoleUI;

        public static DebugConsole Instance
        {
            get
            {
                return _instance;
            }
            private set
            {
                _instance ??= value;
            }
        }

        public void Log<T>(T text)
        {
            _debugConsoleUI.UpdateConsoleLogs(text);
        }

        private DebugConsole()
        {
        }

        private void Awake()
        {
            Instance = this;

            _debugConsoleUI = new DebugConsoleUI(_contentRectTransform, _logRectTransform, _buttonRectTransform, _visibleConsole, _switchConsoleButton);
        }
    }
}
