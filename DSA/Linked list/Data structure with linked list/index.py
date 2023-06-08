#find the slowest
#find common factor
# def find_common_factor(num_1, num_2):
#     i = 1
#     common_factor = []
#     check_value = ""
#     if num_1 > num_2:
#         check_value += str(num_2)
#     else:
#         check_value += str(num_1)  
   
#     while(i <= int(check_value)):
#         if num_1 % i == 0 and num_2 % i == 0:
#             common_factor.append(i)
#         i += 1
 
#     return len(common_factor)    


# num_1 = 10
# num_2 = 20

# print(find_common_factor(num_1, num_2))


# # number to  replace that the sum of whole arr should be greater than current 
# def find_num(arr, size):
#     sum_value = sum(arr)
#     print(f"the sum value is {sum_value}")
#     i = 1
#     while(i <= size):
#         if i * size > sum_value:
            
#             print(f"the sum value of this {i * size}")
#             return i
#         i += 1

# arr = [0, 0, 0]
# size = len(arr)
# print(find_num(arr, size)) 


