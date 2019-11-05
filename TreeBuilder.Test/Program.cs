using System;
using System.Collections.Generic;

namespace ShComp.Construction.Tree.Test
{
    class Program
    {
        // Itemはデータベースに記録されることを想定していて、
        // 記録しないもの（ParentやChildren）をItemには持たないようにしています。
        static void Main(string[] args)
        {
            var items = new List<Item>();
            items.Add(new Item { Name = "a", Left = 1, Right = 2 });
            items.Add(new Item { Name = "b", Left = 3, Right = 14 });
            items.Add(new Item { Name = "c", Left = 4, Right = 5 });
            items.Add(new Item { Name = "d", Left = 6, Right = 9 });
            items.Add(new Item { Name = "e", Left = 7, Right = 8 });
            items.Add(new Item { Name = "f", Left = 10, Right = 13 });
            items.Add(new Item { Name = "g", Left = 11, Right = 12 });
            items.Add(new Item { Name = "h", Left = 15, Right = 16 });

            var nodeDic = new Dictionary<string, Node>();

            Console.WriteLine("構造化のテスト");
            var roots = TreeBuilder.Rebuild(items, item =>
            {
                var node = new Node { Item = item };
                nodeDic.Add(item.Name, node);
                return node;
            });

            foreach (var root in roots)
            {
                WriteTree(root, "");
            }

            Console.WriteLine();
            Console.WriteLine("更新のテスト");
            // テストのため、一旦すべてのLeftとRightを初期化する
            foreach (var item in items)
            {
                item.Left = 0;
                item.Right = 0;
            }

            TreeBuilder.Update(roots, node => node.Item);
            foreach (var root in roots)
            {
                WriteTree(root, "");
            }

            // 構造を変更してみる
            Console.WriteLine();
            Console.WriteLine("ノードの移動テスト");
            while (true)
            {
                Console.WriteLine("移動させたいノードの名前を入力してください。");
                Node target;
                if (!nodeDic.TryGetValue(Console.ReadLine(), out target))
                {
                    Console.WriteLine("指定した名前のノードは存在しませんでした。");
                    continue;
                }

                Console.WriteLine("移動先のノードの名前を入力してください。");
                Node parent;
                if (!nodeDic.TryGetValue(Console.ReadLine(), out parent))
                {
                    Console.WriteLine("指定した名前のノードは存在しませんでした。");
                    continue;
                }

                if (target.Item.Left < parent.Item.Left && parent.Item.Right < target.Item.Right)
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

                TreeBuilder.Update(roots, node => node.Item);
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

    class Item : ITreeItem
    {
        public string Name { get; set; }

        public int Left { get; set; }

        public int Right { get; set; }
    }

    class Node : ITreeNode<Node>
    {
        public Node Parent { get; set; }

        private IList<Node> _children;

        public IList<Node> Children => _children ?? (_children = new List<Node>());

        public Item Item { get; set; }

        public override string ToString()
        {
            return $"Name: {Item.Name}, Left: {Item.Left}, Right: {Item.Right}, Parent: {Parent?.Item.Name}";
        }
    }
}
