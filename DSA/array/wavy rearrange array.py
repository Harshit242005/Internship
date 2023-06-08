# every odd index should be greater than their even index element and every even index element should be greater than odd index value
def rearrange_array(arr, size, start):
    if start > size:
        return arr
    if start == size:
        if size%2 == 0:
            if arr[start-1] > arr[start]:
                return rearrange_array(arr, size, start+1)
            else:
                x = arr[start-1]
                arr[start-1] = arr[start]
                arr[start] = x
                return rearrange_array(arr, size, start+1)
            
        else:
            if arr[start-1] < arr[start]:
                return rearrange_array(arr, size, start+1)
            else:
                x = arr[start-1] 
                arr[start-1] = arr[start]
                arr[start] = x
                return rearrange_array(arr, size, start+1)
    if start == 0:
        if arr[start] < arr[start+1]:
            return rearrange_array(arr, size, start+1)
        else:
            x = arr[start]
            arr[start] = arr[start+1]
            arr[start+1] = x
            return rearrange_array(arr, size, start+1)
    if start % 2 == 0 and start > 0:
        if arr[start] < arr[start-1] and arr[start] < arr[start+1]:
            return rearrange_array(arr, size, start+1)
        else:
            if arr[start] > arr[start-1]:
                x = arr[start-1] 
                arr[start-1] = arr[start]
                arr[start] = x
                return rearrange_array(arr, size, start+1)
            else:
                x = arr[start]
                arr[start] = arr[start+1]
                arr[start+1] = x
                return rearrange_array(arr, size, start+1)
            
    else:
        if arr[start] > arr[start-1] and arr[start] > arr[start+1]:
            return rearrange_array(arr, size, start+1)
        else:
            if arr[start] < arr[start-1]:
                x = arr[start-1] 
                arr[start-1] = arr[start]
                arr[start] = x
                return rearrange_array(arr, size, start+1)
            else:
                x = arr[start]
                arr[start] = arr[start+1]
                arr[start+1] = x
                return rearrange_array(arr, size, start+1)
            
arr = [1, 2, 3, 4, 5, 6]
size = len(arr)-1
start = 0
print(rearrange_array(arr, size, start))            