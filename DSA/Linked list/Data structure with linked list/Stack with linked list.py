class Node:
    def __init__(self, data):
        self.data = data
        self.next = self.prev = None

class Stack:
    def __init__(self):
        self.head = self.tail = None
        # for popping out the last node to follow LILO we have to define a another pointer called second_tail which will help in performing the operations on (1) time 
        

    def push(self, x):
        new_node = Node(x)
        if self.head is None:
            self.head = self.tail = self.second_tail = new_node
            new_node.prev = None
            return
        else:
            self.tail.next = new_node
            new_node.next = None
            new_node.prev = self.tail
            self.second_tail = self.tail
            self.tail = new_node
            return 
        

    def print_stack(self, node):
        if node is None:
            return 
        else:
          
            while(node != None):
                print(node.data, end=' ')
                node = node.next


    def pop(self):
        if self.head is None:
            return None
        elif self.head == self.tail:
            # Special case: Only one node in the list
            popped_value = self.head.data
            self.head = self.tail = self.second_tail = None
            return popped_value
        else:
            popped_value = self.tail.data
            self.tail = self.second_tail
            self.tail.next = None
            self.tail.prev = self.second_tail.prev
            self.second_tail = self.tail.prev
            return popped_value
        

    def get_size(self):
        if self.head is None:
            return 
        else:
            temp = self.head
            count = 0
            while(temp):
                count += 1
                temp = temp.next 
            return count      


l = Stack()
l.push(10)
l.push(20)
l.push(30)

l.print_stack(l.head)
print()
l.pop()

l.print_stack(l.head)

print()
print(f"the size of the current stack is {l.get_size()}")