# reverseing a stack using another stack
class reverse_stack:
    def __init__(self):
        self.s1 = [ ]
        self.s2 = [ ]

    def push(self, x):
        self.s1.append(x)

    def pop(self):
        while(len(self.s1) != 0):
            self.s2.append(self.s1.pop()) 
        return self.s2.pop()

    def get_size(self):
        if len(self.s1) == 0:
            return 0
        else:
            return len(self.s1)

if __name__ == "__main__":
    r = reverse_stack()
    for x in range(1, 11):
        r.push(x)
    print(r.pop())                    