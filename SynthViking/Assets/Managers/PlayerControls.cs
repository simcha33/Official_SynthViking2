// GENERATED AUTOMATICALLY FROM 'Assets/Managers/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""0cec0917-3af0-4dc9-bc0e-1f9616b8f5d9"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""d915495a-0ca2-408a-876c-31ae6ed8e652"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MeleeAttack"",
                    ""type"": ""Value"",
                    ""id"": ""dd3c16a4-979a-47da-9845-721aa487f436"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LightAttack"",
                    ""type"": ""Value"",
                    ""id"": ""e6c771d0-2088-4453-ba92-e331c5220dab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Value"",
                    ""id"": ""eea10429-3cda-4b3f-be6b-7dc501f6c136"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Value"",
                    ""id"": ""7c137dd7-b773-4bc1-82fe-ffac923d5078"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SlowMo"",
                    ""type"": ""Value"",
                    ""id"": ""f1360145-004f-49c9-89c9-f528194b951c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""37bf77a6-9641-40b0-a15b-4849bf79fc59"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Value"",
                    ""id"": ""4b0fe72c-7bf1-4b72-bc94-56328b17d089"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""d396cf9a-2c6f-413a-bf38-f28ec2051b6b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Value"",
                    ""id"": ""c6ef2c3b-bfee-43d2-a2cb-40c846ccbc39"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AirSmash"",
                    ""type"": ""Value"",
                    ""id"": ""f5b59fd6-e1ac-435a-ab69-0c5fe17e008d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UltimateMove "",
                    ""type"": ""Value"",
                    ""id"": ""9a69e5bc-21dc-4562-80eb-49e19927866e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RestartScene"",
                    ""type"": ""Value"",
                    ""id"": ""97f037c8-2d74-4be0-9033-5f2e0d6b2bb7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PauseGame"",
                    ""type"": ""Value"",
                    ""id"": ""23d0fb2a-f219-4f0d-9736-4b0b2986f05c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dpad-Up"",
                    ""type"": ""Value"",
                    ""id"": ""8908142f-9eed-4990-921f-72f3083c26e6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dpad-Down"",
                    ""type"": ""Value"",
                    ""id"": ""fff86f4d-e508-40b5-ab9f-b576684a8fa6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dpad-Left"",
                    ""type"": ""Value"",
                    ""id"": ""bf89defa-9f7c-4cb0-99ae-fa21378c48f4"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dpad-Right"",
                    ""type"": ""Value"",
                    ""id"": ""c56c602b-f2f8-4c05-985b-6f51c292686a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3237be5e-9164-4535-9ec4-6b306741aadc"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b76bcb10-ee3e-4742-bc91-b05093fcc9eb"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ea3fc9b-c15d-4feb-8acf-926477d30fe1"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""0a2f98cf-b7e7-4e64-bda9-c2373146bf51"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""cb6e213f-c9b5-4557-8a2b-590e0dcae458"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""618cb307-2077-4e1d-aab5-fb24c6f65418"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6a790684-18e8-4524-9767-2136284174df"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7c64b0c5-19d8-4eab-a691-bf8fec851e83"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1b3ca883-eb23-45e5-940b-a4923e16e93b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""030b5e1a-8c88-49a8-8ee0-727699314d14"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45166b6d-f50f-489e-9319-65faf8d28f33"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82fa9320-7e84-4d25-b4e4-4f9fbced1360"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87981530-d7e3-4eed-a210-2d79798da777"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""423a0cc0-aa2f-4552-aa3b-86671a687f92"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1fc14f5-e998-43d7-b3b1-d6f8d64c43e7"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56932e8a-b348-484b-aa59-ab2ff86c571c"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""RestartScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33382662-b752-4e20-b4e4-ce6489c2f123"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""RestartScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""61304500-94f3-4404-a840-2ff113756f4b"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MeleeAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c9993c4-36a3-4463-8c76-87cd9a4f8436"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""UltimateMove "",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bc8e0d1-c282-407a-bf06-0dcd34cf17ec"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AirSmash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3f8b999-d23a-46dc-af67-88361e1e8fe7"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SlowMo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb6d9f80-1619-4490-ad19-5771feb45e75"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SlowMo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""819cd111-ef89-47a4-b585-c7926cee5571"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a1902c2-6601-4c0c-bd61-3ab1d24b6471"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dpad-Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b0339dd-a122-402d-a8e7-620de3c10eea"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dpad-Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc66f281-483e-4264-bd1b-cfd11906a385"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dpad-Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e7b6419-e4b1-4d7c-acad-e924053adbf8"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dpad-Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a0fd1c8b-485c-4be7-bea6-e63c4c9119e6"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Test controller"",
            ""id"": ""1d29491a-7358-48f8-8440-ce9d35fceb42"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""be2b6f88-4790-47d8-a7ff-73a95bf3a54a"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveUp"",
                    ""type"": ""Button"",
                    ""id"": ""406eb4ca-6562-4002-a9e1-da150aef590a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveDown"",
                    ""type"": ""Button"",
                    ""id"": ""2a1c9151-6625-4e53-9ddb-7c31d210cf51"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e0ec3009-abec-4d06-85a8-375ce48a0479"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33717822-357d-42d4-b41c-cb768701225c"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99fe8336-d821-4a5b-ad12-1e8a1c72eaed"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": []
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_MeleeAttack = m_Gameplay.FindAction("MeleeAttack", throwIfNotFound: true);
        m_Gameplay_LightAttack = m_Gameplay.FindAction("LightAttack", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Sprint = m_Gameplay.FindAction("Sprint", throwIfNotFound: true);
        m_Gameplay_SlowMo = m_Gameplay.FindAction("SlowMo", throwIfNotFound: true);
        m_Gameplay_Look = m_Gameplay.FindAction("Look", throwIfNotFound: true);
        m_Gameplay_Block = m_Gameplay.FindAction("Block", throwIfNotFound: true);
        m_Gameplay_Aim = m_Gameplay.FindAction("Aim", throwIfNotFound: true);
        m_Gameplay_Dash = m_Gameplay.FindAction("Dash", throwIfNotFound: true);
        m_Gameplay_AirSmash = m_Gameplay.FindAction("AirSmash", throwIfNotFound: true);
        m_Gameplay_UltimateMove = m_Gameplay.FindAction("UltimateMove ", throwIfNotFound: true);
        m_Gameplay_RestartScene = m_Gameplay.FindAction("RestartScene", throwIfNotFound: true);
        m_Gameplay_PauseGame = m_Gameplay.FindAction("PauseGame", throwIfNotFound: true);
        m_Gameplay_DpadUp = m_Gameplay.FindAction("Dpad-Up", throwIfNotFound: true);
        m_Gameplay_DpadDown = m_Gameplay.FindAction("Dpad-Down", throwIfNotFound: true);
        m_Gameplay_DpadLeft = m_Gameplay.FindAction("Dpad-Left", throwIfNotFound: true);
        m_Gameplay_DpadRight = m_Gameplay.FindAction("Dpad-Right", throwIfNotFound: true);
        // Test controller
        m_Testcontroller = asset.FindActionMap("Test controller", throwIfNotFound: true);
        m_Testcontroller_Move = m_Testcontroller.FindAction("Move", throwIfNotFound: true);
        m_Testcontroller_MoveUp = m_Testcontroller.FindAction("MoveUp", throwIfNotFound: true);
        m_Testcontroller_MoveDown = m_Testcontroller.FindAction("MoveDown", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_MeleeAttack;
    private readonly InputAction m_Gameplay_LightAttack;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_Sprint;
    private readonly InputAction m_Gameplay_SlowMo;
    private readonly InputAction m_Gameplay_Look;
    private readonly InputAction m_Gameplay_Block;
    private readonly InputAction m_Gameplay_Aim;
    private readonly InputAction m_Gameplay_Dash;
    private readonly InputAction m_Gameplay_AirSmash;
    private readonly InputAction m_Gameplay_UltimateMove;
    private readonly InputAction m_Gameplay_RestartScene;
    private readonly InputAction m_Gameplay_PauseGame;
    private readonly InputAction m_Gameplay_DpadUp;
    private readonly InputAction m_Gameplay_DpadDown;
    private readonly InputAction m_Gameplay_DpadLeft;
    private readonly InputAction m_Gameplay_DpadRight;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @MeleeAttack => m_Wrapper.m_Gameplay_MeleeAttack;
        public InputAction @LightAttack => m_Wrapper.m_Gameplay_LightAttack;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Sprint => m_Wrapper.m_Gameplay_Sprint;
        public InputAction @SlowMo => m_Wrapper.m_Gameplay_SlowMo;
        public InputAction @Look => m_Wrapper.m_Gameplay_Look;
        public InputAction @Block => m_Wrapper.m_Gameplay_Block;
        public InputAction @Aim => m_Wrapper.m_Gameplay_Aim;
        public InputAction @Dash => m_Wrapper.m_Gameplay_Dash;
        public InputAction @AirSmash => m_Wrapper.m_Gameplay_AirSmash;
        public InputAction @UltimateMove => m_Wrapper.m_Gameplay_UltimateMove;
        public InputAction @RestartScene => m_Wrapper.m_Gameplay_RestartScene;
        public InputAction @PauseGame => m_Wrapper.m_Gameplay_PauseGame;
        public InputAction @DpadUp => m_Wrapper.m_Gameplay_DpadUp;
        public InputAction @DpadDown => m_Wrapper.m_Gameplay_DpadDown;
        public InputAction @DpadLeft => m_Wrapper.m_Gameplay_DpadLeft;
        public InputAction @DpadRight => m_Wrapper.m_Gameplay_DpadRight;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @MeleeAttack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMeleeAttack;
                @MeleeAttack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMeleeAttack;
                @MeleeAttack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMeleeAttack;
                @LightAttack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLightAttack;
                @LightAttack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLightAttack;
                @LightAttack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLightAttack;
                @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Sprint.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @SlowMo.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSlowMo;
                @SlowMo.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSlowMo;
                @SlowMo.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSlowMo;
                @Look.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                @Block.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlock;
                @Block.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlock;
                @Block.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlock;
                @Aim.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAim;
                @Dash.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @AirSmash.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAirSmash;
                @AirSmash.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAirSmash;
                @AirSmash.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAirSmash;
                @UltimateMove.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUltimateMove;
                @UltimateMove.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUltimateMove;
                @UltimateMove.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUltimateMove;
                @RestartScene.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRestartScene;
                @RestartScene.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRestartScene;
                @RestartScene.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRestartScene;
                @PauseGame.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPauseGame;
                @PauseGame.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPauseGame;
                @PauseGame.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPauseGame;
                @DpadUp.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDpadUp;
                @DpadUp.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDpadUp;
                @DpadUp.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDpadUp;
                @DpadDown.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDpadDown;
                @DpadDown.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDpadDown;
                @DpadDown.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDpadDown;
                @DpadLeft.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDpadLeft;
                @DpadLeft.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDpadLeft;
                @DpadLeft.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDpadLeft;
                @DpadRight.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDpadRight;
                @DpadRight.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDpadRight;
                @DpadRight.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDpadRight;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @MeleeAttack.started += instance.OnMeleeAttack;
                @MeleeAttack.performed += instance.OnMeleeAttack;
                @MeleeAttack.canceled += instance.OnMeleeAttack;
                @LightAttack.started += instance.OnLightAttack;
                @LightAttack.performed += instance.OnLightAttack;
                @LightAttack.canceled += instance.OnLightAttack;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @SlowMo.started += instance.OnSlowMo;
                @SlowMo.performed += instance.OnSlowMo;
                @SlowMo.canceled += instance.OnSlowMo;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Block.started += instance.OnBlock;
                @Block.performed += instance.OnBlock;
                @Block.canceled += instance.OnBlock;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @AirSmash.started += instance.OnAirSmash;
                @AirSmash.performed += instance.OnAirSmash;
                @AirSmash.canceled += instance.OnAirSmash;
                @UltimateMove.started += instance.OnUltimateMove;
                @UltimateMove.performed += instance.OnUltimateMove;
                @UltimateMove.canceled += instance.OnUltimateMove;
                @RestartScene.started += instance.OnRestartScene;
                @RestartScene.performed += instance.OnRestartScene;
                @RestartScene.canceled += instance.OnRestartScene;
                @PauseGame.started += instance.OnPauseGame;
                @PauseGame.performed += instance.OnPauseGame;
                @PauseGame.canceled += instance.OnPauseGame;
                @DpadUp.started += instance.OnDpadUp;
                @DpadUp.performed += instance.OnDpadUp;
                @DpadUp.canceled += instance.OnDpadUp;
                @DpadDown.started += instance.OnDpadDown;
                @DpadDown.performed += instance.OnDpadDown;
                @DpadDown.canceled += instance.OnDpadDown;
                @DpadLeft.started += instance.OnDpadLeft;
                @DpadLeft.performed += instance.OnDpadLeft;
                @DpadLeft.canceled += instance.OnDpadLeft;
                @DpadRight.started += instance.OnDpadRight;
                @DpadRight.performed += instance.OnDpadRight;
                @DpadRight.canceled += instance.OnDpadRight;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Test controller
    private readonly InputActionMap m_Testcontroller;
    private ITestcontrollerActions m_TestcontrollerActionsCallbackInterface;
    private readonly InputAction m_Testcontroller_Move;
    private readonly InputAction m_Testcontroller_MoveUp;
    private readonly InputAction m_Testcontroller_MoveDown;
    public struct TestcontrollerActions
    {
        private @PlayerControls m_Wrapper;
        public TestcontrollerActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Testcontroller_Move;
        public InputAction @MoveUp => m_Wrapper.m_Testcontroller_MoveUp;
        public InputAction @MoveDown => m_Wrapper.m_Testcontroller_MoveDown;
        public InputActionMap Get() { return m_Wrapper.m_Testcontroller; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestcontrollerActions set) { return set.Get(); }
        public void SetCallbacks(ITestcontrollerActions instance)
        {
            if (m_Wrapper.m_TestcontrollerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_TestcontrollerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_TestcontrollerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_TestcontrollerActionsCallbackInterface.OnMove;
                @MoveUp.started -= m_Wrapper.m_TestcontrollerActionsCallbackInterface.OnMoveUp;
                @MoveUp.performed -= m_Wrapper.m_TestcontrollerActionsCallbackInterface.OnMoveUp;
                @MoveUp.canceled -= m_Wrapper.m_TestcontrollerActionsCallbackInterface.OnMoveUp;
                @MoveDown.started -= m_Wrapper.m_TestcontrollerActionsCallbackInterface.OnMoveDown;
                @MoveDown.performed -= m_Wrapper.m_TestcontrollerActionsCallbackInterface.OnMoveDown;
                @MoveDown.canceled -= m_Wrapper.m_TestcontrollerActionsCallbackInterface.OnMoveDown;
            }
            m_Wrapper.m_TestcontrollerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @MoveUp.started += instance.OnMoveUp;
                @MoveUp.performed += instance.OnMoveUp;
                @MoveUp.canceled += instance.OnMoveUp;
                @MoveDown.started += instance.OnMoveDown;
                @MoveDown.performed += instance.OnMoveDown;
                @MoveDown.canceled += instance.OnMoveDown;
            }
        }
    }
    public TestcontrollerActions @Testcontroller => new TestcontrollerActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnMeleeAttack(InputAction.CallbackContext context);
        void OnLightAttack(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnSlowMo(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnBlock(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnAirSmash(InputAction.CallbackContext context);
        void OnUltimateMove(InputAction.CallbackContext context);
        void OnRestartScene(InputAction.CallbackContext context);
        void OnPauseGame(InputAction.CallbackContext context);
        void OnDpadUp(InputAction.CallbackContext context);
        void OnDpadDown(InputAction.CallbackContext context);
        void OnDpadLeft(InputAction.CallbackContext context);
        void OnDpadRight(InputAction.CallbackContext context);
    }
    public interface ITestcontrollerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnMoveUp(InputAction.CallbackContext context);
        void OnMoveDown(InputAction.CallbackContext context);
    }
}
