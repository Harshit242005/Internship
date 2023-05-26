class Node:
    def __init__(self, data):
        self.data = data
        self.next = None

class Linked_list:
    def __init__(self):
        self.head = None

    def add_node(self, data):
        new_node = Node(data)
        if self.head == None:
            self.head = new_node
            return 
        else:
            current = self.head
            while(current.next != None):
                current = current.next
            current.next = new_node
            new_node.next = None

    def print_nodes(self):
        if self.head == None:
            return 
        else:
            temp = self.head
            while(temp != None):
                print(temp.data, end=" ")
                temp = temp.next
    
    def delete_element(self, value):
        current = self.head
        if self.head is None:
            return 
        if self.head.data == value:
            self.head = current.next
            return
        else:
            temp = self.head
            while(temp.data != value and temp.next != None):
                prev_node = temp
                temp = temp.next
            prev_node.next = temp.next


    def delete_right_greater(self, start, second):
        if second is None:
            return 
        else:
            if start.data < second.data:
                self.delete_element(start.data)
                start = second
                second = second.next
                return self.delete_right_greater(start, second)
            else:
                start = second
                second = second.next
                return self.delete_right_greater(start, second)
            
if __name__ == "__main__":
    l = Linked_list()
    arr = [2, 4, 6, 3, 7, 9, 1, 0]
    for x in range(len(arr)):
        l.add_node(arr[x])
    l.print_nodes()
    print()
    x = l.head
    y = x.next
    l.delete_right_greater(x, y)         
    l.print_nodes()   