namespace FCTools.UIView
{
    public interface IUIWindow
    {
        string Id { get; }
        void Open();
        void Close();
        bool IsOpen { get; }
    }
}