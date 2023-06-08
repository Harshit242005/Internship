# remove consecutive sum
def remove_same_word(word):
    stack = []
    for i in range(len(word)):
        if len(stack) == 0:
            stack.append(word[i])
        else:
            Str = stack[-1]
            if Str == word[i]:
                stack.pop()    


            else:
                stack.append(word[i])

    return len(stack)

if __name__ == "__main__":
    v = ["aa", "bc", "bc", "ad"]
    print(remove_same_word(v))    
                    