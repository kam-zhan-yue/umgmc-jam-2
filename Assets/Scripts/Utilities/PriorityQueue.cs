//using System;

//public class BinarySearchTree<T, V> where T : IComparable<T>
//{
//    public TreeNode<T, V> Root { get; private set; }

//    public BinarySearchTree()
//    {
//        Root = null;
//    }

//    public void Insert(T value, V item)
//    {
//        Root = Insert(Root, value, item);
//    }

//    private TreeNode<T, V> Insert(TreeNode<T, V> node, T value, V item)
//    {
//        if (node == null)
//        {
//            node = new TreeNode<T, V>(value, item);
//        }
//        else if (value.CompareTo(node.Value) < 0)
//        {
//            node.Left = Insert(node.Left, value, item);
//        }
//        else if (value.CompareTo(node.Value) > 0)
//        {
//            node.Right = Insert(node.Right, value, item);
//        }
//        return node;
//    }

//    public bool Contains(T value)
//    {
//        return Contains(Root, value);
//    }

//    private bool Contains(TreeNode<T> node, T value)
//    {
//        if (node == null)
//        {
//            return false;
//        }

//        int compare = value.CompareTo(node.Value);
//        if (compare < 0)
//        {
//            return Contains(node.Left, value);
//        }
//        else if (compare > 0)
//        {
//            return Contains(node.Right, value);
//        }
//        else
//        {
//            return true;
//        }
//    }


//    public void Delete(T value)
//    {
//        Root = Delete(Root, value);
//    }

//    private TreeNode<T> Delete(TreeNode<T> node, T value)
//    {
//        if (node == null) return node;

//        int compare = value.CompareTo(node.Value);
//        if (compare < 0)
//        {
//            node.Left = Delete(node.Left, value);
//        }
//        else if (compare > 0)
//        {
//            node.Right = Delete(node.Right, value);
//        }
//        else
//        {
//            // Node with only one child or no child
//            if (node.Left == null)
//                return node.Right;
//            else if (node.Right == null)
//                return node.Left;

//            // Node with two children: Get the inorder successor (smallest in the right subtree)
//            node.Value = MinValue(node.Right);

//            // Delete the inorder successor
//            node.Right = Delete(node.Right, node.Value);
//        }

//        return node;
//    }

//    private T MinValue(TreeNode<T> node)
//    {
//        T minValue = node.Value;
//        while (node.Left != null)
//        {
//            minValue = node.Left.Value;
//            node = node.Left;
//        }
//        return minValue;
//    }

//    public void InOrderTraversal(Action<T> visit)
//    {
//        InOrderTraversal(Root, visit);
//    }

//    private void InOrderTraversal(TreeNode<T> node, Action<T> visit)
//    {
//        if (node != null)
//        {
//            // Traverse the left subtree
//            InOrderTraversal(node.Left, visit);

//            // Visit the node
//            visit(node.Value);

//            // Traverse the right subtree
//            InOrderTraversal(node.Right, visit);
//        }
//    }

//}

//public class TreeNode<T, V> where T : IComparable<T>
//{
//    public T Value { get; set; }
//    public V Item { get; set; }
//    public TreeNode<T, V> Left { get; set; }
//    public TreeNode<T, V> Right { get; set; }

//    public TreeNode(T value, V item)
//    {
//        Value = value;
//        Item = item;
//        Left = null;
//        Right = null;
//    }
//}
