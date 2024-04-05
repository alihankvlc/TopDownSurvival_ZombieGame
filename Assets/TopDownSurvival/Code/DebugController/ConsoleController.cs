using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DeadNation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Serialization;

    public sealed class ConsoleController : Singleton<ConsoleController>
    {
        #region Variables

        [SerializeField] private KeyCode _toggleEnableKey;

        [SerializeField] private Image _debugImage;
        [SerializeField] private Sprite _debugErrorIcon;

        private bool _enableConsoleWindow;
        private bool _enableDebugMode;

        private string _debugText = "";
        private string _inputText = "";

        private const string _fileName = "game_log";
        private string _filePath = Path.Combine(Application.dataPath, _fileName);

        private Vector2 _scrollPosition;

        private Command _activeDebugging;

        private Command _restartGame;

        //private Command<int> _giveHealth;
        private Command<int> _spawnZombie;
        private List<CommandBase> _commands;

        private Rect _windowRect;

        public bool IsOpen => _enableConsoleWindow;

        #endregion

        #region Fonksiyonlar...

        private void Awake()
        {
            _activeDebugging = new Command("/debug", "", () => { _enableDebugMode = !_enableDebugMode; });
            _restartGame = new Command("/restart", "Restart the game.", () =>
            {
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentSceneIndex);
            });

            /*_giveHealth = new Command<int>("/give_health", "The player has been given health",
                (int value) => { StatManager.Instance.Stat<Health>().Modify += value; });*/ //Pasiff


            _spawnZombie = new Command<int>("/zombie_add", "Zombie spawn...", (int count) =>
            {
                if (count > 20)
                {
                    Debug.Log("Cannot spawn more than 20 zombies.");
                    return;
                }

                SpawnManager.Instance.SpawnZombie(count, 0.5f, 0.5f);
            });
            _commands = new List<CommandBase>
            {
                _activeDebugging,
                _restartGame,
                _spawnZombie
                //  _giveHealth
            };
        }

        private void OnEnable() =>
            Application.logMessageReceived += HandleLog;

        private void Start()
        {
            CreateLogFile(_debugText);

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            _windowRect = new Rect(0, 0, screenWidth * 0.8f, screenHeight * 0.8f);
            _windowRect.center = new Vector2(screenWidth / 2, screenHeight / 2);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_toggleEnableKey))
                _enableConsoleWindow = !_enableConsoleWindow;

            _debugImage.gameObject.SetActive(_enableDebugMode);
        }

        #region GUI

        private void OnGUI()
        {
            if (!_enableConsoleWindow)
            {
                _inputText = "";
                return;
            }

            if (_enableDebugMode)
            {
                int fps = Mathf.RoundToInt(1f / Time.deltaTime);
                GUI.Label(new Rect(10, 10, 100, 20), "FPS: " + fps);
            }

            _windowRect = GUILayout.Window(0, _windowRect, DebugWindow, "Console");
        }

        private void DebugWindow(int windowID)
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            GUILayout.Label(_debugText);
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            _inputText = GUILayout.TextField(_inputText, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Send", GUILayout.Width(80)) || Event.current.keyCode == KeyCode.Return)
            {
                if (!string.IsNullOrEmpty(_inputText))
                {
                    AddDebugMessage(_inputText);
                    _inputText = "";
                }
            }
            else if (GUILayout.Button("Close", GUILayout.Width(80), GUILayout.Height(20)))
                _enableConsoleWindow = false;

            GUILayout.EndHorizontal();
        }

        #endregion

        #region Komut Gönderme...

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            string log = type switch
            {
                LogType.Warning => $"<color=yellow>{logString}</color>",
                LogType.Error => $"<color=red>{logString}</color>",
                LogType.Log => $"<color=cyan>{logString}</color>",
                _ => $"<color=red>{logString}</color>"
            };

            string info = $"[SYSTEM: {DateTime.Now}] {log}";

            CreateLogFile(_debugText);
            AddDebugMessage(type == LogType.Log ? log : info, false);

            if (type == LogType.Error)
            {
                _debugImage.sprite = _debugErrorIcon;
            }
        }

        private void AddDebugMessage(string message, bool lessThan = true)
        {
            string[] properties = message.Split(' ');
            string debugMessage = lessThan ? "> " + message + "\n" : message + "\n";
            _debugText += debugMessage;

            CommandBase commandBase = _commands.FirstOrDefault(r => r.Id == properties[0]);

            if ((commandBase == null ||
                 (!_enableDebugMode) && properties[0].StartsWith("/") && commandBase.Id != "/debug"))
            {
                if (message.StartsWith("/"))
                    Debug.LogWarning($"Command {message} not found or you do not have permission to use it.");

                return;
            }

            if (commandBase is Command<int> argumentCommand)
            {
                if (properties.Length > 1 && int.TryParse(properties[1], out int intValue))
                    argumentCommand.Use(intValue);
                else
                    Debug.LogWarning("Invalid or missing argument for command: " + properties[0]);
            }
            else if (commandBase is Command command)
                command.Use();
        }

        #endregion

        #region Log Oluşturma...

        private void CreateLogFile(string content)
        {
            try
            {
                if (CheckFileExists())
                {
                    File.WriteAllText(_filePath, string.Empty);
                }

                File.AppendAllText(_filePath, content);
            }
            catch (Exception e)
            {
                Debug.LogError("An error occurred while creating the log file. " + e.ToString());
                throw;
            }
        }

        private bool CheckFileExists()
        {
            string filePath = Path.Combine(Application.dataPath, "game_log");
            return File.Exists(_filePath);
        }

        #endregion

        private void OnDisable() =>
            Application.logMessageReceived -= HandleLog;

        #endregion
    }
}