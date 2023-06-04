# easy 
# find the missing element on array whose in n
# example = n = 4 {2, 1, 4} result = 3
# we can solve for more than one also

def find_missing(arr, size):
    arr.sort()
    missing = []
    all = []
    i = 1
    while(i <= size):
        all.append(i)
        i += 1
    for x in range(len(all)):
        if all[x] not in arr:
            
            missing.append(all[x])
    return missing        

arr = [3, 2, 1]
size = 7
print(find_missing(arr, size))

# improved version

def find_missing(arr, size):
    missing = []
    i = 1
    while(i <= size):
        if i not in arr:
            missing.append(i)
        i += 1
    return missing

print(find_missing([2, 3, 1], 4))     