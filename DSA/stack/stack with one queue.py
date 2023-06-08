# implementing stack with queue
class Node:
    def __init__(self, data):
        self.data = data
        self.next = self.prev = None

class Queue:
    def __init__(self):
        self.head = self.tail = None 

    def push(self, x):
        new_node = Node(x)
        if self.head is None:
            self.head = self.tail  =  new_node
            new_node.prev = self.head
            return
        else:
            self.tail.next = new_node
            new_node.next = None
            self.tail = new_node
            return 
        
    def move_head_last(self):
        temp = self.head
        if self.head is None:
            return 
        else:
            self.head = temp.next
            self.tail.next = temp
            temp.next = None
            self.tail = temp
            return

    def print_node(self):
        if self.head is None:
            return 
        else:
            temp = self.head
            while(temp != None):
                print(temp.data, end=" ")
                temp = temp.next

    def count_node(self):
        if self.head is None:
            return 0
        else:
            temp = self.head
            count = 0
            while(temp != None):
                count += 1
                temp= temp.next
            return count      

    def pop(self):
        temp = self.head
        if self.head is None:
            return 
        if self.count_node() == 1:
            return self.head.data
        else:
            i = 1
            length = self.count_node()
            while(i <= length-1):
                self.move_head_last()
                i += 1
            print(self.head.data)
            self.head = temp.next   
            return                            

if __name__ == "__main__":
    l = Queue()
    for x in range(1, 11):
        l.push(x)
    l.pop()
    l.pop()
    