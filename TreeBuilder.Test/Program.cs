using System;
using System.Collections.Generic;

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

            var roots = TreeBuilder.Rebuild(nodes);
            foreach (var root in roots)
            {
                WriteTree(root, "");
            }
        }

        static void WriteTree(Node node, string d)
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
