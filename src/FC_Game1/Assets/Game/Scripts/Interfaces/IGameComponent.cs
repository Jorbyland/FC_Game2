
namespace Game
{
    public interface IGameComponent
    {
        void Setup(GameContextScriptable context);
        void Init(GameState state);
    }
}
