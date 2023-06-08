# we have given two sorted arrays in that we have to merge them and make a single array 
def merge_array(arr_1, arr_2, size_1, size_2, start_1, start_2, new_arr):
    if size_1 < 0:
        for x in range(size_2+1):
            new_arr.append(arr_2[x])
        return new_arr    
    if size_2 < 0:
        for x in range(size_1+1):
            new_arr.append(arr_1[x])    
        return new_arr    
    else:
        if arr_1[start_1] < arr_2[start_2]:
            #print(arr_1[start_1])
            new_arr.append(arr_1[start_1])
            arr_1.pop(start_1)
            return merge_array(arr_1, arr_2, size_1-1, size_2, start_1, start_2, new_arr)
        else:
            print(arr_2[start_2])
            new_arr.append(arr_2[start_2])
            arr_2.pop(start_2)
            return merge_array(arr_1, arr_2, size_1, size_2-1, start_1, start_2, new_arr)

arr_1 = [2, 4, 6]
arr_2 = [1, 3, 5]
size_1 = len(arr_1)-1
size_2 = len(arr_2)-1
start_1 = 0
start_2 = 0
new_arr = []
print(merge_array(arr_1, arr_2, size_1, size_2, start_1, start_2, new_arr))