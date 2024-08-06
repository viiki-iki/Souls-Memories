using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class ItensEnum
    {
        [System.Flags]
        public enum Itens
        {
            None = 0,

            Book1 = 1 << 0,
            Book2 = 1 << 1,
            Book3 = 1 << 2,
            Hand1 = 1 << 3,
            Hand2 = 1 << 4,
            Other = 1 << 5,

            Everything = 0b1111
        }
    }
}