def count_inversion(arr, size, start, second, count):
    if second > size:
        if start > size-1:
            return count
        else:
            start += 1
            second = start + 1
            return count_inversion(arr, size, start, second, count)
        
    else:
        if arr[start] > arr[second]:
            count += 1
            return count_inversion(arr, size, start, second+1, count)
        else:
            return count_inversion(arr, size, start, second+1, count)

arr = [2, 4, 1, 3, 5]
size = len(arr)-1
start = 0
second = 1
count = 0
print(count_inversion(arr, size, start, second, count))            

# merge sort applied count inversion to get the result in O(nlongn) time complexity
def merge_sort(arr):
    if len(arr) <= 1:
        return arr, 0

    mid = len(arr) // 2
    left, inv_left = merge_sort(arr[:mid])
    right, inv_right = merge_sort(arr[mid:])

    merged, inv_merge = merge(left, right)

    inversions = inv_left + inv_right + inv_merge

    return merged, inversions


def merge(left, right):
    merged = []
    inversions = 0
    i = j = 0

    while i < len(left) and j < len(right):
        if left[i] <= right[j]:
            merged.append(left[i])
            i += 1
        else:
            merged.append(right[j])
            j += 1
            inversions += len(left) - i

    merged.extend(left[i:])
    merged.extend(right[j:])

    return merged, inversions


arr = [2, 3, 4, 5, 6]
sorted_arr, inversions = merge_sort(arr)
print(sorted_arr)

print("Number of inversions:", inversions)
