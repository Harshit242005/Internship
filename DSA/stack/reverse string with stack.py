class Stack:
    def __init__(self):
        self.items = [ ]

    def push(self, x):
        self.items.append(x)

    def pop(self):
        return self.items.pop()
    
    def reverse_string(self, get_string):
        new_string = ""
        for x in range(len(get_string)):
            s.push(get_string[x])

        for x in range(len(get_string)):
            new_string += s.pop()

        return new_string        
        

if __name__ == "__main__":
    s = Stack()
    get_string = "abc"
    print(f"the reverse of the string ' {get_string} ' is : ", s.reverse_string(get_string))