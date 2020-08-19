using System;

namespace SpaceGame.Message_UI_System {
    public static class Broker {
        public static event Action<string> OnNewMessage = default;
        public static void Push(string message) {
            OnNewMessage?.Invoke(message);
        }
    }
}
