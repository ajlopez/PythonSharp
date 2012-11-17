"Native threads module"

from System.Threading import Thread
from PythonSharp.Language import FunctionWrapper

def start(function):
    wrapper = FunctionWrapper(function, locals())
    ts = wrapper.CreateThreadStart()
    thread = Thread(ts)
    thread.Start()
    return thread
    
