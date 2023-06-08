def array_puzzle(arr, size, start, empty):
    if start >= size:
        return empty
    else:
        count = 1
        i = 0
        while i < size:
            if i != start:
                count *= arr[i]
            i += 1    
        empty.append(count)
        return array_puzzle(arr, size, start+1, empty) 


arr = [10, 3, 5, 6, 2]
size = len(arr)
start = 0
empty = []
print(array_puzzle(arr, size, start, empty))
