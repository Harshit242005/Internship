def get_clean_array(arr, size, start, value):
    if start > size:
        return 
    else:
        if arr[start] == value:
            return get_clean_array(arr, size, start+1, value)
        else:
            print(arr[start], end=" ")
            return get_clean_array(arr, size, start+1, value)
        
arr = [3, 2, 1, 3, 9, 0, 3, 2, 3, 1, 3]
size = len(arr)-1
start = 0
value = 3
get_clean_array(arr, size, start, value)