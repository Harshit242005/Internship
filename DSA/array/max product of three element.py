# in the given array find out the max product of the three element
# for this i will use .sort() function and multiply the last three element 
def max_product(arr, size):
    arr.sort()
    return arr[size] * arr[size-1] * arr[size-2]

arr = [3, 0, 1, 4, -1, 5]
size = len(arr)-1
print(f"the max product of the {arr} is {max_product(arr, size)}")

# for two element you can do the same just use size and size-1

def max_product(arr, size):
    arr.sort()
    return arr[size] * arr[size-1]

arr = [3, 0, 1, 4, -1, 5]
size = len(arr)-1
print(f"the max product of the {arr} is {max_product(arr, size)}")