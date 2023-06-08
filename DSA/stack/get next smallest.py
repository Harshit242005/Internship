class Node:
    def __init__(self, data):
        self.data = data
        self.next = self.prev = None

class Stack:
    def __init__(self):
        self.head = self.tail = self.second_tail = None

    def push(self, x):
        new_node = Node(x)
        if self.head is None:
            self.head = self.tail = self.second_tail = new_node
            return
        else:
            self.tail.next = new_node
            new_node.prev = self.tail
            self.tail = new_node

    def pop(self):
        if self.head is None:
            return None
        else:
            last = self.tail
            self.second_tail.next = None
            self.second_tail = self.second_tail.prev
            self.tail = self.second_tail
            return last.data

    def size(self):
        if self.head is None:
            return 0
        else:
            temp = self.head
            count = 0
            while temp:
                count += 1
                temp = temp.next
            return count
        
    def get_smallest(self, start, second):
        if second is None:
            return  
        else:
            if start.data > second.data:
                print(second.data, end=" ")
                start = start.next
                second = start.next
                return self.get_smallest(start, second)
            else:
                print(-1, end=" ")
                start = start.next
                second = start.next
                return self.get_smallest(start, second)   

if __name__ == "__main__":
    stack = Stack()           
    arr = [5, 6, 2, 3, 1, 7]
    for x in range(len(arr)):
        stack.push(arr[x])
    print(stack.size())    
    start = stack.head
    second = start.next
    stack.get_smallest(start, second)