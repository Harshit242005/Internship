# in a given array find out the maximum product of two number 
# by this you can find out the addition, subtraction, and divide
def get_max_product(arr, size):
    arr.sort()
    return (arr[size] * arr[size-1])

arr = [2, 1, 3, 0, 4]
size = len(arr)-1
print(get_max_product(arr, size))                   