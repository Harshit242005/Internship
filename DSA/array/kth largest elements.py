# Given an array Arr of N positive integers and an integer K, find K largest elements from the array.  The output elements should be printed in decreasing order.
def kth_largest_numbers(arr, num):
    arr.sort()
    size = len(arr)-1
    last = size - num
    while(size > last):
        print(arr[size], end=" ")
        size -= 1
arr = [1, 23, 12, 9, 30, 2, 50]
num = 3
kth_largest_numbers(arr, num)     