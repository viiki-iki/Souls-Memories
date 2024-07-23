using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class ItemDataEvent : UnityEvent<ItemData> { }

	[CreateAssetMenu(
	    fileName = "ItemDataVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "custom/ItemData",
	    order = 120)]
	public class ItemDataVariable : BaseVariable<ItemData, ItemDataEvent>
	{
	}
}