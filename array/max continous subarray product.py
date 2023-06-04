
def find(arr, size, start, new_value, product, value):
    if start > size:
        max_pair = max(new_value.items(), key=lambda x: x[0])

# print the maximum key-value pair
        return max_pair
    if arr[start] > 0:
        value.append(arr[start])
        product *= arr[start]
        i = start+1
        while(i <= size and arr[i] > 0):
            product *= arr[i]
            value.append(arr[i])
            i += 1
        start = i
        new_value[product] = value
        product = 1
        value = []
        return find(arr, size, start, new_value, product, value)
    else:
        product = 1
        value = []
        return find(arr, size, start+1, new_value, product, value)        

arr = [2, 3, -2, 4, 5, 6]
size = len(arr)-1
start = 0
new_value = {}
product = 1
value = []   
print(find(arr, size, start, new_value, product, value))
