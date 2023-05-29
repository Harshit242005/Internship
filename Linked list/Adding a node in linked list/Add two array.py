class Node:
    def __init__(self, data):
        self.data = data
        self.next = self.prev = None

class Linked_list:
    def __init__(self):
        self.head = self.tail = None

    def Add_node(self, x):
        new_node = Node(x)
        if self.head is None:
            self.head = self.tail = new_node
            new_node.prev = None
            return 
        else:
            self.tail.next = new_node
            new_node.prev = self.tail
            self.tail = new_node
            return  
        
    def print_node(self, node):
        if node is None:
            return 
        else:
            while(node):
                print(node.data, end=' ')
                node = node.next 

    def make_list(self, arr):
        if arr is None:
            return 
        else:
            l = Linked_list()
            for x in range(len(arr)):
                
                l.Add_node(arr[x])   
            l.print_node(l.head)           
    

    def add_two_array(self, arr_1, arr_2):
        
        num_1 = ""
        num_2 = ""
        for x in range(len(arr_1)):
            num_1 += str(arr_1[x])
        for x in range(len(arr_2)):
            num_2 += str(arr_2[x])    
        result = int(num_1) + int(num_2)
        str_result = str(result)
        print(f'the result is {result}')
        list_reslut = []
        for x in range(len(str_result)):
            list_reslut.append(int(str_result[x]))
        return self.make_list(list_reslut)        

l = Linked_list()
arr_1 = [3, 2, 6, 7, 8, 10]
arr_2 = [4, 6, 2, 1, 4, 10]
l.add_two_array(arr_1, arr_2)