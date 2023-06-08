# Given an array arr[] of N non-negative integers representing the height of blocks. If width of each block is 1, compute how much water can be trapped between the blocks during the rainy season.
def find_block_of_water(arr, size):
    start = arr[0]
    i = 1
    water = 0
    while(i <= size):
        if arr[i] == 0:
            water += start
            
        else:
            if arr[i] > start:
                water += 0
            else:
                water += start - arr[i]
        i += 1
    return water

arr = [3,0,0,2,0,4]
size = len(arr)-1
print(find_block_of_water(arr, size))
