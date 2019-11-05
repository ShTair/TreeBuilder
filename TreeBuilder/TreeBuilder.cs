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
    }
}
