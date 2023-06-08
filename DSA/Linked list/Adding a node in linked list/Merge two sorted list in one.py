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
        
    def length(self, node):
        count = 0
        
        if node is None:
            return 0
        else:
            while(node):
                count += 1
                node = node.next
            return count


    def merge_lists(self, node_1, node_2):
        if node_1 is None:
            return node_2
        if node_2 is None:
            return node_1

        if node_1.data <= node_2.data:
            merged_head = node_1
            node_1 = node_1.next
        else:
            merged_head = node_2
            node_2 = node_2.next

        current = merged_head

        while node_1 is not None and node_2 is not None:
            if node_1.data <= node_2.data:
                current.next = node_1
                node_1 = node_1.next
            else:
                current.next = node_2
                node_2 = node_2.next
            current = current.next

        if node_1 is not None:
            current.next = node_1
        if node_2 is not None:
            current.next = node_2

        return self.print_node(merged_head)



l1 = Linked_list()
l2 = Linked_list()
arr_1 = [1, 3, 5, 7]
arr_2 = [2, 4, 6]
for x in range(len(arr_1)):
    l1.Add_node(arr_1[x])
for x in range(len(arr_2)):
    l2.Add_node(arr_2[x])

l1.print_node(l1.head)
print()
l2.print_node(l2.head)                                 

print()
l1.merge_lists(l1.head, l2.head)