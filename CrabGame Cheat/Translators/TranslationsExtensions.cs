using CodeStage.AntiCheat.ObscuredTypes;

namespace JNNJMods.CrabGameCheat.Translators
{
    public static class TranslationsExtensions
    {
        public static MonoBehaviourPublicObVeSiVeRiSiAnVeanTrUnique GetOnlinePlayerMovement(this MonoBehaviourPublicCSstReshTrheObplBojuUnique manager) => manager.field_Private_MonoBehaviourPublicObVeSiVeRiSiAnVeanTrUnique_0;

        public static void ClearMessage(this MonoBehaviourPublicRaovTMinTemeColoonCoUnique chat) => chat.inputField.text = "";

        #region PlayerMovement

        public static float GetSwimSpeed(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement) => movement.field_Private_Single_12;
        public static void SetSwimSpeed(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement, float swimSpeed) => movement.field_Private_Single_12 = swimSpeed;

        public static float GetJumpForce(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement) => movement.field_Private_Single_3;
        public static void SetJumpForce(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement, float jumpForce) => movement.field_Private_Single_3 = jumpForce;

        public static float GetMoveSpeed(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement) => movement.field_Private_ObscuredFloat_0.hiddenValue;
        public static void SetMoveSpeed(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement, float moveSpeed) => movement.field_Private_ObscuredFloat_0 = new ObscuredFloat(moveSpeed);

        public static float GetMaxRunSpeed(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement) => movement.field_Private_ObscuredFloat_5.hiddenValue;
        public static void SetMaxRunSpeed(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement, float maxRunSpeed) => movement.field_Private_ObscuredFloat_5 = new ObscuredFloat(maxRunSpeed);

        public static float GetMaxSpeed(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement) => movement.field_Private_ObscuredFloat_6.hiddenValue;
        public static void SetMaxSpeed(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement, float maxSpeed) => movement.field_Private_ObscuredFloat_6 = new ObscuredFloat(maxSpeed);

        public static float GetMaxSlopeAngle(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement) => movement.field_Private_Single_24;
        public static void SetMaxSlopeAngle(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement, float maxSlopeAngle) => movement.field_Private_Single_24 = maxSlopeAngle;

        public static float GetSlowDownSpeed(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement) => movement.field_Private_Single_23;
        public static void SetSlowDownSpeed(this MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique movement, float slowDownSpeed) => movement.field_Private_Single_23 = slowDownSpeed;

        #endregion
    }
}
