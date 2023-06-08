# reversing the nodes inside a linked list

# first understand how to reverse the linked list
class Node:
    def __init__(self, data):
        self.data = data
        self.next = self.prev = None

class linked_list:
    def __init__(self):
        self.head = self.prev = None

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
            self.tail = new_node
            return 
        

    def print_node(self, node):
        if node is None:
            return 
        else:
          
            while(node != None):
                print(node.data, end=' ')
                node = node.next

   # let's use recursion here
    def pair_wise_reverse(self, start, second):
        if start is None or second is None:
            return start

        if second.next is None:
            if start.next.next is None:
                return start
            else:
                start.next = start.next.next
                second = start.next
        else:
            if start.data % 2 == 0:
                while second is not None and second.data % 2 != 0:
                    second = second.next
                if second is not None:
                    start.data, second.data = second.data, start.data
                    start = second.next
                    second = start.next
                    self.pair_wise_reverse(start, second)
            else:
                self.pair_wise_reverse(start.next, second.next)

        return 



    def reverse_list(self):
        if self.head is None:
            return 
        

        else:
            start = self.head
            end = self.tail
            print(start.data, end.data)
            while(start != end):
                start.data, end.data = end.data, start.data 
                start = start.next
                end = end.prev

            return start


l = linked_list()            
arr = [3, 6, 1, 8,9, 12, 1, 14, 7, 9, 0, 13]
for x in range(len(arr)):
    l.add_node(arr[x])


l.print_node(l.head)
print()
start = l.head
second = start.next
l.pair_wise_reverse(start, second)      
l.print_node(l.head)


