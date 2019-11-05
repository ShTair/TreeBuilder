using System;
using System.Collections.Generic;

namespace ShComp.Construction.Tree
{
    public static class TreeBuilder
    {
        /// <summary>
        /// 入れ子集合モデルにより表現された木構造を再構築します。
        /// <para>nodesはLeft順にソートしてある必要があります。</para>
        /// </summary>
        /// <param name="nodes">Left順にソートされたすべての要素</param>
        public static IList<TNode> Rebuild<TItem, TNode>(IEnumerable<TItem> items, Func<TItem, TNode> nodeCreator)
            where TItem : ITreeItem
            where TNode : ITreeNode<TNode>
        {
            var roots = new List<TNode>();
            var enumerator = items.GetEnumerator();
            enumerator.MoveNext();
            while (enumerator.Current != null)
            {
                roots.Add(RebuildSubTree(enumerator, nodeCreator));
            }

            return roots;
        }

        private static TNode RebuildSubTree<TItem, TNode>(IEnumerator<TItem> items, Func<TItem, TNode> nodeCreator)
            where TItem : ITreeItem
            where TNode : ITreeNode<TNode>
        {
            var parentItem = items.Current;
            var parentNode = nodeCreator(parentItem);

            items.MoveNext();
            while (items.Current?.Left < parentItem.Right)
            {
                var childNode = RebuildSubTree(items, nodeCreator);
                parentNode.Children.Add(childNode);
                childNode.Parent = parentNode;
            }

            return parentNode;
        }

        /// <summary>
        /// 木構造を入れ子集合モデルで表現した場合のLeftとRightを更新します。
        /// </summary>
        public static void Update<TId, TItem, TNode>(Dictionary<TId, TItem> items, IEnumerable<TNode> roots, Func<TNode, TItem> itemCreator)
            where TItem : ITreeItem<TId>
            where TNode : ITreeNode<TId, TNode>
        {
            int i = 0;
            foreach (var root in roots)
            {
                UpdateSubTree(items, root, itemCreator, ref i);
            }
        }

        private static void UpdateSubTree<TId, TItem, TNode>(Dictionary<TId, TItem> items, TNode node, Func<TNode, TItem> itemCreator, ref int i)
            where TItem : ITreeItem<TId>
            where TNode : ITreeNode<TId, TNode>
        {
            var item = GetOrCreate(items, node.Id, () => itemCreator(node));
            item.Left = ++i;

            foreach (var child in node.Children)
            {
                UpdateSubTree(items, child, itemCreator, ref i);
            }

            item.Right = ++i;
        }

        private static TValue GetOrCreate<TKey, TValue>(Dictionary<TKey, TValue> dic, TKey key, Func<TValue> creator)
        {
            TValue value;
            if (!dic.TryGetValue(key, out value))
            {
                value = creator();
                dic.Add(key, value);
            }

            return value;
        }
    }
}
