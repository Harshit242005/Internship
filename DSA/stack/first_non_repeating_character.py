# given a string you have to find the first non repeating charatcer by using the stack
class Stack:
    def __init__(self):
        self.items_1 = []
        self.items_2 = []

    def push(self, x):
        self.items_1.append(x)

    def pop_1(self):
        if len(self.items_1) == 0:
            print("stack is empty")
            return 
        else:
            return self.items_1.pop()  
        
    def size_1(self):
        if len(self.items_1) == 0:
            return 0
        else:
            return len(self.items_1)    
        
    def size_2(self):
        if len(self.items_2) == 0:
            return 0
        else:
            return len(self.items_2)    

    def print_elements(self):
        if len(self.items_1) == 0:
            return 
        else:
            print(self.items_1)

    def get_first_non_repeating(self):
        start_element = self.items_1.pop()
        second_element = self.items_1.pop()
        i = self.size_1()
        while(i != 0 and second_element):
            if start_element != second_element:
                self.items_2.append(second_element)
            second_element = self.pop_1()  
        print(self.items_2)
        return


if __name__ == "__main__":
    s = Stack()
    get_string = "abcad"
    for x in range(len(get_string)):
        s.push(get_string[x])  
    print(s.get_first_non_repeating)           
                        