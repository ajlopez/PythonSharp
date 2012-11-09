def factorial(n):
    "factorial function"
    result = 1
    while n>1:
        result = result * n
        n = n - 1
    return result

print(factorial(1))
print(factorial(2))
print(factorial(3))
print(factorial(4))

