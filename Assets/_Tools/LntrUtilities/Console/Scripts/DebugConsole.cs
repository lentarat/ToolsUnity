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

            _debugConsoleUI = new DebugConsoleUI(_contentRectTransform, _logRectTransform, _buttonRectTransform, _visibleConsole);
            _debugConsoleUI.ArrangeLogs();

            _switchConsoleButton.onClick.AddListener(HandleSwitchConsoleButtonClicked);
        }

        private void HandleSwitchConsoleButtonClicked()
        {
            _debugConsoleUI.SwitchConsoleState();
        }

        private class DebugConsoleUI
        {
            private TextMeshProUGUI[] _logsOnConsole;
            private RectTransform _contentRectTransform;
            private RectTransform _logRectTransform;
          
            private GameObject _visibleConsole;
            private RectTransform _buttonRectTransform;
            private bool _isConsoleActive = true;

            public DebugConsoleUI(RectTransform contentRectTransform, RectTransform logTextRectTransform, RectTransform buttonRectTransform, GameObject visibleConsole)
            {
                _contentRectTransform = contentRectTransform;
                _logRectTransform = logTextRectTransform;

                _buttonRectTransform = buttonRectTransform;
                _visibleConsole = visibleConsole;
            }

            public void ArrangeLogs()
            {
                int maxVisibleLogsNumber = Mathf.Abs((int)(_contentRectTransform.rect.height / _logRectTransform.rect.height));
                _logsOnConsole = new TextMeshProUGUI[maxVisibleLogsNumber];

                float logHeight = _logRectTransform.rect.height;
                for (int i = 0; i < maxVisibleLogsNumber; i++)
                {
                    RectTransform instantiatedText = Instantiate(_logRectTransform, _contentRectTransform);
                    instantiatedText.transform.position += Vector3.up * logHeight * i;
                    _logsOnConsole[i] = instantiatedText.GetComponent<TextMeshProUGUI>();
                }
            }

            public void UpdateConsoleLogs<T>(T text)
            {
                for (int i = _logsOnConsole.Length - 1; i > 0; i--)
                {
                    _logsOnConsole[i].text = _logsOnConsole[i - 1].text;
                }

                _logsOnConsole[0].text = $"[{DateTime.Now.ToString("HH:mm:ss")}] {text.ToString()}";
            }

            public void SwitchConsoleState()
            {
                _isConsoleActive = !_isConsoleActive;

                if (_isConsoleActive)
                {
                    _buttonRectTransform.anchorMin = Vector2.one;
                    _buttonRectTransform.anchorMax = Vector2.one;
                    _buttonRectTransform.pivot = Vector2.one;
                }
                else
                {
                    _buttonRectTransform.anchorMin = Vector2.zero;
                    _buttonRectTransform.anchorMax = Vector2.zero;
                    _buttonRectTransform.pivot = Vector2.zero;
                }

                _visibleConsole.SetActive(_isConsoleActive);
            }
        }
    }
}
