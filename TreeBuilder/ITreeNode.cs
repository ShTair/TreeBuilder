using System.Collections.Generic;

namespace ShComp.Construction.Tree
{
    public interface ITreeNode<T> where T : ITreeNode<T>
    {
        T Parent { set; }

        IList<T> Children { get; }
    }

    public interface ITreeNode<TId, TNode> : ITreeNode<TNode> where TNode : ITreeNode<TId, TNode>
    {
        TId Id { get; }
    }
}
