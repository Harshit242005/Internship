
def number_of_jumps(arr, size, start, count):
    if start >= size:
        if count:
            return count
        return -1
    else:
        prev = arr[start]
        start = start + prev
        count += 1
        return number_of_jumps(arr, size, start, count)
    

arr = [1, 2, 3, 4, 5]
size = len(arr)-1
start = 0
count = 0
print(number_of_jumps(arr, size, start, count))