def get_lenght_array(arr, size, start, second, target, new_array, flag=0):
    if second > size or flag == 1:
        if second > size and flag == 0:
            if start > size-1:
                return "we are out of the array"
            else:
                start += 1
                second = start + 1
                new_array.clear()
                return get_lenght_array(arr, size, start, second, target, new_array, flag=0)

        if flag == 1:
            return len(new_array)
    else:
        
        print("sum of the array is : ", sum(new_array))
        print(new_array)
        if sum(new_array) >= target:
            return get_lenght_array(arr, size, start, second, target, new_array, flag=1)
        else:
            new_array.append(arr[start])
            new_array.append(arr[second])
            get_lenght_array(arr, size, start, second+1, target, new_array, flag=0)

arr = [2, 3, 1, 7, 5, 0, 1]
size = len(arr)-1
start = 0
second = 1
target = 6
new_array = [ ]

print(get_lenght_array(arr, size, start, second, target, new_array))                     