def remove_all_duplicate(get_string):
    ge_string_list = list(get_string)
    get_string_set = set(ge_string_list)
    get_string_new_list = list(get_string_set)
    for x in range(len(get_string_new_list)):
        print(get_string_new_list[x], end="")

get_string = "abcabcddea"
remove_all_duplicate(get_string)        