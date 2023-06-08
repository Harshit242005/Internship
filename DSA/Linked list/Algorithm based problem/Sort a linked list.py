# my first approach would be of using the two for loop nested and in that 

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
        

    def sort_list(self, start, second):
        if self.head is None:
            return 
        if second.next is None:
            if start.next.next is None:
                return 
            else:
                start = start.next
                second = start.next    
                return self.sort_list(start, second)
        else:
            if start.data > second.data:
                start.data, second.data = second.data, start.data
                return self.sort_list(start, second.next)
            else:
                return self.sort_list(start, second.next)


l = linked_list()
arr = [2, 1, 6, 3, 5, 9, 7]
for x in range(len(arr)):
    l.add_node(arr[x])
l.print_node(l.head)  
start = l.head
second = start.next
l.sort_list(start, second)
print()
l.print_node(start)