
# finding the least prime fatcor of all the number from 1 to n-1
def find_common(num):
    all_factors = []
    i = 2
    while i <= num:
        factors = list_prime(i)
        if factors:
            all_factors.append(min(factors))
        else:
            all_factors.append(i)
        i += 1
    all_factors.append(1)
    return all_factors


def list_prime(num):
    factors = []
    i = 2
    while i * i <= num:
        if num % i == 0:
            factors.append(i)
            num //= i
        else:
            i += 1
    if num > 1:
        factors.append(num)
    return factors


print(find_common(6))
