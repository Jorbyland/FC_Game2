using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ShoppingCart
    { private Dictionary<ItemScriptable, int> m_entries = new Dictionary<ItemScriptable, int>();

    public event System.Action OnCartChanged;

    public void Add(ItemScriptable a_item, int a_qty = 1)
    {
        if (!m_entries.ContainsKey(a_item)) m_entries[a_item] = 0;
        m_entries[a_item] += a_qty;
        OnCartChanged?.Invoke();
    }

    public void SetQuantity(ItemScriptable a_item, int a_qty)
    {
        if (a_qty <= 0) m_entries.Remove(a_item);
        else m_entries[a_item] = a_qty;
        OnCartChanged?.Invoke();
    }

    public void Remove(ItemScriptable a_item)
    {
        if (m_entries.ContainsKey(a_item)) m_entries.Remove(a_item);
        OnCartChanged?.Invoke();
    }

    public int GetQuantity(ItemScriptable a_item) =>
        m_entries.TryGetValue(a_item, out var a_qty) ? a_qty : 0;

    public IReadOnlyDictionary<ItemScriptable, int> Items => m_entries;

    public void Clear()
    {
        m_entries.Clear();
        OnCartChanged?.Invoke();
    }
    }
}
