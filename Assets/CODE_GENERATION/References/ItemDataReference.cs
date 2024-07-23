using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class ItemDataReference : BaseReference<ItemData, ItemDataVariable>
	{
	    public ItemDataReference() : base() { }
	    public ItemDataReference(ItemData value) : base(value) { }
	}
}