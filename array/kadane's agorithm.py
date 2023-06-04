def find_max_subarray(arr):
    size = len(arr)
    start = 0
    max_sum = float('-inf')
    max_array = []
    curr_sum = 0
    for i in range(size):
        curr_sum += arr[i]
        if curr_sum > max_sum:
            max_sum = curr_sum
            max_array = arr[start:i+1]
        if curr_sum < 0:
            curr_sum = 0
            start = i + 1
    return max_array

arr = [3, -1, 5, 4, 0, 1, -3, -2, 5]
print(find_max_subarray(arr))
  