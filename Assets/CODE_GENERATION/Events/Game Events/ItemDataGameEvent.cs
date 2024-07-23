using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "ItemDataGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "custom/ItemData",
	    order = 120)]
	public sealed class ItemDataGameEvent : GameEventBase<ItemData>
	{
	}
}