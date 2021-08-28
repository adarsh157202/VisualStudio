#include "BinarySearchTree.h"
#include<iostream>
using namespace std;
class BinarySearchTree
{
public:
	BinarySearchTree(int data);
	BinarySearchTree* Insert(BinarySearchTree*,int);

private:
	int data;
	BinarySearchTree* left, * right;
};

BinarySearchTree::BinarySearchTree(int x)
{
	data = x;
	left = NULL;
	right = NULL;
}
BinarySearchTree* BinarySearchTree::Insert(BinarySearchTree* root, int data)
{

}