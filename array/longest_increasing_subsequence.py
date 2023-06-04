# given an array you have to find the subsequence that has longest length 
def find_longest_subsequence(arr, size, start, second, count, new_arr):
    if second > size:
        new_arr.append(count)
        if start > size-1:
            lenght = len(arr)-1
            new_arr.sort()
            return new_arr[lenght]
        else:
            start += 1
            second = start + 1
            count = 0
            return find_longest_subsequence(arr, size, start, second, count, new_arr)
    else:
        if arr[start] < arr[second]:
            count += 1
            second += 1
            return find_longest_subsequence(arr, size, start, second, count, new_arr)
        else:
            new_arr.append(count)
            count = 0
            start += 1
            second = start + 1
            return find_longest_subsequence(arr, size, start, second, count, new_arr)

arr = [1, 4, 2, 7, 5, 0, 7]
size = len(arr)-1
start = 0
second = 1
count = 0
new_arr = []            
print(find_longest_subsequence(arr, size, start, second, count, new_arr))