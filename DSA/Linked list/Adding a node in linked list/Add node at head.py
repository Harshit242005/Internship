# adding a node at the head of a linked list
class Node:
    def __init__(self, data):
        self.data = data
        self.next = self.prev = None

class Linked_list:
    def __init__(self):
        self.head = self.tail = None

    def Add_node(self, x):
        new_node = Node(x)
        if self.head is None:
            self.head = self.tail = new_node
            new_node.prev = None
            return 
        else:
            self.tail.next = new_node
            new_node.prev = self.tail
            self.tail = new_node
            return 
        
    def Add_head(self, x):
        new_node = Node(x)
        if self.head is None:
            self.head = self.tail = new_node
            new_node.prev = None
        else:
            self.head.prev = new_node
            new_node.next = self.head
            self.head = new_node
            return         

    def print_node(self):
        if self.head is None:
            return 
        else:
            temp = self.head
            while(temp):
                print(temp.data, end=' ')
                temp = temp.next               

l = Linked_list()
arr = [1, 2, 8, 9, 4]
for x in range(len(arr)):
    l.Add_node(arr[x])

l.print_node()

l.Add_head(100)
print()
l.print_node()