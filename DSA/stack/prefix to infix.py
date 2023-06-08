# prefix to infix
def prfix_to_infix(prefix):
    stack = []
    i = len(prefix)-1
    while(i >= 0):
        if not isOperator(prefix[i]):
            stack.append(prefix[i])
            i -= 1
        else:
            str = "(" + stack.pop() + prefix[i] + stack.pop() + ")"
            stack.append(str)
            i -= 1

    return stack.pop()

def isOperator(c):
    if c == "*" or c == "+" or c == "-" or c == "/" or c == "^" or c == ")" or c == "(":
        return True
    else:
        return False
if __name__ == "__main__":
    str = "*-a/bc-/akl"
    print(prfix_to_infix(str))                