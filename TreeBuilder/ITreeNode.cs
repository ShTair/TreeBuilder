using System.Collections.Generic;

namespace ShComp.Construction.Tree
{
    public interface ITreeNode<T> where T : ITreeNode<T>
    {
        T Parent { set; }

        IList<T> Children { get; }
    }
}
