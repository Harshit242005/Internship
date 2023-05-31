class Node:
    def __init__(self, data):
        self.data = data
        self.next = self.prev = None

class Linked_list:
    def __init__(self):
        self.head = self.prev = None
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
        
    def print_node(self, head):
        if head is None:
            return
        else:
            while head:
                print(head.data, end=" ")
                head = head.next

    def delete_head(self):
        temp = self.head
        if temp is not None:
            self.head = temp.next
            return             

    def delete_last(self):
        if self.head is None:
            return None
        elif self.head == self.tail:
            # Special case: Only one node in the list
            popped_value = self.head.data
            self.head = self.tail = self.second_tail = None
            return popped_value
        else:
            self.tail = self.second_tail
            self.tail.next = None
            self.tail.prev = self.second_tail.prev
            self.second_tail = self.tail.prev
            return 
        
    def delete_pos(self, pos):
        temp = self.head
        if self.head is None:
            return 
        if pos == 1:
            self.delete_head()
        elif pos == 2:
            self.delete_last()
        else:
            i = 1
            while i < pos and temp is not None:
                prev = temp
                i += 1
                temp = temp.next
                
            if temp is None or temp.next is None:
                prev.next = None    
            else:
                prev.next = temp.next
            return 

    def delete_duplicate(self, head):
        if head is None:
            return
        current = head
        while current:
            runner = current
            while runner.next:
                if runner.next.data == current.data:
                    runner.next = runner.next.next
                else:
                    runner = runner.next
            current = current.next

    def delete_duplicates(self):
        self.delete_duplicate(self.head)

l = Linked_list()
arr = [2, 5, 4, 5, 7, 8, 9, 1, 2, 1, 4, 7]
for x in range(len(arr)):
    l.add_node(arr[x])

l.print_node(l.head)
print()

l.delete_duplicates()

l.print_node(l.head)
