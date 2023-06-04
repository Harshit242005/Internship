# for a given array remove all the adjcent element and count all the removed element 
def remove_adjcent(arr, size, start, second, count):
    if second > size:
        return count 
    else:
        if arr[start] == arr[second]:
            arr.pop(second)
            start += 1
            second = start+1
            size = size-1
            return remove_adjcent(arr, size, start, second, count+1)
        else:
            start += 1
            second += 1
            return remove_adjcent(arr, size, start, second, count)
        
arr = [1, 2, 3, 2, 3, 2, 4, 4, 5, 4, 4]
size = len(arr)-1
start = 0
second = 1
count = 0
print(remove_adjcent(arr, size, start, second, count))        