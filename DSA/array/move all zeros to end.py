def move_all_zeros_to_end(arr, size, start, count):
    if start > size:
        for x in range(count):
            arr.append(0)
        return arr
    else:
        if arr[start] == 0:
            count += 1
            arr.pop(start)
            return move_all_zeros_to_end(arr, size-1, start+1, count)
        else:
            return move_all_zeros_to_end(arr, size, start+1, count)  

arr = [2, 0, 6, 0, 2, 0, 1, 1, 0]
size = len(arr)
start = 0
count = 1
print(move_all_zeros_to_end(arr, size, start, count))