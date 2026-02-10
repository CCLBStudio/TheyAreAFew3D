using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCLBStudio.Inputs
{
    [CreateAssetMenu(fileName = "NewInputReader", menuName = "CCLBStudio/Inputs/InputReader")]
    public class InputReader : ScriptableObject, PlayerControls.IPlayerActions
    {
        [SerializeField] private bool autoInit = true;
        [SerializeField] private PlayerId playerId = PlayerId.Player1;

        [Header("Editor Settings")] 
        [SerializeField] private bool hardResetOnPlaymodeEnter = true;
    
        public event UnityAction<Vector2> MoveEvent;
        public event UnityAction<Vector2> AimEvent;
        public event UnityAction<bool> AttackEvent;
        public event UnityAction OnStratagemInputBegin;
        public event UnityAction OnStratagemInputCancelled;
        public event UnityAction OnStratagemInputUp;
        public event UnityAction OnStratagemInputDown;
        public event UnityAction OnStratagemInputLeft;
        public event UnityAction OnStratagemInputRight;

        [NonSerialized] private PlayerControls _playerInputs;
        [NonSerialized] private InputDevice _assignedDevice;

        private static bool _globalInitPerformed = false;
        private static Dictionary<PlayerId, InputDevice> _playerDevices;
    
        private enum PlayerId {Player1 = 0, Player2 = 1, Player3 = 2, Player4 = 3}

        private void OnEnable()
        {
            HardReset();
        
            if (autoInit)
            {
                Init();
            }
            
            #if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            #endif

        }

        private void OnDisable()
        {
            HardReset();
            
            #if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            #endif
        }

        public void Init()
        {
            if (_playerInputs != null)
            {
                Debug.Log($"Input reader {name} has some player controls. Aborting init.");
                return;
            }
        
            GlobalInit();

            _assignedDevice = _playerDevices[playerId];
            _playerInputs = new PlayerControls();
            _playerInputs.Player.SetCallbacks(this);
            _playerInputs.devices = new ReadOnlyArray<InputDevice>();
        
            InputSystem.onDeviceChange += OnDeviceChange;
            CheckForPlayerDevice();

            _playerInputs.Player.Enable();
            _playerInputs.UI.Disable();
        }

        private static void GlobalInit()
        {
            if (_globalInitPerformed)
            {
                return;
            }

            _globalInitPerformed = true;
            _playerDevices = new Dictionary<PlayerId, InputDevice>(4);

            foreach (PlayerId id in Enum.GetValues(typeof(PlayerId)))
            {
                _playerDevices[id] = null;
            }

            for (int i = 0; i < Gamepad.all.Count; i++)
            {
                if (i >= 4)
                {
                    break;
                }

                PlayerId id = (PlayerId)i;
                _playerDevices[id] = Gamepad.all[i];
            }
        }

        private void Clear()
        {
            if (_playerInputs == null)
            {
                return;
            }

            DisableAll();
            InputSystem.onDeviceChange -= OnDeviceChange;
        }

        public void HardReset()
        {
            Debug.Log($"Hard reset {name}");
            Clear();
            _globalInitPerformed = false;
            _playerInputs = null;
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (device is not Gamepad gamepad)
            {
                return;
            }

            if (_assignedDevice != null && _assignedDevice != device)
            {
                return;
            }
        
            Debug.Log($"Gamepad named {gamepad.displayName} with id {gamepad.GetHashCode()} changed state to : {change}.");
        
            switch (change)
            {
                case InputDeviceChange.Added:
                    if (IsNewDevice(gamepad) && _assignedDevice == null)
                    {
                        _playerDevices[playerId] = gamepad;
                        SetDevice(gamepad);
                    }
                    break;

                case InputDeviceChange.Removed:
                    break;

                case InputDeviceChange.Disconnected:
                    break;

                case InputDeviceChange.Reconnected:
                    if (_assignedDevice == gamepad)
                    {
                        SetDevice(gamepad);
                    }
                    break;
            
                case InputDeviceChange.Enabled:
                    break;
            
                case InputDeviceChange.Disabled:
                    break;
            
                case InputDeviceChange.UsageChanged:
                    break;
            
                case InputDeviceChange.ConfigurationChanged:
                    break;
            
                case InputDeviceChange.SoftReset:
                    break;
            
                case InputDeviceChange.HardReset:
                    break;
            
                default:
                    throw new ArgumentOutOfRangeException(nameof(change), change, null);
            }
        }

        private bool IsNewDevice(InputDevice device)
        {
            return _playerDevices.Values.All(playerDevice => device != playerDevice);
        }

        private void CheckForPlayerDevice()
        {
            if (_playerDevices[playerId] == null)
            {
                Debug.LogWarning($"There is no gamepad available for {playerId}");
                return;
            }
        
            SetDevice(_playerDevices[playerId]);
        }
    
        public void SetDevice(InputDevice device)
        {
            if (device == null)
            {
                return;
            }
        
            _playerInputs.devices = new[] {device};
            _assignedDevice = device;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                MoveEvent?.Invoke(context.ReadValue<Vector2>());
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                MoveEvent?.Invoke(Vector2.zero);
            }
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                AimEvent?.Invoke(context.ReadValue<Vector2>());
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                AimEvent?.Invoke(Vector2.zero);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                AttackEvent?.Invoke(true);
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                AttackEvent?.Invoke(false);
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {

        }

        public void OnStratagem(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnStratagemInputBegin?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                OnStratagemInputCancelled?.Invoke();
            }
        }

        public void OnStratagemUp(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnStratagemInputUp?.Invoke();
            }
        }

        public void OnStratagemDown(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnStratagemInputDown?.Invoke();
            }
        }

        public void OnStratagemLeft(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnStratagemInputLeft?.Invoke();
            }
        }

        public void OnStratagemRight(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnStratagemInputRight?.Invoke();
            }
        }

        public void EnablePlayerInputs()
        {
            _playerInputs.Player.Enable();
        }
    
        public void DisablePlayerInputs()
        {
            _playerInputs.Player.Disable();
        }
    
        public void EnableUiInputs()
        {
            _playerInputs.UI.Enable();
        }

        public void DisableUiInputs()
        {
            _playerInputs.UI.Disable();
        }

        public void DisableAll()
        {
            DisablePlayerInputs();
            DisableUiInputs();
        }

        #region Editor

#if UNITY_EDITOR
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    if(hardResetOnPlaymodeEnter)
                    {
                        HardReset();
                    }
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    if (autoInit)
                    {
                        Init();
                    }
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    ResetAllUnityActions();
                    break;
                case PlayModeStateChange.EnteredEditMode:
                    break;
            }
        }
        
        private void ResetAllUnityActions()
        {
            var fields = GetType().GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic
            );
            
            foreach (var field in fields)
            {
                if (typeof(Delegate).IsAssignableFrom(field.FieldType))
                {
                    field.SetValue(this, null);
                    //Debug.Log("Reset field " + field.Name);
                }
            }
        }
#endif
        #endregion
    }
}
