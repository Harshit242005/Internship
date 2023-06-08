# implementing stack with help of class and list data structures

class Stack():
    def __init__(self):
        self.items = [ ]

    def push(self, x):
        self.items.append(x)

    def pop(self):
        return self.items.pop()

    def size(self):
        if self.pop() == None:
            return 0
        else:
            return len(self.items)        
        
    def is_empty(self):
        if self.pop() == None:
            return True
        else:
            return False
        
    def top(self):
        if self.is_empty():
            return None
        else:
            return self.pop()    

if __name__ == "__main__":
    s = Stack()
    for x in range(1, 11):
        s.push(x)
    for x in range(1, 6):
        print(s.pop(), end=" ")            