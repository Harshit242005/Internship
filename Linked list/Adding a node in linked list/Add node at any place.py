# in this script you can add node at any place

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

    def length(self):
        temp = self.head
        count = 0
        if temp is None:
            return 0
        else:
            while(temp):
                count += 1
                temp = temp.next
            return count                
    # add the node after the given position and if want the node at exact position where the pos value given -1 from pos
    def Add_any(self, pos, x):
        new_node = Node(x)
        if self.head is None:
            return 
        if pos < 0 or pos > self.length():
            return 
        if pos == 1:
            self.Add_head(x)
        if pos == self.length():
            self.Add_node(x)

        else:
            i = 0
            temp = self.head
            while(i < pos):
                prev = temp
                temp = temp.next
                i += 1
            prev.next = new_node
            new_node.next = temp
            return 

l = Linked_list()
arr = [4, 7, 2, 0, 10, 45, 78, 90]
for x in range(len(arr)):
    l.Add_node(arr[x])

l.print_node()    

l.Add_any(3, 100)
print()
l.print_node()