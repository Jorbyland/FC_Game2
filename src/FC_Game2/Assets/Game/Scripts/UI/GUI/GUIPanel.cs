using UnityEngine;

namespace Game
{
    public abstract class GUIPanel : MonoBehaviour
    {
        public abstract void Bind(GameContextScriptable context, GameState state);
        public abstract void Refresh();
    }
}
