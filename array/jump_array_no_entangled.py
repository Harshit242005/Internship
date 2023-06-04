def jump_out_array(arr, size, start, second, jump):
    if start >= size:
        return jump
    else:
        second = start
        print(second)
        start = arr[start] + second
        print(start)
        return jump_out_array(arr, size, start, second, jump+1)
    
arr = [1, 2, 3, 4, 5, 6, 7, 7, 8]
size = len(arr)-1
second  = 0
start = 0
jump = 0
print(f"the jump required in the {[2, 3, 1, 1, 4]} is {jump_out_array(arr, size, start, second, jump)}") 