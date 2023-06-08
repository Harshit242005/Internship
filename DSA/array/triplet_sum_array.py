# given a target value find the triplet value whose sum is equal to the give target value
def find_triplet(arr, size, start, second, third, target):
    if third > size:
        if second > size-1:
            if start > size-2:
                return 
            else:
                start += 1
                second = start + 1
                third = second + 1
                return find_triplet(arr, size, start, second, third, target)
        else:
            second += 1
            third = second + 1
            return find_triplet(arr, size, start, second, third, target)
    else:
        if arr[start] + arr[second] + arr[third] == target:
            print([arr[start], arr[second], arr[third]], end=' ')
            return find_triplet(arr, size, start, second, third+1, target)
        else:
            return find_triplet(arr, size, start, second, third+1, target)   

arr = [4, 2, 0, -1, -3, 6, 5]
size = len(arr)-1
start = 0
second = 1
third = 2
target = 2
find_triplet(arr, size, start, second, third, target)         