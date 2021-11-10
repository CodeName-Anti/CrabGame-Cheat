using CodeStage.AntiCheat.ObscuredTypes;

namespace JNNJMods.CrabGameCheat.Translators
{
    public static class TranslationsExtensions
    {
        #region Packets
        public static ulong ReadUlong(this BDMEEOHLNLM packet, bool moveCursor) => packet.ALIAAEIBADI(moveCursor);
        public static bool ReadBool(this BDMEEOHLNLM packet, bool moveCursor) => packet.IFJOCHBPJAK(moveCursor);
        public static void Reset(this BDMEEOHLNLM packet) => packet.CGHIBHOEOAJ();
        #endregion

        public static OnlinePlayerMovement GetOnlinePlayerMovement(this PlayerManager manager) => manager.KMCIOAANFIP;
        public static void BreakGlass(this GlassBreak glass) => glass.HKMGMNICNIJ();

        public static void ClearMessage(this Chatbox chat) => chat.DJDIJJKCCMB();

        #region PlayerMovement

        public static float GetSwimSpeed(this PlayerMovement movement) => movement.AIFHCHMENDJ;
        public static void SetSwimSpeed(this PlayerMovement movement, float swimSpeed) => movement.AIFHCHMENDJ = swimSpeed;

        public static float GetJumpForce(this PlayerMovement movement) => movement.COEPGCLOCJI;
        public static void SetJumpForce(this PlayerMovement movement, float jumpForce) => movement.COEPGCLOCJI = jumpForce;

        public static float GetMoveSpeed(this PlayerMovement movement) => movement.KMDAOPAKFHH.hiddenValue;
        public static void SetMoveSpeed(this PlayerMovement movement, float moveSpeed) => movement.KMDAOPAKFHH = new ObscuredFloat(moveSpeed);

        public static float GetMaxRunSpeed(this PlayerMovement movement) => movement.JCMODIFCEDL.hiddenValue;
        public static void SetMaxRunSpeed(this PlayerMovement movement, float maxRunSpeed) => movement.JCMODIFCEDL = new ObscuredFloat(maxRunSpeed);

        public static float GetMaxSpeed(this PlayerMovement movement) => movement.DJPDEFOLLIE.hiddenValue;
        public static void SetMaxSpeed(this PlayerMovement movement, float maxSpeed) => movement.DJPDEFOLLIE = new ObscuredFloat(maxSpeed);

        public static float GetMaxSlopeAngle(this PlayerMovement movement) => movement.FHJLLDBEICA;
        public static void SetMaxSlopeAngle(this PlayerMovement movement, float maxSlopeAngle) => movement.FHJLLDBEICA = maxSlopeAngle;

        public static float GetSlowDownSpeed(this PlayerMovement movement) => movement.IHKGCEJCDNC;
        public static void SetSlowDownSpeed(this PlayerMovement movement, float slowDownSpeed) => movement.IHKGCEJCDNC = slowDownSpeed;

        #endregion
    }
}
