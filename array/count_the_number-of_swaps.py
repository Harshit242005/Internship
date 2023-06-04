def find_the_count_of_swap(arr, size, start, second, count):
    if second > size:
        if start > size-1:
            print(arr)
            print()
            return count
        else:
            start += 1
            second = start + 1
            return find_the_count_of_swap(arr, size, start, second, count)
    else:
        if arr[start] < arr[second]:
            return find_the_count_of_swap(arr, size, start, second+1, count)  
        else:
            x = arr[start]
            arr[start] = arr[second]
            arr[second] = x
            return find_the_count_of_swap(arr, size, start, second+1, count+1) 

arr = [4, 3, 5, 2, 1, 7, 6, 8]
size = len(arr)-1
start = 0
second = 1
count = 1
print(find_the_count_of_swap(arr, size, start, second, count))         