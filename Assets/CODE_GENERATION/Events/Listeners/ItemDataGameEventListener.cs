using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "ItemData")]
	public sealed class ItemDataGameEventListener : BaseGameEventListener<ItemData, ItemDataGameEvent, ItemDataUnityEvent>
	{
	}
}