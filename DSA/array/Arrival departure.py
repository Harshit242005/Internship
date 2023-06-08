# Given arrival and departure times of all trains that reach a railway station. Find the minimum number of platforms required for the railway station so that no train is kept waiting.
# example array = arr[] = {0900, 0940, 0950, 1100, 1500, 1800}
#dep[] = {0910, 1200, 1120, 1130, 1900, 2000}

def find_number_of_stations(arrival, departure, arr_start, dep_start, size, count):
    if arr_start > size:
        if count == 0:
            return 1
        else:
            return count
    else:
        if arrival[arr_start] < departure[dep_start]:
            count += 1
            return find_number_of_stations(arrival, departure, arr_start+1, dep_start+1, size, count)
        else:
            return find_number_of_stations(arrival, departure, arr_start+1, dep_start+1, size, count)
        
arrival = [900, 1100, 1235]   
departure = [1000, 1200, 1240]
size = len(arrival)-1
arr_start = 1
dep_start = 0
count = 0
print(find_number_of_stations(arrival, departure, arr_start, dep_start, size, count))   