using CodeStage.AntiCheat.ObscuredTypes;

namespace JNNJMods.CrabCheat.Translators;

public static class TranslationsExtensions
{
	public static OnlinePlayerMovement GetOnlinePlayerMovement(this PlayerManager manager)
	{
		return manager.field_Private_MonoBehaviourPublicObVeSiVeRiSiAnVeanTrUnique_0;
	}

	public static void ClearMessage(this ChatBox chat)
	{
		chat.inputField.text = "";
	}

	public static float GetSwimSpeed(this PlayerMovement movement)
	{
		return movement.field_Private_Single_12;
	}

	public static void SetSwimSpeed(this PlayerMovement movement, float swimSpeed)
	{
		movement.field_Private_Single_12 = swimSpeed;
	}

	public static float GetJumpForce(this PlayerMovement movement)
	{
		return movement.field_Private_Single_3;
	}

	public static void SetJumpForce(this PlayerMovement movement, float jumpForce)
	{
		movement.field_Private_Single_3 = jumpForce;
	}

	public static float GetMoveSpeed(this PlayerMovement movement)
	{
		return movement.field_Private_ObscuredFloat_0.hiddenValue;
	}

	public static void SetMoveSpeed(this PlayerMovement movement, float moveSpeed)
	{
		movement.field_Private_ObscuredFloat_0 = new ObscuredFloat(moveSpeed);
	}

	public static float GetMaxRunSpeed(this PlayerMovement movement)
	{
		return movement.field_Private_ObscuredFloat_5.hiddenValue;
	}

	public static void SetMaxRunSpeed(this PlayerMovement movement, float maxRunSpeed)
	{
		movement.field_Private_ObscuredFloat_5 = new ObscuredFloat(maxRunSpeed);
	}

	public static float GetMaxSpeed(this PlayerMovement movement)
	{
		return movement.field_Private_ObscuredFloat_6.hiddenValue;
	}

	public static void SetMaxSpeed(this PlayerMovement movement, float maxSpeed)
	{
		movement.field_Private_ObscuredFloat_6 = new ObscuredFloat(maxSpeed);
	}

	public static float GetMaxSlopeAngle(this PlayerMovement movement)
	{
		return movement.field_Private_Single_24;
	}

	public static void SetMaxSlopeAngle(this PlayerMovement movement, float maxSlopeAngle)
	{
		movement.field_Private_Single_24 = maxSlopeAngle;
	}

	public static float GetSlowDownSpeed(this PlayerMovement movement)
	{
		return movement.field_Private_Single_23;
	}

	public static void SetSlowDownSpeed(this PlayerMovement movement, float slowDownSpeed)
	{
		movement.field_Private_Single_23 = slowDownSpeed;
	}
}
