
import re

string = "20+5-3*2/4"
# Step 1: Separate numbers and signs using regular expressions
tokens = re.findall(r'\d+|[+\-*/]', string)
# Step 2: Iterate through the tokens and group consecutive numbers together
result = []
temp_num = ""
for token in tokens:
    if re.match(r'\d+', token):
        temp_num += token
    else:
        if temp_num:
            result.append(temp_num)
            temp_num = ""
        result.append(token)
if temp_num:
    result.append(temp_num)


print(len(result))
reverse_string = ""
i = len(result)-1
while(i >= 0):
    reverse_string += result[i]
    i -= 1
print(reverse_string)    
