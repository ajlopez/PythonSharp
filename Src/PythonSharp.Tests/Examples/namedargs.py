def foo(a=1, b=2):
    return a+b*2

print(foo()) # 5
print(foo(b=3)) # 7
print(foo(b=3,a=2)) # 8


