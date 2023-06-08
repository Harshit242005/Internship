class Node:
    def __init__(self, data):
        self.data = data
        self.next = None

class Stack:
    def __init__(self):
        self.head = None

    def push(self, x):
        new_node = Node(x)
        if self.head is None:
            self.head = new_node
        else:
            new_node.next = self.head
            self.head = new_node

    def pop(self):
        if self.head is None:
            return None
        else:
            popped_data = self.head.data
            self.head = self.head.next
            return popped_data

    def top(self):
        if self.head is None:
            return None
        else:
            return self.head.data

    def size(self):
        count = 0
        temp = self.head
        while temp:
            count += 1
            temp = temp.next
        return count


def sort_stack(stack_1):
    stack_2 = Stack()
    while not stack_1.size() == 0:
        temp = stack_1.pop()
        while not stack_2.size() == 0 and stack_2.top() > temp:
            stack_1.push(stack_2.pop())
        stack_2.push(temp)

    return stack_2


stack1 = Stack()

arr = [4, 2, 8, 6, 5, 1, 7]
for x in arr:
    stack1.push(x)

sorted_stack = sort_stack(stack1)
while sorted_stack.size() > 0:
    print(sorted_stack.pop())
