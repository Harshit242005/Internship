# in this we will add a node in a sorted list in a sorted manner so the list will remain sorted
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
        
    def print_node(self, node):
        if node is None:
            return 
        else:
            while(node):
                print(node.data, end=' ')
                node = node.next    
        
    def Add_sorted(self, x):
        temp = self.head
        new_node = Node(x)
        if temp is None:
            self.Add_node(x)
        else:
            while(temp is not None and temp.data < x):
                prev = temp
                temp = temp.next
             
            prev.next = new_node
            new_node.next = temp
            return 

l = Linked_list()
arr = [2, 5, 7, 10, 17]
for x in range(len(arr)):
    l.Add_node(arr[x])
l.print_node(l.head)
print()
l.Add_sorted(6)
l.print_node(l.head)