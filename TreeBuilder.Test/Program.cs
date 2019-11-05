using System;
using System.Collections.Generic;
using System.Linq;

namespace ShComp.Construction.Tree.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var nodes = new List<Node>();
            nodes.Add(new Node { Name = "a", Left = 1, Right = 2 });
            nodes.Add(new Node { Name = "b", Left = 3, Right = 14 });
            nodes.Add(new Node { Name = "c", Left = 4, Right = 5 });
            nodes.Add(new Node { Name = "d", Left = 6, Right = 9 });
            nodes.Add(new Node { Name = "e", Left = 7, Right = 8 });
            nodes.Add(new Node { Name = "f", Left = 10, Right = 13 });
            nodes.Add(new Node { Name = "g", Left = 11, Right = 12 });
            nodes.Add(new Node { Name = "h", Left = 15, Right = 16 });

            Console.WriteLine("構造化のテスト");
            var roots = TreeBuilder.Rebuild(nodes);
            foreach (var root in roots)
            {
                WriteTree(root, "");
            }

            Console.WriteLine();
            Console.WriteLine("更新のテスト");
            // テストのため、一旦すべてのLeftとRightを初期化する
            foreach (var node in nodes)
            {
                node.Left = 0;
                node.Right = 0;
            }

            TreeBuilder.Update(roots);
            foreach (var root in roots)
            {
                WriteTree(root, "");
            }

            // 構造を変更してみる
            Console.WriteLine();
            Console.WriteLine("ノードの移動テスト");
            var dic = nodes.ToDictionary(t => t.Name);
            while (true)
            {
                Console.WriteLine("移動させたいノードの名前を入力してください。");
                Node target;
                if (!dic.TryGetValue(Console.ReadLine(), out target))
                {
                    Console.WriteLine("指定した名前のノードは存在しませんでした。");
                    continue;
                }

                Console.WriteLine("移動先のノードの名前を入力してください。");
                Node parent;
                if (!dic.TryGetValue(Console.ReadLine(), out parent))
                {
                    Console.WriteLine("指定した名前のノードは存在しませんでした。");
                    continue;
                }

                if (target.Left < parent.Left && parent.Right < target.Right)
                {
                    Console.WriteLine("移動先が移動元の子孫のため、移動できません。");
                    continue;
                }

                if (target.Parent == null)
                {
                    roots.Remove(target);
                }
                else
                {
                    target.Parent.Children.Remove(target);
                }

                parent.Children.Add(target);
                target.Parent = parent;

                TreeBuilder.Update(roots);
                foreach (var root in roots)
                {
                    WriteTree(root, "");
                }

                Console.WriteLine();
            }
        }

        private static void WriteTree(Node node, string d)
        {
            Console.WriteLine($"{d}{node}");

            foreach (var child in node.Children)
            {
                WriteTree(child, d + " ");
            }
        }
    }

    class Node : ITreeNode<Node>
    {
        public string Name { get; set; }

        public int Left { get; set; }

        public int Right { get; set; }

        public Node Parent { get; set; }

        private IList<Node> _children;

        public IList<Node> Children => _children ?? (_children = new List<Node>());

        public override string ToString()
        {
            return $"{Name} L:{Left} R:{Right} P:{Parent?.Name}";
        }
    }
}
