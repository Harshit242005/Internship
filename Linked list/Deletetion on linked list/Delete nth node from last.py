class Node:
    def __init__(self, data):
        self.data = data
        self.next = self.prev = None

class linked_list:
    def __init__(self):
        self.head = self.tail = None
        # for popping out the last node to follow LILO we have to define a another pointer called second_tail which will help in performing the operations on (1) time 
        self.second_tail = None

    def add_node(self, x):
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
        

    def print_node(self, node):
        if node is None:
            return 
        else:
          
            while(node != None):
                print(node.data, end=' ')
                node = node.next

    def length(self):
        temp = self.head
        if self.head is None:
            return 0
        else:
            count = 0
            while(temp):
                count += 1
                temp = temp.next
            return count     


    def delete_nth_node(self, nth_value):
        temp = self.head
        if self.head is None:
            return 
        if nth_value > self.length():
            return 
        pos = self.length() - nth_value + 1
        i = 1
        while(i < pos and temp.next is not None):
            prev = temp
            temp = temp.next
            i += 1
        prev.next = temp.next

arr = [3, 1, 6, 7, 8, 0, 1, 10, 1, 100, 21, 45, 67, 78]
l = linked_list()           
for x in range(len(arr)):
    l.add_node(arr[x])
l.print_node(l.head)    

print()
l.delete_nth_node(5)

l.print_node(l.head)