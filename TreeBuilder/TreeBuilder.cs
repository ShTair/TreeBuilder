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
        public static IList<T> Rebuild<T>(IEnumerable<T> nodes) where T : ITreeNode<T>
        {
            var roots = new List<T>();
            var enumerator = nodes.GetEnumerator();
            enumerator.MoveNext();
            while (enumerator.Current != null)
            {
                roots.Add(RebuildSubTree(enumerator));
            }

            return roots;
        }

        private static T RebuildSubTree<T>(IEnumerator<T> nodes) where T : ITreeNode<T>
        {
            var parent = nodes.Current;
            nodes.MoveNext();
            while (nodes.Current?.Left < parent.Right)
            {
                var child = RebuildSubTree(nodes);
                parent.Children.Add(child);
                child.Parent = parent;
            }

            return parent;
        }

        /// <summary>
        /// 木構造を入れ子集合モデルで表現した場合のLeftとRightを更新します。
        /// </summary>
        public static void Update<T>(IEnumerable<T> roots) where T : ITreeNode<T>
        {
            int i = 0;
            foreach (var root in roots)
            {
                UpdateSubTree(root, ref i);
            }
        }

        private static void UpdateSubTree<T>(T node, ref int i) where T : ITreeNode<T>
        {
            node.Left = ++i;

            foreach (var child in node.Children)
            {
                UpdateSubTree(child, ref i);
            }

            node.Right = ++i;
        }
    }
}
