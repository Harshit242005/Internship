
def merge_sorted_arrays(arr1, arr2, kth):
    merged_array = []
    i = 0  # Pointer for the first array
    j = 0  # Pointer for the second array

    # Compare elements from both arrays and add them to the merged array
    while i < len(arr1) and j < len(arr2):
        if arr1[i] <= arr2[j]:
            merged_array.append(arr1[i])
            i += 1
        else:
            merged_array.append(arr2[j])
            j += 1

    # Add the remaining elements from the first array, if any
    while i < len(arr1):
        merged_array.append(arr1[i])
        i += 1

    # Add the remaining elements from the second array, if any
    while j < len(arr2):
        merged_array.append(arr2[j])
        j += 1

    return merged_array[kth-1]


arr_1 = [2, 3, 6, 7, 9]
arr_2 = [1, 4, 8, 10]

print(merge_sorted_arrays(arr_1, arr_2, 5))