def foo(a=1, b=2):
    return a+b

print(foo()) # 3
print(foo(a=2)) # 4
print(foo(b=3,a=2)) # 5


