# find the max sum value of the non adjcent element in a given array
def find_non_adjcent_array_sum(arr, size, start, new_arr, sum_value, diff):
    if start > size:
        new_arr.append(sum_value)
        if diff > size-1:
            ma_lenght = len(new_arr)-1
            new_arr.sort()
            print(new_arr)
            return new_arr[ma_lenght]
        else:
            diff += 1
            start = 0
            sum_value = 0
            return find_non_adjcent_array_sum(arr, size, start, new_arr, sum_value, diff)
    else:
        sum_value += arr[start]
        start += diff   
        return find_non_adjcent_array_sum(arr, size, start, new_arr, sum_value, diff) 
    
arr = [3, -2, 1, 0, 5, 4, -2, 7]
size = len(arr)-1
start = 0
new_arr = [ ]
sum_value = 0
diff = 2
print("the max value of sum of non adjcent element is : ", find_non_adjcent_array_sum(arr, size, start, new_arr, sum_value, diff))    