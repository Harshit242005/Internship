# find the next greater element
# it's the most optimize algorithm 

def get_next_greater_element(arr, size, start, second):
    if second > size:
        print(-1, end=" ")
    else:
        if arr[start] < arr[second]:
            print(arr[second], end=" ")
            start += 1
            second = start + 1
            return get_next_greater_element(arr, size, start, second)
        else:
            return get_next_greater_element(arr, size, start, second+1)    
            
arr = [4, 3, 5, 2, 1, 7, 6, 8] 
size = len(arr)-1
start = 0
second = 1
get_next_greater_element(arr, size, start, second)           