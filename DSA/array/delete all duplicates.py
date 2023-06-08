def delete_all_duplicate(arr, start, second, size):
    if second > size:
        if start > size-1:
            return arr
        else:
            start += 1
            second = start + 1
            return delete_all_duplicate(arr, start, second, size)
    else:
        if arr[start] == arr[second]:
            arr.pop(second)
            second = start+1
            return delete_all_duplicate(arr, start, second, size-1)
        else:
            return delete_all_duplicate(arr, start, second+1, size)

arr = [3, 2, 1, 3, 9, 0, 3, 2, 3, 1, 3]
size = len(arr)-1
start = 0
second  = 1
print(delete_all_duplicate(arr, start, second, size))