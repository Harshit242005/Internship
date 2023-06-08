# getting back the smallest element in constant time comeplexity
class Stack:
    def __init__(self):
        self.main_stack = []
        self.min_stack = []

    def push(self, new_element):
        self.main_stack.append(new_element)
        if not self.min_stack or new_element <= self.min_stack[-1]:
            self.min_stack.append(new_element)

    def pop(self):
        if not self.main_stack:
            return None
        popped_element = self.main_stack.pop()
        if popped_element == self.min_stack[-1]:
            self.min_stack.pop()
        return popped_element

    def get_min(self):
        if not self.min_stack:
            return None
        return self.min_stack[-1]


stack = Stack()
arr = [5, 8, 9, 10, 3]
for x in range(len(arr)):
    stack.push(arr[x])
print(stack.get_min())    


# getting back the smallest element in constant time comeplexity
class Stack:
    def __init__(self):
        self.main_stack = []
        self.min_stack = []

    def push(self, new_element):
        self.main_stack.append(new_element)
        if not self.min_stack or new_element >= self.min_stack[-1]:
            self.min_stack.append(new_element)

    def pop(self):
        if not self.main_stack:
            return None
        popped_element = self.main_stack.pop()
        if popped_element == self.min_stack[-1]:
            self.min_stack.pop()
        return popped_element

    def get_min(self):
        if not self.min_stack:
            return None
        return self.min_stack[-1]


stack = Stack()
arr = [5, 8, 9, 10, 3]
for x in range(len(arr)):
    stack.push(arr[x])
print(stack.get_min())    


