using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LntrUtilities
{
    public class DebugConsoleUI
    {
        private TextMeshProUGUI[] _logsOnConsole;
        private RectTransform _contentRectTransform;
        private RectTransform _logRectTransform;

        private GameObject _visibleConsole;
        private RectTransform _buttonRectTransform;
        private Button _switchConsoleButton;
        private bool _isConsoleActive = true;

        public DebugConsoleUI(RectTransform contentRectTransform, RectTransform logTextRectTransform, RectTransform buttonRectTransform, GameObject visibleConsole, Button switchConsoleButton)
        {
            _contentRectTransform = contentRectTransform;
            _logRectTransform = logTextRectTransform;

            _buttonRectTransform = buttonRectTransform;
            _visibleConsole = visibleConsole;
            _switchConsoleButton = switchConsoleButton;

            _switchConsoleButton.onClick.AddListener(HandleSwitchConsoleButtonClicked);

            ArrangeLogs();
        }

        private void HandleSwitchConsoleButtonClicked()
        {
            SwitchConsoleState();
        }

        private void SwitchConsoleState()
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

        private void ArrangeLogs()
        {
            int maxVisibleLogsNumber = Mathf.Abs((int)(_contentRectTransform.rect.height / _logRectTransform.rect.height));
            _logsOnConsole = new TextMeshProUGUI[maxVisibleLogsNumber];

            float logHeight = _logRectTransform.rect.height;
            for (int i = 0; i < maxVisibleLogsNumber; i++)
            {
                RectTransform instantiatedText = GameObject.Instantiate(_logRectTransform, _contentRectTransform);
                instantiatedText.transform.position += Vector3.up * logHeight * i;
                _logsOnConsole[i] = instantiatedText.GetComponent<TextMeshProUGUI>();
            }
        }

        ~DebugConsoleUI()
        {
            _switchConsoleButton.onClick.RemoveListener(HandleSwitchConsoleButtonClicked);
        }

        public void UpdateConsoleLogs<T>(T text)
        {
            for (int i = _logsOnConsole.Length - 1; i > 0; i--)
            {
                _logsOnConsole[i].text = _logsOnConsole[i - 1].text;
            }

            _logsOnConsole[0].text = $"[{DateTime.Now.ToString("HH:mm:ss")}] {text.ToString()}";
        }
    }
}