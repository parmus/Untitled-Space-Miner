// GENERATED AUTOMATICALLY FROM 'Assets/_Project/Systems/PlayerInput/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace SpaceGame.PlayerInput
{
    public class @Controls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Controls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Flight"",
            ""id"": ""1539b986-1ff2-4122-9600-4f9bd2558765"",
            ""actions"": [
                {
                    ""name"": ""Look Around"",
                    ""type"": ""Value"",
                    ""id"": ""4f0d3357-467f-4385-b224-757e81d0ad2a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Thrust"",
                    ""type"": ""Value"",
                    ""id"": ""0bd10a92-5bba-44d2-b878-afb71a638a4e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""9ffa8df8-86f3-47d1-8ea2-150f8c110883"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Boost"",
                    ""type"": ""Button"",
                    ""id"": ""bb75972a-cdbd-44cb-8af3-12d0ed2014df"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bab4f624-f6c5-4645-9c18-74d64bbfa15a"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Look Around"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""ADWS"",
                    ""id"": ""5ab2d94a-4483-4c34-a122-ed7e49d1cc68"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thrust"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ed6a7232-2421-4e28-ac2f-11bf086dcccc"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b31f5d55-4ca1-483d-94c4-371f2ec08c08"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f3f9a30e-59f7-41ef-a3b2-4e9e71f8bd53"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8b1c9d1a-c171-4e3d-8082-3fd7a88bf6d1"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""32d427a1-9013-4b56-9dd6-98609bc3c28a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""daa28fe6-d5eb-4f42-97a1-a9a7ff16937c"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""c052729f-abc9-4556-a874-773ecfce3d29"",
            ""actions"": [
                {
                    ""name"": ""ToggleInventory"",
                    ""type"": ""Button"",
                    ""id"": ""bb518626-1340-49ed-a412-faf0b8584552"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""adcbf6e6-7cb2-4056-9efc-b19445fdb737"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""ToggleInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Flight
            m_Flight = asset.FindActionMap("Flight", throwIfNotFound: true);
            m_Flight_LookAround = m_Flight.FindAction("Look Around", throwIfNotFound: true);
            m_Flight_Thrust = m_Flight.FindAction("Thrust", throwIfNotFound: true);
            m_Flight_Fire = m_Flight.FindAction("Fire", throwIfNotFound: true);
            m_Flight_Boost = m_Flight.FindAction("Boost", throwIfNotFound: true);
            // UI
            m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
            m_UI_ToggleInventory = m_UI.FindAction("ToggleInventory", throwIfNotFound: true);
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

        // Flight
        private readonly InputActionMap m_Flight;
        private IFlightActions m_FlightActionsCallbackInterface;
        private readonly InputAction m_Flight_LookAround;
        private readonly InputAction m_Flight_Thrust;
        private readonly InputAction m_Flight_Fire;
        private readonly InputAction m_Flight_Boost;
        public struct FlightActions
        {
            private @Controls m_Wrapper;
            public FlightActions(@Controls wrapper) { m_Wrapper = wrapper; }
            public InputAction @LookAround => m_Wrapper.m_Flight_LookAround;
            public InputAction @Thrust => m_Wrapper.m_Flight_Thrust;
            public InputAction @Fire => m_Wrapper.m_Flight_Fire;
            public InputAction @Boost => m_Wrapper.m_Flight_Boost;
            public InputActionMap Get() { return m_Wrapper.m_Flight; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(FlightActions set) { return set.Get(); }
            public void SetCallbacks(IFlightActions instance)
            {
                if (m_Wrapper.m_FlightActionsCallbackInterface != null)
                {
                    @LookAround.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnLookAround;
                    @LookAround.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnLookAround;
                    @LookAround.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnLookAround;
                    @Thrust.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnThrust;
                    @Thrust.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnThrust;
                    @Thrust.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnThrust;
                    @Fire.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnFire;
                    @Fire.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnFire;
                    @Fire.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnFire;
                    @Boost.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnBoost;
                    @Boost.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnBoost;
                    @Boost.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnBoost;
                }
                m_Wrapper.m_FlightActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @LookAround.started += instance.OnLookAround;
                    @LookAround.performed += instance.OnLookAround;
                    @LookAround.canceled += instance.OnLookAround;
                    @Thrust.started += instance.OnThrust;
                    @Thrust.performed += instance.OnThrust;
                    @Thrust.canceled += instance.OnThrust;
                    @Fire.started += instance.OnFire;
                    @Fire.performed += instance.OnFire;
                    @Fire.canceled += instance.OnFire;
                    @Boost.started += instance.OnBoost;
                    @Boost.performed += instance.OnBoost;
                    @Boost.canceled += instance.OnBoost;
                }
            }
        }
        public FlightActions @Flight => new FlightActions(this);

        // UI
        private readonly InputActionMap m_UI;
        private IUIActions m_UIActionsCallbackInterface;
        private readonly InputAction m_UI_ToggleInventory;
        public struct UIActions
        {
            private @Controls m_Wrapper;
            public UIActions(@Controls wrapper) { m_Wrapper = wrapper; }
            public InputAction @ToggleInventory => m_Wrapper.m_UI_ToggleInventory;
            public InputActionMap Get() { return m_Wrapper.m_UI; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
            public void SetCallbacks(IUIActions instance)
            {
                if (m_Wrapper.m_UIActionsCallbackInterface != null)
                {
                    @ToggleInventory.started -= m_Wrapper.m_UIActionsCallbackInterface.OnToggleInventory;
                    @ToggleInventory.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnToggleInventory;
                    @ToggleInventory.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnToggleInventory;
                }
                m_Wrapper.m_UIActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @ToggleInventory.started += instance.OnToggleInventory;
                    @ToggleInventory.performed += instance.OnToggleInventory;
                    @ToggleInventory.canceled += instance.OnToggleInventory;
                }
            }
        }
        public UIActions @UI => new UIActions(this);
        private int m_KeyboardandMouseSchemeIndex = -1;
        public InputControlScheme KeyboardandMouseScheme
        {
            get
            {
                if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
                return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
            }
        }
        public interface IFlightActions
        {
            void OnLookAround(InputAction.CallbackContext context);
            void OnThrust(InputAction.CallbackContext context);
            void OnFire(InputAction.CallbackContext context);
            void OnBoost(InputAction.CallbackContext context);
        }
        public interface IUIActions
        {
            void OnToggleInventory(InputAction.CallbackContext context);
        }
    }
}
