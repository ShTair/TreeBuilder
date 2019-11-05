using System.Collections.Generic;

namespace ShComp.Construction.Tree
{
    public interface ITreeNode<T> where T : ITreeNode<T>
    {
        int Left { get; set; }

        int Right { get; set; }

        T Parent { set; }

        IList<T> Children { get; }
    }
}
