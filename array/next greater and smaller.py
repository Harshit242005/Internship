#  find out the next greater and next smaller element of every element in a given array
def find_next_element(arr, size, start, second):
    if second > size:
        if start > size-1:
            print(-1, end=" ")
            return 
        else:
            print(-1, end=" ")
            start += 1
            second = start + 1
            find_next_element(arr, size, start, second) 

    if arr[second] > arr[start]:
        print(arr[second], end=" ")
        start += 1
        second = start + 1
        return find_next_element(arr, size, start, second)
    else:
        return find_next_element(arr, size, start, second+1)

arr = [1, 3, 2, 4]
size = len(arr)-1
start = 0
second  = 1
find_next_element(arr, size, start, second)