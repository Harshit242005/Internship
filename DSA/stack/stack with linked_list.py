# implementing a stack with the help of linked list
# stack follow LIFO
class Node:
    def __init__(self, data):
        self.data = data
        self.next = None
        self.prev = None

class Stack:
    def __init__(self):
        self.head = None
        self.tail = None

    # adding and deketion of the node should be on the constant time complexity
    # i would not be able to solve how to delete the node at O(1)
    def push(self, x):
        new_node = Node(x)
        if self.head is None:
            self.head = self.tail = new_node
            return 
        else:
            self.tail.next = new_node
            new_node.next = None
            self.tail = new_node
            return 
        
    def pop(self):
        if self.head is None:
            return 
        if self.head.next == None:
            self.head = self.tail = None
            return 
        else:
            temp = self.head
            while(temp != self.tail):
                prev_node = temp
                temp = temp.next
            prev_node.next = None
            self.tail = prev_node
            return    
            
        
    def print_nodes(self):
        if self.head is None:
            print("stack is empty")
            return
        else:
            temp = self.head
            while(temp != None):
                print(temp.data, end=" ")
                temp = temp.next    

if __name__ == "__main__":
    s = Stack()
    s.push(1)
    s.push(2)
    s.push(3)
    s.push(4)
    s.print_nodes()
    print()
    s.pop()
    s.print_nodes()