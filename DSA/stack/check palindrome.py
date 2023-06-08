# function to check if the string is palindrome using the stack
class Stack:
    def __init__(self):
        self.s1 = []

    def check_palindrome(self, get_string):
        if get_string is None:
            return None
        else:
            for x in range(len(get_string)):
                self.s1.append(get_string[x])

            output_string = ""
            for y in range(len(self.s1)):
                output_string += self.s1.pop(len(self.s1)-1)
            if output_string == get_string:
                return "yes"
            else:
                return "no"

if __name__ == "__main__":
    s = Stack()
    print(s.check_palindrome("racecar")) 




