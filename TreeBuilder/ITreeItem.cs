namespace ShComp.Construction.Tree
{
    public interface ITreeItem
    {
        int Left { get; set; }

        int Right { get; set; }
    }

    public interface ITreeItem<TId> : ITreeItem
    {
        TId Id { get; }
    }
}
