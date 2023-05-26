class Node:
    def __init__(self, data):
        self.data = data
        self.next = self.prev = None


class linked_list:
    def __init__(self):
        self.head = self.tail = self.second_tail = None

    def add_node(self, x):
        new_node = Node(x)
        if self.head is None:
            self.head = self.tail = new_node
            new_node.prev = None
            return 
        else:
            self.tail.next = new_node
            new_node.next = None
            new_node.prev = self.tail
            self.second_tail = self.tail
            self.tail = new_node
            return 


    def print_nodes(self):
        temp = self.head
        if temp is None:
            return 
        else:
            while(temp):
                print(temp.data, end=" ")
                temp = temp.next

    def delete_last(self):
        if self.head is None:
            return
        elif self.head == self.tail:
        # Special case: Only one node in the list
            self.head = self.tail = self.second_tail = None
        else:
            self.second_tail.next = None
            self.tail = self.second_tail


l = linked_list()
for x in range(1, 11):
    l.add_node(x)

l.print_nodes()
l.delete_last() 
print()    
l.print_nodes()                       