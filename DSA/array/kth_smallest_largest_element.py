def find_kth_smallest_larget_element(arr, size, kth):
    if kth < 1:
        return 
    else:
        arr.sort()
        print(f"the {kth} smallest element is {arr[kth]}")
        print()
        print(f"the {kth} larget element is {arr[size-kth]}")

arr = [3, 2, 9, 0, 1, 7]
size = len(arr)-1
kth = 1
find_kth_smallest_larget_element(arr, size, kth) 