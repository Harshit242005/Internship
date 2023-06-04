def find_max(arr, size, k, start, element):
    if k > size:
        return 
    else:
        element = arr[start]
        i = start
        while(i < k):
            if element < arr[start+1]:
                element = arr[start+1]
            i += 1    
        print(element, end=" ")
        return find_max(arr, size, k+1, start+1, element) 

arr = [1,3,-1,-3,5,3,6,7]
size = len(arr)
k = 3
start = 0
element = 0
find_max(arr, size, k, start, element)  