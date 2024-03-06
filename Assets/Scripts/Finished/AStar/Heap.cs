/********************************************************
 * NAME: Travis Wright									*
 * CLASS: 4373.003										*
 * ASSIGNMENT: Project 4								*
 * FILE: Heap.cs										*
 * DESCRIPTION: An optimization used to allow quick 	*
 * 		finding of the best square in pathfinding using	*
 * 		the heap data structure.						*
 * ******************************************************/

using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T> {

	//array of generic item
	T[] items;
	//count of items in array
	int itemCount;

	//heap constructor
	public Heap(int maxSize) {
		items = new T[maxSize];
	}

	//adds new item to heap and increment count, then sorts
	public void add(T item) {
		item.HeapIndex = itemCount;
		items[itemCount] = item;
		sortUp(item);
		itemCount++;
	}

	//removes the first, aka cheapest, node from the heap
	public T removeFirst() {
		T firstItem = items[0];
		itemCount--;
		items[0] = items[itemCount];
		items[0].HeapIndex = 0;
		sortDown(items[0]);
		return firstItem;
	}

	//re-sorts heap after a node is updated
	public void updateItem(T item) {
		sortUp(item);
	}

	//count
	public int count {
		get {
			return itemCount;
		}
	}

	//contains function - returns boolean if item is in the heap
	public bool contains(T item) {
		return Equals(items[item.HeapIndex], item);
	}

	//sort item downwards
	void sortDown(T item) {
		while (true) {//loop until break
			int leftChildIndex = item.HeapIndex * 2 + 1;
			int rightChildIndex = item.HeapIndex * 2 + 2;
			int swapIndex = 0;

			//if the index of the left child is still in the heap array
			if (leftChildIndex < itemCount) {
				swapIndex = leftChildIndex; //set temporary swap index

				//if index of right child has item
				if (rightChildIndex < itemCount) {
					//and the right item is less than the left item
					if (items[leftChildIndex].CompareTo(items[rightChildIndex]) < 0) {
						swapIndex = rightChildIndex; //move swap index to ensure smallest child is swapped
					}
				}

				//if child at swap index is less than the parent, swap them
				if (item.CompareTo(items[swapIndex]) < 0) {
					swap (item,items[swapIndex]);
				}
				//otherwise, return
				else {
					return;
				}
				
			}
			//otherwise, return
			else {
				return;
			}
		}
	}

	//sort item upwards
	void sortUp(T item) {
		//get parent of node
		int parentIndex = (item.HeapIndex-1)/2;

		//loop until break
		while (true) {
			//get actual parent item
			T parent = items[parentIndex];
			//if item is greater than parent, swap them
			if (item.CompareTo(parent) > 0) {
				swap (item,parent);
			}
			else {//end if else
				break;
			}
			//get new parent index
			parentIndex = (item.HeapIndex-1)/2;
		}
	}

	//swap 2 nodes within the heap
	void swap(T a, T b) {
		items[a.HeapIndex] = b;
		items[b.HeapIndex] = a;
		int aIndex = a.HeapIndex;
		a.HeapIndex = b.HeapIndex;
		b.HeapIndex = aIndex;
	}
}

//interface used for heapIndex
public interface IHeapItem<T> : IComparable<T> {
	int HeapIndex {
		get;
		set;
	}
}