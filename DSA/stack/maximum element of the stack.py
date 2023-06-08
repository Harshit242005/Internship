# function to get the maximum element in stack
class Stack:
    def __init__(self):
        self.items = [ ]
        # we have defined an list to appned and pop out elements

    def push(self, x):
        self.items.append(x)

    def pop(self):
        if len(self.items) == 0:
            print("stack is empty")
            return 
        else:
            return self.items.pop()

    def size(self):
        if len(self.items) == 0:
            return 0
        else:
            return len(self.items)

    def get_min_element(self):
        if self.size() == 0:
            return 
        else:
            start_element = self.items.pop()
            check_elemnt = self.pop()
            i = self.size()
            
            while(i != 0 and check_elemnt):
                if start_element < check_elemnt:
                    start_element = check_elemnt 
                check_elemnt = self.pop()

            return start_element

    def print_element(self):
        print(self.items)


if __name__ == "__main__":
    s = Stack()
    s.push(2)
    s.push(1)
    s.push(5)
    s.push(7)
    s.push(4)
    s.print_element()
    print()
    print(s.get_min_element())   
    
     
